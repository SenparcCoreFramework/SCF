var vm = new Vue({
    el: "#app",
    data() {
        return {
            //分页参数
            paginationQuery: {
                total: 5
            },
            //分页接口传参
            listQuery: {
                pageIndex: 1,
                pageSize: 20,
                roleName: '',
                orderField: ''
            },
            tableData: [],
            dialog: {
                title: '新增角色',
                visible: false,
                data: {
                    roleName: '', roleCode: '', adminRemark: '', remark: '', addTime: '', id: '', enabled: false
                },
                rules: {
                    roleName: [
                        { required: true, message: "角色名称为必填项", trigger: "blur" }
                    ],
                    roleCode: [{ required: true, message: "角色代码为必填项", trigger: "blur" }]
                },
                updateLoading: false
            },
            // 树结构字段
            defaultProps: {
                children: 'children',
                label: 'menuName'
            },
            // 授权
            au: {
                title: '',
                visible: false,
                updateLoading: false,
                temp: {}
            },
            allMenu: [],// 所有权限
            currMenu: [],// 当前权限
            defaultExpandedKeys:[], // 默认展开
            defaultCheckedKeys: [] // 默认选中
        };
    },
    created: function () {
        this.getList();
    },
    watch: {
        'dialog.visible': function (val, old) {
            // 关闭dialog，清空
            if (!val) {
                this.dialog.data = {
                    roleName: '', roleCode: '', adminRemark: '', remark: '', addTime: '', id: ''
                };
                this.dialog.updateLoading = false;
            }
        },
        'au.visible': function (val, old) {
            // 关闭dialog，清空
            if (!val) {
                this.currMenu = [];
                this.defaultCheckedKeys = [];
                this.defaultExpandedKeys = [];
                this.au.updateLoading = false;
            }
        }
    },
    methods: {
        // 权限
        async handleRole(index, row) {
            // 打开dialog
            this.au = {
                title: row.roleName,
                visible: true,
                temp: row
            };
            // 获取当前已有权限
            const c = await service.get(`/Admin/Role/Permission?handler=RolePermission&roleId=${row.id}`);
            this.currMenu = c.data.data;
            let defaultCheckedKeys = [];
            this.currMenu.map(res => {
                // 默认选中权限
                defaultCheckedKeys.push(res.permissionId);
            });
            this.defaultCheckedKeys = defaultCheckedKeys;
            // 获取所有权限
            const a = await service.get('/Admin/Menu/Edit?handler=menu');
            const b = a.data.data;
            let allMenu = [];
            // d 所有为父节点的集合。用于求默认和条件为父节点时的差集，解决element tree无半选问题。
            let d = [];
            for (var i in b) {
                // 一级
                if (b[i].parentId === null) {
                    allMenu.push(b[i]);
                    d.push(b[i].id);
                } else {
                    allMenu.filter((ele, index) => {
                        if (ele.id === b[i].parentId) {
                            if (allMenu[index].children === undefined) { allMenu[index].children = []; }
                            allMenu[index].children.push(b[i]);
                        }
                    });
                }
            }
            this.defaultExpandedKeys = d;
            // 格式后的数据
            this.allMenu = allMenu;
            const e = [];
            d.map((res) => {
                this.defaultCheckedKeys.map((ele) => {
                    if (res !== ele) {
                        if (d.indexOf(ele) < 0) {
                            e.push(ele);
                        }
                    }
                });
            });
            this.defaultCheckedKeys = e;
        },
        // 更新授权
        async  auUpdateData() {
            this.au.updateLoading = true;
            const checkNodes = this.$refs.tree.getCheckedNodes(false, true);
            let array = [];
            checkNodes.map((ele) => {
                array.push({
                    PermissionId: ele.id,
                    roleId: this.au.temp.id,
                    isMenu: ele.isMenu,
                    roleCode: ele.resourceCode
                });
            });
            const respnseData = await service.post('/Admin/Role/Permission', array);
            if (respnseData.data.success) {
                this.getList();
                this.$notify({
                    title: "Success",
                    message: "授权成功",
                    type: "success",
                    duration: 2000
                });
                this.au.visible = false;
                this.au.updateLoading = false;
            }
        },
        // 初始化获取数据
        getList() {
            let { pageIndex, pageSize, roleName, orderField } = this.listQuery;
            service.get(`/Admin/Role/index?handler=List&pageIndex=${pageIndex}&pageSize=${pageSize}&roleName=${roleName}&orderField=${orderField}`).then(res => {
                this.tableData = res.data.data.list;
                this.paginationQuery.total = res.data.data.totalCount;
            });
        },
        // 编辑
        handleEdit(index, row) {
            this.dialog.visible = true;
            if (row) {
                // 编辑
                let { roleName, roleCode, adminRemark, remark, addTime, id, enabled } = row;
                this.dialog.data = {
                    roleName, roleCode, adminRemark, remark, addTime, id, enabled
                };
                this.dialog.title = '编辑角色';
            } else {
                // 新增
                this.dialog.title = '新增角色';
            }
        },
        // 更新新增、编辑
        updateData() {
            this.dialog.updateLoading = true;
            this.$refs['dataForm'].validate(valid => {
                // 表单校验
                if (valid) {
                    console.log(this.dialog.data);
                    let data = {
                        Id: this.dialog.data.id,
                        RoleName: this.dialog.data.roleName,
                        RoleCode: this.dialog.data.roleCode,
                        AdminRemark: this.dialog.data.adminRemark,
                        Remark: this.dialog.data.remark,
                        Enabled: this.dialog.data.enabled
                    };
                    service.post("/Admin/Role/Edit?handler=Save", data).then(res => {
                        if (res.data.success) {
                            this.getList();
                            this.$notify({
                                title: "Success",
                                message: "成功",
                                type: "success",
                                duration: 2000
                            });
                            this.dialog.visible = false;
                            this.dialog.updateLoading = false;
                        }
                    });
                }
            });


        },
        // 删除
        handleDelete(index, row) {
            let ids = [row.id];
            service.post("/Admin/Role/Index?handler=Delete", ids).then(res => {
                if (res.data.success) {
                    this.getList();
                    this.$notify({
                        title: "Success",
                        message: "删除成功",
                        type: "success",
                        duration: 2000
                    });
                }
            });
        }
    }

});
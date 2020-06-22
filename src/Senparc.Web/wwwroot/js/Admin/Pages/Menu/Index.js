var vm = new Vue({
    el: "#app",
    data() {
        return {
            // 表格数据
            tableData: [],
            dialog: {
                title: '新增菜单',
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
            }
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
        }
    },
    methods: {
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
            // 获取所有菜单
        async  getList() {
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
            this.tableData = allMenu;
            console.log(allMenu);
        },
        // 增加下一级
        handleAdd(index, row) { console.log('增加下一级');},
        // 编辑
        handleEdit(index, row) {
            this.dialog.visible = true;
            if (row) {
                // 编辑
                let { roleName, roleCode, adminRemark, remark, addTime, id, enabled } = row;
                this.dialog.data = {
                    roleName, roleCode, adminRemark, remark, addTime, id, enabled
                };
                this.dialog.title = '编辑菜单';
            } else {
                // 新增
                this.dialog.title = '新增菜单';
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
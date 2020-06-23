var vm = new Vue({
    el: "#app",
    data() {
        var validateCode = (rule, value, callback) => {
            if (this.dialog.data.menuType === 3) {
                if (!value) {
                    callback(new Error('当类型是按钮类型时此项必填'));
                } else {
                    callback();
                }
            } else {
                callback();
            }
        };
        return {
            // 表格数据
            tableData: [],
            dialog: {
                title: '新增菜单',
                visible: false,
                data: {
                    id: '', menuName: '', parentId: [], url: '', icon: '', sort: '', visible: true,
                    resourceCode: '', isLocked: false, menuType: ''
                },
                rules: {
                    menuName: [
                        { required: true, message: "菜单名称为必填项", trigger: "blur" }
                    ],
                    menuType: [{ required: true, message: "类型为必选项", trigger: "blur" }],
                    resourceCode: [{ validator: validateCode, trigger: "blur" }]
                },
                updateLoading: false,
                disabled: false
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
                    id: '', menuName: '', parentId: [], url: '', icon: '', sort: '', visible: false,
                    resourceCode: '', isLocked: false, menuType: ''
                };
                this.dialog.updateLoading = false;
                this.dialog.disabled = false;
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
            const a = await service.get('/Admin/Menu/Edit?handler=Menu');
            const b = a.data.data;
            let allMenu = [];
            this.ddd(b, null, allMenu);
            this.tableData = allMenu;
        },
        ddd(source, parentId, dest) {
            var array = source.filter(_ => _.parentId === parentId);
            for (var i in array) {
                var ele = array[i];
                ele.children = [];
                dest.push(ele);
                this.ddd(source, ele.id, ele.children);
            }
        },
        // 编辑 // 新增菜单 // 增加下一级
        handleEdit(index, row, flag) {
            this.dialog.visible = true;
            if (flag === 'add') {
                // 新增
                this.dialog.title = '新增菜单';
                return;
            }
            // 编辑
            let { id, menuName, parentId, url, icon, sort, visible,
                resourceCode, isLocked, menuType } = row;
            this.dialog.data = {
                id, menuName, parentId: [parentId], url, icon, sort, visible,
                resourceCode, isLocked, menuType
            };
            if (flag === 'edit') {
                this.dialog.title = '编辑菜单';
                if (row.isLocked) {
                    this.dialog.disabled = true;
                }
            } else if (flag === 'addNext') {
                this.dialog.data.id = '';
                this.dialog.title = '增加下一级菜单';
                this.dialog.menuName = '';
                // 增加下一级
                this.tableData.forEach((res, index) => {
                    if (res.id === row.id) {
                        this.dialog.data.parentId = [row.id];
                        throw new Error("ending");// 跳出循环
                    } else {
                        if (!res.children) { return false; }
                        res.children.forEach(ele => {
                            if (ele.id === row.id) {
                                this.dialog.data.parentId = [res.id, row.id];
                                throw new Error("ending");// 跳出循环
                            } else {
                                if (!ele.children) { return false; }
                                ele.children.forEach(el => {
                                    if (el.id === row.id) {
                                        this.dialog.data.parentId = [res.id, row.id, el.id];
                                        throw new Error("ending");// 跳出循环
                                    }
                                });
                            }
                        });
                    }
                });
            }
        },
        // 更新新增、编辑
        updateData() {
            this.$refs['dataForm'].validate(valid => {
                // 表单校验
                if (valid) {
                    this.dialog.updateLoading = true;
                    let data = {
                        Id: this.dialog.data.id,
                        MenuName: this.dialog.data.menuName,
                        ParentId: this.dialog.data.parentId[this.dialog.data.parentId.length - 1],
                        Url: this.dialog.data.url,
                        Icon: this.dialog.data.icon,
                        Sort: this.dialog.data.sort * 1,
                        Visible: this.dialog.data.visible,
                        ResourceCode: this.dialog.data.resourceCode,
                        IsLocked: this.dialog.data.isLocked,
                        MenuType: this.dialog.data.menuType
                    };
                    service.post("/Admin/Menu/Edit?handler=save", data).then(res => {
                        if (res.data.success) {
                            this.getList();
                            this.$notify({
                                title: "Success",
                                message: "成功",
                                type: "success",
                                duration: 2000
                            });
                            this.dialog.visible = false;
                        }
                    });
                }
            });


        },
        // 删除
        handleDelete(index, row) {
            let ids = [row.id];
            service.post("/Admin/Menu/edit?handler=Delete", ids).then(res => {
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
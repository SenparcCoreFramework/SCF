var vm = new Vue({
    el: "#app",
    data() {
        const validatePass = (rule, value, callback) => {
            if (value === '') {
                callback(new Error('请输入密码'));
            } else {
                if (this.dialog.data.password2 !== '') {
                    this.$refs.dataForm.validateField('password2');
                }
                callback();
            }
        };
        const validatePass2 = (rule, value, callback) => {
            if (value === '') {
                callback(new Error('请再次输入密码'));
            } else if (value !== this.dialog.data.password) {
                callback(new Error('两次输入密码不一致!'));
            } else {
                callback();
            }
        };
        return {
            //分页参数
            paginationQuery: {
                total: 5
            },
            //分页接口传参
            listQuery: {
                pageIndex: 1,
                pageSize: 20,
                adminUserInfoName: ''
            },
            tableData: [],
            dialog: {
                title: '新增管理员',
                visible: false,
                data: {
                    id: 0,
                    userName: '',
                    password: '',
                    password2: '',
                    realName: '',
                    phone: '',
                    note: ''
                },
                rules: {
                    userName: [
                        { required: true, message: "用户名为必填项", trigger: "blur" }
                    ],
                    password: [{ required: true, validator: validatePass, trigger: "blur" }],
                    password2: [{ required: true, validator: validatePass2, trigger: "blur" }]
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
                    id:0,
                    userName: '',
                    password: '',
                    password2: '',
                    realName: '',
                    phone: '',
                    note: ''
                };
                this.dialog.updateLoading = false;
            }
        }
    },
    methods: {
        // 获取数据
        getList() {
            let { adminUserInfoName, pageIndex, pageSize } = this.listQuery;
            service.get(`/Admin/AdminUserInfo/index?handler=List&adminUserInfoName=${adminUserInfoName}&pageIndex=${pageIndex}&pageSize=${pageSize}`).then(res => {
                this.tableData = res.data.data.list;
                this.paginationQuery.total = res.data.data.totalCount;
            });
        },
        // 编辑
        handleEdit(index, row) {
            this.dialog.visible = true;
            if (row) {
                // 编辑
                let { userName, password, realName, phone, note, id } = row;
                this.dialog.data = {
                    userName, password, realName, phone, note, id
                };
                this.dialog.data.password2 = password;
                this.dialog.title = '编辑管理员';
            } else {
                // 新增
                this.dialog.title = '新增管理员';
            }
        },
        // 更新新增编辑
        updateData() {
            this.dialog.updateLoading = true;
            this.$refs['dataForm'].validate(valid => {
                // 表单校验
                if (valid) {
                    console.log(this.dialog.data);
                    let data = {
                        Id: this.dialog.data.id,
                        UserName: this.dialog.data.userName,
                        Password: this.dialog.data.password,
                        Note: this.dialog.data.note,
                        RealName: this.dialog.data.realName,
                        Phone: this.dialog.data.phone
                    };
                    service.post("/Admin/AdminUserInfo/Edit?handler=Save", data).then(res => {
                        if (res.data.success) {
                            this.getList();
                            this.$notify({
                                title: "Success",
                                message: "编辑成功",
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
        // 设置角色
        handleSet(index, row) {
            console.log(index);
        },
        // 删除
        handleDelete(index, row) {
            let ids = [row.id];
            service.post("/Admin/AdminUserInfo/Index?handler=Delete", ids).then(res => {
                if (res.data.success) {
                    this.getList();
                    this.$notify({
                        title: "Success",
                        message: "编辑成功",
                        type: "success",
                        duration: 2000
                    });
                }
            });
        }
    }

});
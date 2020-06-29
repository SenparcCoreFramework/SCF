var app = new Vue({
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
            // 菜单栏数据 navMenu.js
            navMenu: navMenu,
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
                disabled: false,
                checkStrictly: true // 是否严格的遵守父子节点不互相关联	
            },
            dialogIcon:{
                visible: false,
                 elementIcons : ['platform-eleme', 'eleme', 'delete-solid', 'delete', 's-tools', 'setting', 'user-solid', 'user', 'phone', 'phone-outline', 'more', 'more-outline', 'star-on', 'star-off', 's-goods', 'goods', 'warning', 'warning-outline', 'question', 'info', 'remove', 'circle-plus', 'success', 'error', 'zoom-in', 'zoom-out', 'remove-outline', 'circle-plus-outline', 'circle-check', 'circle-close', 's-help', 'help', 'minus', 'plus', 'check', 'close', 'picture', 'picture-outline', 'picture-outline-round', 'upload', 'upload2', 'download', 'camera-solid', 'camera', 'video-camera-solid', 'video-camera', 'message-solid', 'bell', 's-cooperation', 's-order', 's-platform', 's-fold', 's-unfold', 's-operation', 's-promotion', 's-home', 's-release', 's-ticket', 's-management', 's-open', 's-shop', 's-marketing', 's-flag', 's-comment', 's-finance', 's-claim', 's-custom', 's-opportunity', 's-data', 's-check', 's-grid', 'menu', 'share', 'd-caret', 'caret-left', 'caret-right', 'caret-bottom', 'caret-top', 'bottom-left', 'bottom-right', 'back', 'right', 'bottom', 'top', 'top-left', 'top-right', 'arrow-left', 'arrow-right', 'arrow-down', 'arrow-up', 'd-arrow-left', 'd-arrow-right', 'video-pause', 'video-play', 'refresh', 'refresh-right', 'refresh-left', 'finished', 'sort', 'sort-up', 'sort-down', 'rank', 'loading', 'view', 'c-scale-to-original', 'date', 'edit', 'edit-outline', 'folder', 'folder-opened', 'folder-add', 'folder-remove', 'folder-delete', 'folder-checked', 'tickets', 'document-remove', 'document-delete', 'document-copy', 'document-checked', 'document', 'document-add', 'printer', 'paperclip', 'takeaway-box', 'search', 'monitor', 'attract', 'mobile', 'scissors', 'umbrella', 'headset', 'brush', 'mouse', 'coordinate', 'magic-stick', 'reading', 'data-line', 'data-board', 'pie-chart', 'data-analysis', 'collection-tag', 'film', 'suitcase', 'suitcase-1', 'receiving', 'collection', 'files', 'notebook-1', 'notebook-2', 'toilet-paper', 'office-building', 'school', 'table-lamp', 'house', 'no-smoking', 'smoking', 'shopping-cart-full', 'shopping-cart-1', 'shopping-cart-2', 'shopping-bag-1', 'shopping-bag-2', 'sold-out', 'sell', 'present', 'box', 'bank-card', 'money', 'coin', 'wallet', 'discount', 'price-tag', 'news', 'guide', 'male', 'female', 'thumb', 'cpu', 'link', 'connection', 'open', 'turn-off', 'set-up', 'chat-round', 'chat-line-round', 'chat-square', 'chat-dot-round', 'chat-dot-square', 'chat-line-square', 'message', 'postcard', 'position', 'turn-off-microphone', 'microphone', 'close-notification', 'bangzhu', 'time', 'odometer', 'crop', 'aim', 'switch-button', 'full-screen', 'copy-document', 'mic', 'stopwatch', 'medal-1', 'medal', 'trophy', 'trophy-1', 'first-aid-kit', 'discover', 'place', 'location', 'location-outline', 'location-information', 'add-location', 'delete-location', 'map-location', 'alarm-clock', 'timer', 'watch-1', 'watch', 'lock', 'unlock', 'key', 'service', 'mobile-phone', 'bicycle', 'truck', 'ship', 'basketball', 'football', 'soccer', 'baseball', 'wind-power', 'light-rain', 'lightning', 'heavy-rain', 'sunrise', 'sunrise-1', 'sunset', 'sunny', 'cloudy', 'partly-cloudy', 'cloudy-and-sunny', 'moon', 'moon-night', 'dish', 'dish-1', 'food', 'chicken', 'fork-spoon', 'knife-fork', 'burger', 'tableware', 'sugar', 'dessert', 'ice-cream', 'hot-water', 'water-cup', 'coffee-cup', 'cold-drink', 'goblet', 'goblet-full', 'goblet-square', 'goblet-square-full', 'refrigerator', 'grape', 'watermelon', 'cherry', 'apple', 'pear', 'orange', 'coffee', 'ice-tea', 'ice-drink', 'milk-tea', 'potato-strips', 'lollipop', 'ice-cream-square', 'ice-cream-round']
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
        // 选取图标
        pickIcon(item) {
            this.dialogIcon.visible = false;
            this.dialog.data.icon = item;
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
                this.dialog.data.menuName = '';
                // 设置父级菜单默认显示
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
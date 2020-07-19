﻿var app = new Vue({
    el: "#app",
    data() {
        return {
            newTableData: [], // 新模块数据
            oldTableData: [], // 已安装模块
            isExtend: false, //是否切换状态
            handlerText: "",
            handlerTips: "",
            newData: {},
            oldData: {
                state: {
                    0: '关闭',
                    1: '开放',
                    2: '新增待审核',
                    3: '更新待审核'
                }
            }

        };
    },
    watch: {
        'isExtend': {
            handler: function (val, oldVal) {
                this.handlerText = val ? '开启【扩展模块】管理模式' : '切换至发布状态，隐藏【扩展莫管】管理单元';
                this.handlerTips = val ? '打开【扩展模块】管理功能后，所有扩展模块将显示在【扩展模块】二级目录中。确定要打开吗？' : '隐藏【扩展模块】管理功能后，所有扩展模块将并列显示在一级目录中。如需重新打开，请直接浏览器内访问此页面【/Admin/XscfModule】。确定要隐藏吗？';
            },
            immediate: true
        }
    },
    created: function () {
        this.getList();
    },
    methods: {
        // 获取
        async  getList() {
            const oldTableData = await service.get('/Admin/XscfModule/Index?handler=Mofules');
            this.oldTableData = oldTableData.data.data.result;
            // 是否切换状态
            this.isExtend = oldTableData.data.data.hideModuleManager;
            const newTableData = await service.get('/Admin/XscfModule/Index?handler=UnMofules');
            this.newTableData = newTableData.data.data;
        },
        // 切换状态
        async handleSwitch() {
            await service.post('/Admin/XscfModule/Index?handler=HideManager');
            this.isExtend = !this.isExtend;
            window.location.href ="/Admin/Index";
        },
        // 安装
        async handleInstall(index, row) {
            await service.get(`/Admin/XscfModule/Index?handler=ScanAjax&uid=${row.uid}`);
            this.getList();
            //this.$notify({
            //    title: "Success",
            //    message: "安装成功，请在 系统管理-角色管理 页面授权之后再使用！",
            //    type: "success",
            //    duration: 5000
            //});
            window.location.reload();
        },
        // 操作
        handleHandle(index, row) {
            window.location.href = "/Admin/XscfModule/Start/?uid=" + row.xscfRegister.uid;
        },
        // 主页
        handleIndex(index, row) {
            window.location.href = row.xscfRegister.homeUrl;
        }
    }
});
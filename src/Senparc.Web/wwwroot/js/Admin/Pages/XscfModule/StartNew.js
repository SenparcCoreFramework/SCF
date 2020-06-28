var vm = new Vue({
    el: "#app",
    data() {
        return {
            oldData: {
                state: {
                    'String': '文本',
                    'Int32': '数字',
                    'Int64': '数字',
                    'DateTime': '日期',
                    'String[]': '选项'
                }
            },
            tooltip: {
                0: '网页',
                1: '执行方法',
                2: '数据库',
                3: '中间件',
                4: '线程'
            },
            seeMore: false, //查看线程 
            modifyLog: [{
                log: '上海市普陀区金沙江路 1518 弄'
            }, {
                log: '上海市普陀区金沙江路 1517 弄'
            }, {
                log: '上海市普陀区金沙江路 1519 弄'
            }, {
                log: '上海市普陀区金沙江路 1516 弄'
            }] ,// 更新记录
        };
    },
    created: function () {
        this.getList();
    },
    methods: {
        getList() {
        },
        // 安装
        async handleInstall(index, row) {
            await service.get(`/Admin/XscfModule/Index?handler=ScanAjax&uid=${row.uid}`);
        },
        // 删除
        handleDelete() {
            app.pageSrc = '/Admin/XscfModule/Index';
        },
        // 更新版本
        updataVersion() { }
    }
});
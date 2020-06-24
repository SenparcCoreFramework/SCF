var vm = new Vue({
    el: "#app",
    data() {
        return {
            newTableData: [{}, {}, {}], // 新模块数据
            tableData: [{}, {}, {}], // 已安装模块
            isExtend: false,
            handlerText: "",
            handlerTips: ""
        };
    },
    watch: {
        'isExtend': {
            handler: function (val, oldVal)  {
                this.handlerText = val ? '开启【扩展模块】管理模式' : '切换至发布状态，隐藏【扩展莫管】管理单元';
                this.handlerTips = val ? '打开【扩展模块】管理功能后，所有扩展模块将显示在【扩展模块】二级目录中。确定要打开吗？' : '隐藏【扩展模块】管理功能后，所有扩展模块将并列显示在一级目录中。如需重新打开，请直接浏览器内访问此页面【/Admin/XscfModule】。确定要隐藏吗？';
            },
            immediate:true
        }
    },
    created: function () {
        this.getList();
    },
    methods: {
        // 获取
        async  getList() {
            
        },
        // 切换状态
        handleSwitch() {
            this.isExtend = !this.isExtend;
        },
        // 安装
        handleInstall(index, row, flag) {
            console.log('安装');
        },
        // 操作
        handleHandle(index, row, flag) {
            console.log('操作');
        },
        // 主页
        handleIndex(index, row, flag) {
            console.log('主页');
        }
    }

});
var app = new Vue({
    el: "#app",
    data() {
        return {
            navMenu: navMenu, // 菜单栏数据 navMenu.js
            data: [], // 数据
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
                "IAreaRegister": '网页',
                "IXscfDatabase": '数据库',
                "IXscfMiddleware": '中间件',
                "IXscfRazorRuntimeCompilation": '线程'
            },
            xScfModules_State: {
                0: '关闭',
                1: '开放',
                2: '新增待审核',
                3: '更新待审核'
            },
            seeMore: false, //查看线程 
            // 执行弹窗
            run: {
                data: {},
                visible: false,
                dialogData: {
                    description: "",
                    isRequired: false,
                    name: "",
                    parameterType: 0,
                    selectionList:[],
                    checkboxList:[],
                    systemType: "",
                    title: "",
                    value: null
                }
            },
            checkList:[]
        };
    },
    created() {
        this.getList();
    },
    methods: {
        async  getList() {
            const uid = resizeUrl().uid;
            const res = await service.get(`/Admin/XscfModule/Start?handler=Detail&uid=${uid}`);
            this.data = res.data.data;
            this.data.xscfRegister.interfaces = this.data.xscfRegister.interfaces.splice(1);
            console.info(this.data);
        },
        // 打开执行
        openRun(item) {
            this.run.data = item;
            this.run.visible = true;
            this.run.dialogData = {
                description: "",
                isRequired: false,
                name: "",
                parameterType: 0,
                selectionList: [],
                checkList: [],
                systemType: "",
                title: "",
                value: null
            };
            console.log(item);
        },
        // 执行
        handleRun(item) {
        },
        // 删除
        async updataState() {
            const uid = resizeUrl().uid;
            await service.get(`/Admin/XscfModule/Start?handler=ChangeState&uid=${uid}`);
            window.location.reload();
        },
        // 更新版本
        async  updataVersion() {
            const uid = resizeUrl().uid;
            await service.get(`/Admin/XscfModule/Index?handler=ScanAjax&uid=${uid}`);
            window.location.reload();
        }
    }
});
var app = new Vue({
    el: "#app",
    data() {
        return {
            data: [], // 数据
            tooltip: {
                "IAreaRegister": '网页',
                "IXscfDatabase": '数据库',
                "IXscfMiddleware": '中间件',
                "IXscfRazorRuntimeCompilation": '线程'
            },
            state: {
                'String': '文本',
                'Int32': '数字',
                'Int64': '数字',
                'DateTime': '日期',
                'String[]': '选项'
            },
            xScfModules_State: {
                0: '关闭',
                1: '开放',
                2: '新增待审核',
                3: '更新待审核'
            },
            // 执行弹窗
            run: {
                data: {},
                visible: false
            },
            runData: {
                // 绑定数据
            },
            runResult: {
                visible: false,
                tit: '',
                tip: '',
                msg: '',
                tempId: '',
                hasLog: false
            },
            //查看线程 
            thread: {
                visible: false
            }
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
            window.document.title = this.data.xscfModule.menuName;
        },


        // 打开执行
        openRun(item) {
            this.run.data = item;
            this.runData = {};
                this.run.data.value.map(res => {
                    // 动态model绑定生成
                    // 默认选择
                    if (res.parameterType === 2 && res.selectionList.items) {
                        this.runData[res.name] = [];
                        res.selectionList.items.map(ele => {
                            if (ele.defaultSelected) {
                                this.runData[res.name].push(ele.value);
                            }
                        });
                    }
                    if (res.parameterType === 1 && res.selectionList.items) {
                        this.runData[res.name] = [];
                        res.selectionList.items.map(ele => {
                            if (ele.defaultSelected) {
                                this.runData[res.name].push(ele.value);
                            }
                        });
                    }
                    if (res.parameterType === 0) {
                        this.runData[res.name] = "";
                    }
                });
            this.runData = Object.assign({}, this.runData);
            this.run.visible = true;
        },
        // 执行
        async handleRun() {
            // 物理路径校验
            if (this.runData.hasOwnProperty('SourcePath') && this.runData.SourcePath.length < 1) {
                this.$notify({
                    title: '警告',
                    message: '请填写源码物理路径',
                    type: 'warning'
                });
                return;
            }
            // 关闭执行弹窗
            this.run.visible = false;
            let xscfFunctionParams = {};
            for (var i in this.runData) {
                if (Array.isArray(this.runData[i])) {
                    xscfFunctionParams[i] = {};
                    xscfFunctionParams[i].SelectedValues = [];
                    xscfFunctionParams[i].SelectedValues = this.runData[i];
                } else {
                    xscfFunctionParams[i] = this.runData[i];
                }
            }
            const data = {
                xscfUid: this.data.xscfModule.uid, xscfFunctionName: this.run.data.key.name, xscfFunctionParams: JSON.stringify(xscfFunctionParams)
            };
            const res = await service.post(`/Admin/XscfModule/Start?handler=RunFunction`, data);
            this.runResult.tempId = res.data.tempId;
            if ((res.data.log || '').length > 0 && (res.data.tempId || '').length > 0) {
                this.runResult.hasLog = true;
            }
            if (!res.data.success) {
                this.runResult.tit = '遇到错误';
                this.runResult.tip = '错误信息';
                this.runResult.msg = res.data.msg;
                return;
            }
            if (res.data.msg && (res.data.msg.indexOf('http://') !== -1 || res.data.msg.indexOf('https://') !== -1)) {
                this.runResult.tit = '执行成功';
                this.runResult.tip = '收到网址，点击下方打开<br />（此链接由第三方提供，请注意安全）：';
                this.runResult.msg = '<i class="fa fa-external-link"></i> <a href="' + res.data.msg + '" target="_blank">' + res.data.msg + '</a>';
            }
            else {
                this.runResult.tit = '执行成功';
                this.runResult.tip = '返回信息';
                this.runResult.msg = res.data.msg;
            }
            // 打开执行结果弹窗
            this.runResult.visible = true;
        },
        // 关闭和开启
        async updataState(state) {
            const id = this.data.xscfModule.id;
            const res = await service.get(`/Admin/XscfModule/Start?handler=ChangeState&id=${id}&tostate=${state}`);
            window.location.reload();
        },
        // 更新版本
        async  updataVersion() {
            const uid = resizeUrl().uid;
            await service.get(`/Admin/XscfModule/Index?handler=ScanAjax&uid=${uid}`);
            window.location.reload();
        },
        // 删除
        async handleDelete() {
            const id = this.data.xscfModule.id;
            const res = await service.post(`/Admin/XscfModule/Start?handler=Delete&id=${id}`);
            window.sessionStorage.setItem('activeMenu', 5);
            window.location.href ='/Admin/XscfModule/Index';
        }
    }
});
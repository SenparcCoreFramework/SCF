var app = new Vue({
    el: '#app',
    data: {
        //侧边栏
        navMenuList: [],
        isCollapse: false,
        variables: {
            menuBg: '#304156', // 背景色
            menuText: '#bfcbd9', // 文字色
            menuActiveText: '#409EFF' //激活颜色
        },
        activeMenu: '0',
        //iframe
        pageSrc: '',
        //分页
        total: 100,
        listQuery: {
            page: 1,
            limit: 20
        }
    },
    watch: {
        storePageSrc(val) {
            if (val) {
                this.pageSrc = val;
            }
        }
    },
    computed: {
        storePageSrc() {
            return Store.state.pageSrc;
        },
        store() {
            return Store.state;
        }
    },
    mounted() {
        this.getNavMenu();
    },
    methods: {
        setStore(key,value) {
            Store.commit(key, value);
            console.log(Store.state.pageSrc);
        },
        toggleSideBar() {
            this.isCollapse = !this.isCollapse;
        },
        getNavMenu() {
            service.get("/Admin/index?handler=MenuTree").then(res => {
                if (res.data.success) {
                    var ddd = res.data.data;
                    myfunctionMain(ddd);
                    //console.info(ddd);
                    this.navMenuList = ddd;
                }
            });
        }
    }
});

function myfunctionMain(list) {
    if (!list && list.length === 0) {
        return;
    }
    for (var i in list) {
        list[i].index = list[i].id;
        myfunctionMain(list[i].children);
    }
}

//$(_init);
//function _init() {
//    initChart();
//}

//function initChart() {
//    let chart1 = document.getElementById('firstChart');
//    let chartOption1 = {
//        title: {
//            text: '数量统计',
//            subtext: '2019年11月1日 - 2019年11月5日'
//        },
//        xAxis: {
//            type: 'category',
//            data: ['商品', '数据', '订单', '消息', '异常', '审批', '退单', '新订单']
//        },
//        yAxis: {
//            type: 'value',
//            axisLabel: {
//                formatter: '{value} 件'
//            }
//        }, tooltip: {

//        },
//        series: [{
//            data: [5, 20, 36, 10, 15, 40, 33, 17],
//            type: 'bar',
//            color: '#91c7ae'
//        }]
//    };

//    let chartInstance1 = echarts.init(chart1);
//    chartInstance1.setOption(chartOption1);


//    let chart2 = document.getElementById('secondChart');
//    let chartOption2 = {
//        title: {
//            text: '数量统计',
//            subtext: '2019年度'
//        }, legend: {
//            data: ['自由商品销售额']
//        },
//        xAxis: {
//            type: 'category',
//            data: ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月']
//        },
//        yAxis: {
//            type: 'value',
//            axisLabel: {
//                formatter: '{value} 元'
//            }
//        }, tooltip: {
//            formatter: function (data, ticket, cllback) {
//                //debugger
//                return data.seriesName + ':' + formatCurrency(data.value) + '元';
//            }
//        },
//        series: [{
//            data: [2666, 2778, 4926, 5767, 6810, 5670, 4123, 5687, 3654, 4999, 5301, 7358],
//            name: '自由商品销售额',
//            type: 'line'
//        }]
//    };

//    let chartInstance2 = echarts.init(chart2);
//    chartInstance2.setOption(chartOption2);
//}
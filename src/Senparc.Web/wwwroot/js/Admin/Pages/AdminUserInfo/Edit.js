var vm = new Vue({
    el: "#app",
    data: {
        form: {
            UserName: '',
            Password: '',
            ConfirmPassword: '',
            RealName: '',
            Phone: '',
            Note: '',
            dialogVisible: true
        }
    },
    methods: {
        onSubmit() {
            vm.$confirm("确定提交吗？", '提示', {
                confirmButtonText: '确定',
                cancelButtonText: '取消',
                type: 'warning'
            }).then((ss) => {

            }).catch((ww) => {

            });
            //base.confirm("确定提交吗？", "提交后保存数据", function () {
            //    console.log("comfirm!");
            //}, function () {
            //    console.log("cancel!");
            //});
            //console.log('submit!');

        }
    }

});
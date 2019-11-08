var vm = new Vue({
    el: "#app",
    data: function () {
        return {
            list: [],//AdminUserInfo_getList
            currentPage1: 5,
            currentPage2: 5,
            currentPage3: 5,
            currentPage4: 4,
            multipleSelection:[]
        };
    },
    created: function () {
        //this.adminUserInfo_getList();
    },
    methods: {
        adminUserInfo_getList: function () {
            base.get("/Admin/AdminUserInfo/OnGetAsync", function (result) {
                //vm.list
            });
        },
        handleEdit: function (data) {
            window.open("/Admin/AdminUserInfo/Edit?id=" + data.Id);
        },
        handleSizeChange(val) {
            console.log(`每页 `+val+` 条`);
        },
        handleCurrentChange(val) {
            console.log(`当前页: `+val);
        },
        handleSelectionChange(val) {
            vm.multipleSelection = val;
        }
    }

});
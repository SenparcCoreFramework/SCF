// 菜单栏数据
const navMenu = {
    //侧边栏
    navMenuList: [],
    isCollapse: false,
    variables: {
        menuBg: '#304156', // 背景色
        menuText: '#bfcbd9', // 文字色
        menuActiveText: '#409EFF' //激活颜色
    },
    // 当前激活菜单的 index
    activeMenu: window.sessionStorage.getItem('activeMenu')||'0'
};
// 菜单栏数据
(function getNavMenu() {
    service.get("/Admin/index?handler=MenuResource").then(res => {
        if (res.data.success) {
            var ddd = res.data.data.menuList;
            myfunctionMain(ddd);
            navMenu.navMenuList = ddd;
            // 按钮权限存起来  使用：直接在dom上v-has=" ['admin-add']"
            window.sessionStorage.setItem('saveResourceCodes', JSON.stringify(res.data.data.resourceCodes));
        }
    });
})();
function menuSelect(key, keyPath) {
    window.sessionStorage.setItem('activeMenu', key);
}
// 切换菜单栏展开
function toggleSideBar() {
    navMenu.isCollapse = !navMenu.isCollapse;
}

// 菜单栏数据递归
function myfunctionMain(list) {
    if (!list && list.length === 0) {
        return;
    }
    for (var i in list) {
        list[i].index = list[i].id;
        myfunctionMain(list[i].children);
    }
}

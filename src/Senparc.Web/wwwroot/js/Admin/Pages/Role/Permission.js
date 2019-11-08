$(function () {
    _init();
});
let query = resizeUrl();
let treeObj;

async function _init() {
    let permissions = await base.get('/Admin/Role/Permission', { handler: 'RolePermission', roleId: query.RoleId  });
    treeObj = await initMenu('#tree', {
        IsExpendAll: true,
        check: {
            enable: true
        },
        callback: {
            onClick: zTreeOnClick
        }
    });
    //treeObj = getTreeInstance('tree');//.expandAll(true);

    for (var i = 0, l = permissions.length; i < l; i++) {
        //
        if (permissions[i].isMenu) {
            //continue;
        }
        let node = treeObj.getNodeByParam("id", permissions[i].permissionId, null);
        if (node) {
            treeObj.checkNode(node, true, false);
        }
    }
    $('#save').on('click', save);

}

async function save() {
    
    let checkNodes = treeObj.getCheckedNodes(true);
    let array = [];
    checkNodes.map((ele) => {
        array.push({
            PermissionId: ele.id,
            roleId : query.RoleId,
            isMenu: ele.isMenu,
            roleCode: ele.roleCode
        });
    });
    let respnseData = await base.post('/Admin/Role/Permission', { sysMenuDto: array });
    await base.swal.alert('保存成功！', 'success');
    window.location.href = '/Admin/Role/Index';
    //debugger;
    //if (respnseData) {
    //    //
        
    //}
}

/**
 * ddd
 * @param {any} event d
 * @param {any} treeId d
 * @param {any} treeNode 22
 */
function zTreeOnClick(event, treeId, treeNode) {

}
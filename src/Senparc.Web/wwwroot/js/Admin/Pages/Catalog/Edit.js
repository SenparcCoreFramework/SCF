$(function () {
    _init();
});

let treeObj;

async function _init() {
    let catalogs = await base.get('/Admin/Document/Edit', { handler: 'Catalog'});
    treeObj = await initCatalog('#tree', {
        IsExpendAll: true,
        check: {
            enable: true
        },
        callback: {
            onClick: zTreeOnClick
        }
    });
    //treeObj = getTreeInstance('tree');//.expandAll(true);

    for (var i = 0, l = catalogs.length; i < l; i++) {
        //
        if (catalogs[i].isMenu) {
            //continue;
        }
        let node = treeObj.getNodeByParam("id", catalogs[i].Id, null);
        if (node) {
            treeObj.checkNode(node, true, false);
        }
    }
    //$('#save').on('click', save);

    bindDeleteButtonEvent();
    //$('.addButton').off('click').on('click', addButton);

    //$('#submit').on('click', submit);

    $('.addTopMenu').on('click', addTopMenu);
    

    //treeObj = await initCatalog(null, {
    //    hiddenButton: true,
    //    IsExpendAll: true,
    //    callback: {
    //        onClick: zTreeOnClick,
    //        beforeRemove: zTreeBeforeRemove
    //    },
    //    edit: {
    //        showRemoveBtn: true,
    //        showRenameBtn: false,
    //        enable: true
    //    }
    //});
}

/**
 * sdfsdf
 * @param {any} treeId e
 * @param {any} treeNode eeee
 * @returns {any} asdfasd
 */
function zTreeBeforeRemove(treeId, treeNode) {
    deleteNode(treeNode);
    return false;
}

async function deleteNode(treeNode) {
    let bol = await base.swal.confirm('是否要删除当前节点，该节点下面的所有节点将会被删除！', 'warning');
    if (!bol) {
        return false;
    }
    var result = getAllChildrenNodes(treeNode, []);
    result.push(treeNode);
    var ids = result.map(_ => { return _.id; });
    let response = await base.post('/Admin/Document/Edit?handler=delete', { ids: ids });
    if (response.length === 0) {
        for (var i = 0; i < result.length; i++) {
            treeObj.removeNode(result[i]);
        }
    } else {
        await base.swal.confirm('删除失败！当前节点不允许删除！', 'warning');
    }
}

async function zTreeOnClick(event, treeId, treeNode) {
    //console.info(treeNode);
    var id = treeNode.id;
    var data = await base.get('/Admin/Document/Edit', { id: id, handler: 'detail' });
    
    for (var i in data.catalogDto) {
        $('#CatalogDto_' + i.toString().replace(/^\S/, s => s.toUpperCase())).val(data.catalogDto[i]);
    }   

    //if (data.catalogDto.isLocked) {
    //    $('#form input').attr('disabled', 'disabled').addClass('disabled');
    //    $('#submit').attr('disabled', 'disabled').addClass('disabled');
    //    $('.addButton').attr('disabled', 'disabled').addClass('disabled');
    //} else {
    //    $('#form input').removeAttr('disabled').removeClass('disabled');
    //    $('#submit').removeAttr('disabled').removeClass('disabled');
    //    $('.addButton').removeAttr('disabled').removeClass('disabled');
    //}
    //debugger
    //if (data.catalogDto.isLocked) {
    //    $('#CatalogDto_IsLocked').iCheck('check');
    //} else {
    //    $('#CatalogDto_IsLocked').iCheck('uncheck');
    //}
    //if (data.catalogDto.visible) {
    //    $('#CatalogDto_Visible').iCheck('check');
    //} else {
    //    $('#CatalogDto_Visible').iCheck('uncheck');
    //}
    //$('input[type="checkbox"]').iCheck('update');
    $('.deleteButton').off('click').on('click', deleteButton);
}

async function addTopMenu() {
    let flag = $(this).data('flag') * 1;//0 顶级，1子集
    let menuName = await base.swal.promot('请输入目录名称');
    var nodes = treeObj.getSelectedNodes()[0] || null;
    if ($.trim(menuName).length <= 0) {
        return;
    }
    if (nodes === null && flag === 1) {
        await base.swal.alert('请选中某个目录');
        return;
    }
    var parentId = (nodes || {}).id || 0;
    var newNode = { name: menuName, parentId: parentId };
    if (flag === 0) {
        nodes = null;
        newNode.parentId = 0;
    }
    debugger
    var data = await base.post('/Admin/Document/Edit?handler=AddCatalog', newNode);
    newNode.id = data;
    treeObj.addNodes(nodes || null, newNode);
}

async function submit() {
    if (!validator.checkAll('form')) {
        return;
    }
    let formData = $('form').serializeArray();
    let jsonData = {};
    formData.forEach((value, index) => { if ($.trim(value.name).length > 0) { jsonData[value.name] = value.value } });
    let $buttonContainers = $('.buttonContainer');
    let buttons = [];
    let i = 0;
    $buttonContainers.each((index, ele) => {
        //debugger
        let $ele = $(ele);
        let button = {};
        $ele.find('[data-attribute-name]').each((eleindex, input) => {
            //debugger;
            button[$(input).data('attributeName')] = $(input).val();
            formData.push({
                name: 'buttons[' + i + '][' + $(input).data('attributeName') + ']',
                value: $(input).val()
            });
        });
        i++;
        buttons.push(button);
    });
    //debugger;
    let response = await base.post('/Admin/Document/Edit', formData);
    let nodes = treeObj.getSelectedNodes();
    zTreeOnClick(undefined, undefined, nodes[0]);
    await base.swal.alert('保存成功!', 'success');
}

function bindDeleteButtonEvent() {
    $('.deleteButton').off('click').on('click', deleteButton);
}

/**
 * 删除按钮
 * */
async function deleteButton() {
    let $this = $(this);
    var isDel = await base.swal.confirm('是否删除当前按钮？', 'info');
    //debugger;
    if (!isDel) {
        return;
    }
    let buttonId = $this.parents('.buttonContainer').find('input[data-attribute-name="Id"]').val();
    
    if (buttonId.length > 0) {
        await base.post('/Admin/Document/Edit?handler=deleteButton', { buttonId: buttonId });
    }

    $this.parents('.buttonContainer').remove();
}

/**
 * 新增按钮
 * */

function addButton() {
    //$('.deleteButton').off('click').on('click', deleteButton)
    bindDeleteButtonEvent();
    var $buttonContaner = $('.buttonContainer').first().clone().attr('id', null);
    $buttonContaner.find('input').val('');
    $('.buttonContainer').last().after($buttonContaner);
    $('.deleteButton').off('click').on('click', deleteButton);
}
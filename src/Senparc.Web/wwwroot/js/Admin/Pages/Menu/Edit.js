$(function () {
    _init();
});

let treeObj;

async function _init() {
    bindDeleteButtonEvent();
    $('.addButton').off('click').on('click', addButton);

    $('#submit').on('click', submit);

    $('.addTopMenu').on('click', addTopMenu);
    

    treeObj = await initMenu(null, {
        hiddenButton: true,
        IsExpendAll: true,
        callback: {
            onClick: zTreeOnClick,
            beforeRemove: zTreeBeforeRemove
        },
        edit: {
            showRemoveBtn: true,
            showRenameBtn: false,
            enable: true
        }
    });
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
    await base.post('/Admin/Menu/Edit?handler=delete', { ids: ids });
    for (var i = 0; i < result.length; i++) {
        treeObj.removeNode(result[i]);
    }
}

async function zTreeOnClick(event, treeId, treeNode) {
    //console.info(treeNode);
    var id = treeNode.id;
    var data = await base.get('/Admin/Menu/Edit', { id: id, handler: 'detail' });

    $('.buttonContainer').not(':first').remove();
    $('.buttonContainer:first input[type="text"]').val('');
    //debugger;

    for (var i in data.sysMenuDto) {
        $('#SysMenuDto_' + i.toString().replace(/^\S/, s => s.toUpperCase())).val(data.sysMenuDto[i]);
    }   
    for (var j in data.sysButtons) {
        let $exits;
        if (j * 1 === 0) {
            $exits = $('.buttonContainer').first();
        } else {
            var $buttonContaner = $('.buttonContainer').first().clone().attr('id', null);
            $('.buttonContainer').last().after($buttonContaner);
            $exits = $buttonContaner;
        }
        
        for (i in data.sysButtons[j]) {
            let selector = '[data-attribute-name="' + i.toString().replace(/^\S/, s => s.toUpperCase()) + '"]';
            $exits.find(selector).val(data.sysButtons[j][i]);
        }
    }

    $('.deleteButton').off('click').on('click', deleteButton);
}

async function addTopMenu() {
    let flag = $(this).data('flag') * 1;//0 顶级，1子集
    let menuName = await base.swal.promot('请输入菜单名称');
    var nodes = treeObj.getSelectedNodes()[0] || null;
    if ($.trim(menuName).length <= 0) {
        return;
    }
    if (nodes === null && flag === 1) {
        await base.swal.alert('请选中某个菜单');
        return;
    }
    var parentId = (nodes || {}).id || null;
    var newNode = { menuName: menuName, parentId: parentId };
    if (flag === 0) {
        nodes = null;
        newNode.parentId = null;
    }
    var data = await base.post('/Admin/Menu/Edit?handler=AddMenu', newNode);
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
    let response = await base.post('/Admin/Menu/Edit', formData);
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
        await base.post('/Admin/Menu/Edit?handler=deleteButton', { buttonId: buttonId });
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
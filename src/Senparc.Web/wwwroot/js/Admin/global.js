
//Vue.prototype.isCollapse = false;//菜单收起/展开
//Vue.prototype.handleOpen = function (key, keyPath) {//菜单展开
//    console.log(key, keyPath);

//};
//Vue.prototype.handleClose = function (key, keyPath) {//菜单收起
//    console.log(key, keyPath);
//};

/**
 * 递归，获取所有子节点
 * @param {any} treeNode 2
 * @param {any} result 2
 * @returns {any} 节点名称
 */
function getAllChildrenNodes(treeNode, result) {
    if (treeNode.isParent) {
        var childrenNodes = treeNode.children;
        if (childrenNodes) {
            for (var i = 0; i < childrenNodes.length; i++) {
                result.push(childrenNodes[i]);
                result = getAllChildrenNodes(childrenNodes[i], result);
            }
        }
    }
    return result;
}

/**
 * 
 * @param {any} selector 选择器
 * @param {any} options 树选项
 * @returns {any} 树示例
 */
async function initMenu(selector, options) {
    return await _initMenuTree(selector || '#tree', '/Admin/Menu/Edit', { handler: 'menu' }, options);
}

/**
 * 初始化树
 * @param {any} selector 元素选择器
 * @param {any} url 请求地址
 * @param {any} params 请求参数
 * @param {any} options 选项
 * @returns {any} 树示例
 */
async function _initMenuTree(selector, url, params, options) {
    var setting = {
        data: {
            simpleData: {
                enable: true,
                pIdKey: "parentId",
                rootPId: null
            },
            key: {
                title: 'id',
                name: 'menuName',
                url: 'xdasdf'
            }
        }
    };
    setting = $.extend(setting, options);
    let responseData = await base.get(url, params);
    if (options.hiddenButton) {
        responseData = responseData.filter((ele, index) => { return ele.isMenu; });
    }
    let treeObj = $.fn.zTree.init($(selector), setting, responseData);
    if (options.IsExpendAll) {
        treeObj.expandAll(true);
    }
    return treeObj;
}


/**
 * 多选删除
 * @param {string} checkBoxName  checkbox 的name
 * @param {string} formId  表单的Id
 * @returns {any} 无返回值
 */
function deleteCheck(checkBoxName, formId) {
    var _checkedAll = $("input[name=" + checkBoxName + "]:checked");
    //debugger
    if (_checkedAll.length === 0) {
        //base.("warning", "请选择需要删除的内容");
        base.swal.alert('请选择需要删除的内容');
        return false;
    }

    swal({
        text: '确定要删除吗？',
        icon: 'warning',
        buttons: {
            cancel: {
                text: "取消",
                value: null,
                visible: true,
                className: "",
                closeModal: true,
            },
            confirm: {
                text: "确定",
                value: true,
                visible: true,
                className: "",
                closeModal: true
            }
        }
    }).then(v => {
        if (v) {
            $('#' + formId).submit();
        }
    });
}


/**
 * 复制内容
 * @param {any} value 值 
 * @param {any} toastText 提示内容
 */
function copyToClipboard(value, toastText) {
    $('#clipboardContainer').val(value);
    $('#clipboardContainer').select();
    toastText = toastText || '复制成功！请使用 Ctrl+V 组合键进行粘贴操作。';
    try {
        document.execCommand("copy"); // 执行浏览器复制命令
        base.swal.toast(toastText);
    } finally {
        //Execute Finally
    }
}


function resizeUrl() {//处理剪切url id
    let url = window.location.href;
    let obj = {};
    let reg = /[?&][^?&]+=[^?&]+/g;
    let arr = url.match(reg); // return ["?id=123456","&a=b"]
    if (arr) {
        arr.forEach((item) => {
            let tempArr = item.substring(1).split('=');
            let key = tempArr[0];
            let val = tempArr[1];
            obj[key] = decodeURIComponent(val);
        });
    }
    return obj;
}

/**
 * 将数值四舍五入(保留2位小数)后格式化成金额形式
 * @param {any} num 数值(Number或者String)
 * @returns {any} 金额格式的字符串,如'1,234,567.45'
 */
function formatCurrency(num) {
    num = num.toString().replace(/\$|\,/g, '');
    if (isNaN(num))
        num = "0";
    sign = num == (num = Math.abs(num));
    num = Math.floor(num * 100 + 0.50000000001);
    cents = num % 100;
    num = Math.floor(num / 100).toString();
    if (cents < 10)
        cents = "0" + cents;
    for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
        num = num.substring(0, num.length - (4 * i + 3)) + ',' +
            num.substring(num.length - (4 * i + 3));
    return (((sign) ? '' : '-') + num + '.' + cents);
}
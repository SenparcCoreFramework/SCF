/**
 * 全局提示框
 * @param {string} type  提示类型
 * @param {string} message  提示内容
 */
function baseAlert(type, message) {
    switch (type) {
        case 'danger':
            new PNotify({
                title: '提示',
                text: message,
                type: 'error',
                styling: 'bootstrap3'
            });
            break;
        case 'warning':
            new PNotify({
                title: '提示',
                text: message,
                styling: 'bootstrap3'
            });
            break;
        case 'info':
            new PNotify({
                title: '提示',
                text: message,
                type: 'info',
                styling: 'bootstrap3'
            });
            break;
        case 'success':
            new PNotify({
                title: '提示',
                text: message,
                type: 'success',
                styling: 'bootstrap3'
            });
            break;
    }
}
/**
 * 多选删除
 * @param {string} checkBoxName  checkbox 的name
 * @param {string} formId  表单的Id
 */
function deleteCheck(checkBoxName, formId) {
    var _checkedAll = $("input[name=" + checkBoxName + "]:checked");
    if (_checkedAll.length === 0) {
        baseAlert("warning", "请选择需要删除的内容");
        return false;
    }
    swal({
        title: "确定要删除吗?",
        text: "删除的数据不可还原!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonClass: "btn-warning",
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        closeOnConfirm: false,
        closeOnCancel: true,
        showLoaderOnConfirm: true
    }, function (isConfirm) {
        if (isConfirm) {
            $('#' + formId).submit();
        }
    });
}
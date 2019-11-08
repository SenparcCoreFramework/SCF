$(function () {
    _init();
});
function _init() {
    $('#submit').on('click', submit);
}

async function submit() {
    let accountId = resizeUrl().accountId;
    let $roleCheckBoxs = $('.roleCheckBox');
    let array = [];
    $roleCheckBoxs.each((index, ele) => {
        if ($(ele).prop('checked')) {
            array.push($(ele).val());
        }
    });
    if (array.length === 0) {
        return;
    }
    await base.post('/Admin/AdminUserInfo/AuthorizationPage', { RoleIds: array, accountId: accountId });
    await base.swal.alert('保存成功！', 'success');
    window.location.href = '/Admin/AdminUserInfo/Index';
}
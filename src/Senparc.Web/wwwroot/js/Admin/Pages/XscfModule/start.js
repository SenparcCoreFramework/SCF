function deleteModule() {
    if (!confirm('确定要删除此模块吗？数据可能永久丢失，请注意备份！')) {
        return false;
    }

    document.getElementById('form_delete').submit();
    return true;
}

function submitFunction(id) {
    var dataInputs = $('[id^=functionForm_Editor_' + id + '_]');
    var jsonData = {};
    for (var i = 0; i < dataInputs.length; i++) {
        var ele = dataInputs[i];
        var tagName = ele.tagName.toLowerCase();
        var obj = $(ele);
        var val;
        if (tagName === 'input' || tagName === 'textara') {
            val = obj.val();
        } else if (tagName === 'select') {
            val = [obj.val()];
        }
        else {
            alert('未知的编辑器类型：' + tagName);
            return false;
        }
        jsonData[obj.data('funcname')] = val;
    }
    $('#xscfFunctionParams_' + id).val(JSON.stringify(jsonData));

    $('#functionForm_' + id).ajaxSubmit(function (data) {

        var downloadArea = $('#result_small_modal_download_log');
        if ((data.log || '').length > 0 && (data.tempId || '').length > 0) {
            downloadArea.show();
            $('#result_small_modal_download_log_link').attr('href', '?handler=Log&tempId=' + data.tempId);
        } else {
            downloadArea.hide();
        }

        if (!data.success) {
            showModal('遇到错误', '错误信息', data.msg);
            return;
        }

        if (data.msg && data.msg.indexOf('http://') !== -1 || data.msg.indexOf('https://') !== -1) {
            showModal('执行成功', '收到网址，点击下方打开<br />（此链接由第三方提供，请注意安全）：', '<i class="fa fa-external-link"></i> <a href="' + data.msg + '" target="_blank">' + data.msg + '</a>');
        }
        else {
            showModal('执行成功', '返回信息', data.msg);
        }
    });
    return false;
}

function showModal(title, subtitle, contentHtml) {
    $('#result_small_modal_title').html(title);
    $('#result_small_modal_subtitle').html(subtitle);
    $('#result_small_modal_content').html((contentHtml || '').replace(/\n/g, '<br />'));

    $('#result_small_modal').modal();

}
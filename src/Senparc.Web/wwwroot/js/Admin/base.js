var base = {
    alert: function (type, message, title, position, onClose, onClick) {//type:success/warning/info/error;position: top-right/top-left/bottom-right/bottom-left
        new PNotify({
            title: "PNotify",
            type: "info",
            text: "Welcome. Try hovering over me. You can click things behind me, because I'm non-blocking.",
            nonblock: {
                nonblock: true
            },
            addclass: 'dark',
            styling: 'bootstrap3',
            hide: true,
            before_close: function (PNotify) {
                PNotify.update({
                    title: PNotify.options.title + " - Enjoy your Stay",
                    before_close: null
                });

                PNotify.queueRemove();

                return false;
            }
        });
    },
    showLoading: function (text, target, spinner, background) {
        window.loading = vm.$loading({
            target: target ? target : document.body,
            lock: true,
            text: text ? text : '加载中',
            spinner: spinner ? spinner : 'el-icon-loading',
            background: background ? background : 'rgba(0, 0, 0, 0.7)'
        });
    },
    post: function (url, data, success, fail, isShowErrorMsg) {
        $.ajax({
            url: url,
            type: 'POST',
            data: JSON.stringify(data),
            contentType: 'application/json;',
            success: function (json) {
                if (json.success) {
                    success(json.result);
                } else {
                    if (fail) {
                        fail(json.result);
                    }
                    if (isShowErrorMsg === undefined) {
                        baseAlert('danger', json.result.message);
                    }
                }
            },
            error: function (xhr, errorType, errormsg) {
                baseAlert('danger', '请求发生错误，请稍后重试！');
            },
            complete: function () {

            }
        });
    },
    get: function (url, success, fail, isShowErrorMsg) {
        $.ajax({
            url: url,
            type: 'GET',
            success: function (json) {
                if (json.success) {
                    success(json.result);
                }
                else {
                    if (fail) {
                        fail(json.result);
                    }
                    if (isShowErrorMsg === undefined) {
                        baseAlert('danger', json.result.message);
                    }
                }
            },
            error: function (xhr, errorType, error) {
                baseAlert('danger', '请求发生错误，请稍后重试！');
            },
            complete: function () {

            }
        });
    }
};

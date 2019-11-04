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

    /**
     * 发送post请求
     * @param {any} url 地址
     * @param {any} data 数据
     * @param {any} ajaxOptions ajax选项
    * @returns {Promise} promise
     */

    post: function (url, data, ajaxOptions) {
        return new Promise((resolve, reject) => {
            var ajaxSetting = {
                url: url,
                method: 'POST',
                data: data,
                headers: {
                    "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').first().val()
                },
                //contentType: 'application/json;',
                success: function (json) {
                    resolve(json.data);
                },
                error: function (xhr, errorType, errormsg) {
                    //swal.close();
                    reject(errormsg);
                    base.swal.alert('请求发生错误');
                },
                complete: function () {

                }
            };
            ajaxSetting = $.extend(ajaxSetting, ajaxOptions);
            $.ajax(ajaxSetting);
        });
    },

    /**
     * 发送get请求
     * @param {any} url 地址
     * @param {any} params 参数
     * @param {any} isShowErrorMsg 发送错误时，是否显示错误信息
     * @returns {Promise} promise
     */
    get: function (url, params, isShowErrorMsg) {
        return new Promise((resolve, reject) => {
            $.ajax({
                url: url,
                type: 'GET',
                data: params,
                success: function (json) {
                    resolve(json.data);
                },
                error: function (xhr, errorType, error) {
                    base.swal.alert('请求发生错误，请稍后重试！');
                    reject(error);
                },
                complete: function () {

                }
            });
        });
    },
    swal: {
        /**
         * 普通警告
         * @param {any} text 文字
         * @param {any} icon 类型 "warning","error", "success" and "info
         * @returns {any} promise
         */
        alert: function (text, icon) {
            return swal({
                text: text,
                icon: icon || 'warning',
                button: {
                    text: '确定'
                }
            });
        },

        /**
         * 确认
         * @param {any} text 文字
         * @param {any} icon 类型 "warning","error", "success" and "info
        * @returns {any} promise
         */
        confirm: function (text, icon) {
            return swal({
                text: text,
                icon: icon || 'warning',
                buttons: {
                    cancel: {
                        text: "取消",
                        value: null,
                        visible: true,
                        className: "",
                        closeModal: true
                    },
                    confirm: {
                        text: "确定",
                        value: true,
                        visible: true,
                        className: "",
                        closeModal: true
                    }
                }
            });
        },

        /**
         * 
         * @param {any} text 文字
         * @param {any} icon 类型 "warning","error", "success" and "info
        * @returns {any} promise
         */
        promot: function (text, icon) {
            return swal({
                text: text,
                icon: icon || 'info',
                content: "input",
                //closeOnClickOutside: false,
                button: {
                    text: "确定",
                    closeModal: true
                }
            });
        },
        load: function () {
            swal({
                title: "",
                text: "加载中...",
                //type: "none",
                //imageUrl:'/images/loading.png',
                showCancelButton: false,
                showConfirmButton: false,
                closeOnConfirm: false,
                showLoaderOnConfirm: true,
            });
        },
        toast: function (title) {
            swal(title, {
                buttons: false,
                timer: 2000,
            });
        },
        /**
         * iframe 弹层
         * @param {any} options 参数
         * */
        iframModal: function (options) {
            let defaultOptions = {};
            defaultOptions = $.extend(defaultOptions, options);
        },
        swalMain: swal
    }
};

import _extends from 'babel-runtime/helpers/extends';
import _defineProperty from 'babel-runtime/helpers/defineProperty';

var _this = this;

import * as React from 'react';
import * as ReactDOM from 'react-dom';
import classNames from 'classnames';
import Icon from '../icon';
import Dialog from './Modal';
import ActionButton from './ActionButton';
import { getConfirmLocale } from './locale';
var IS_REACT_16 = !!ReactDOM.createPortal;
var ConfirmDialog = function ConfirmDialog(props) {
    var onCancel = props.onCancel,
        onOk = props.onOk,
        close = props.close,
        zIndex = props.zIndex,
        afterClose = props.afterClose,
        visible = props.visible,
        keyboard = props.keyboard,
        centered = props.centered,
        getContainer = props.getContainer,
        maskStyle = props.maskStyle,
        okButtonProps = props.okButtonProps,
        cancelButtonProps = props.cancelButtonProps;

    var iconType = props.iconType || 'question-circle';
    var okType = props.okType || 'primary';
    var prefixCls = props.prefixCls || 'ant-modal';
    var contentPrefixCls = prefixCls + '-confirm';
    // 默认为 true，保持向下兼容
    var okCancel = 'okCancel' in props ? props.okCancel : true;
    var width = props.width || 416;
    var style = props.style || {};
    // 默认为 false，保持旧版默认行为
    var maskClosable = props.maskClosable === undefined ? false : props.maskClosable;
    var runtimeLocale = getConfirmLocale();
    var okText = props.okText || (okCancel ? runtimeLocale.okText : runtimeLocale.justOkText);
    var cancelText = props.cancelText || runtimeLocale.cancelText;
    var autoFocusButton = props.autoFocusButton === null ? false : props.autoFocusButton || 'ok';
    var classString = classNames(contentPrefixCls, contentPrefixCls + '-' + props.type, props.className);
    var cancelButton = okCancel && React.createElement(
        ActionButton,
        { actionFn: onCancel, closeModal: close, autoFocus: autoFocusButton === 'cancel', buttonProps: cancelButtonProps },
        cancelText
    );
    return React.createElement(
        Dialog,
        { prefixCls: prefixCls, className: classString, wrapClassName: classNames(_defineProperty({}, contentPrefixCls + '-centered', !!props.centered)), onCancel: close.bind(_this, { triggerCancel: true }), visible: visible, title: '', transitionName: 'zoom', footer: '', maskTransitionName: 'fade', maskClosable: maskClosable, maskStyle: maskStyle, style: style, width: width, zIndex: zIndex, afterClose: afterClose, keyboard: keyboard, centered: centered, getContainer: getContainer },
        React.createElement(
            'div',
            { className: contentPrefixCls + '-body-wrapper' },
            React.createElement(
                'div',
                { className: contentPrefixCls + '-body' },
                React.createElement(Icon, { type: iconType }),
                React.createElement(
                    'span',
                    { className: contentPrefixCls + '-title' },
                    props.title
                ),
                React.createElement(
                    'div',
                    { className: contentPrefixCls + '-content' },
                    props.content
                )
            ),
            React.createElement(
                'div',
                { className: contentPrefixCls + '-btns' },
                cancelButton,
                React.createElement(
                    ActionButton,
                    { type: okType, actionFn: onOk, closeModal: close, autoFocus: autoFocusButton === 'ok', buttonProps: okButtonProps },
                    okText
                )
            )
        )
    );
};
export default function confirm(config) {
    var div = document.createElement('div');
    document.body.appendChild(div);
    var currentConfig = _extends({}, config, { close: close, visible: true });
    function close() {
        for (var _len = arguments.length, args = Array(_len), _key = 0; _key < _len; _key++) {
            args[_key] = arguments[_key];
        }

        currentConfig = _extends({}, currentConfig, { visible: false, afterClose: destroy.bind.apply(destroy, [this].concat(args)) });
        if (IS_REACT_16) {
            render(currentConfig);
        } else {
            destroy.apply(undefined, args);
        }
    }
    function update(newConfig) {
        currentConfig = _extends({}, currentConfig, newConfig);
        render(currentConfig);
    }
    function destroy() {
        var unmountResult = ReactDOM.unmountComponentAtNode(div);
        if (unmountResult && div.parentNode) {
            div.parentNode.removeChild(div);
        }

        for (var _len2 = arguments.length, args = Array(_len2), _key2 = 0; _key2 < _len2; _key2++) {
            args[_key2] = arguments[_key2];
        }

        var triggerCancel = args && args.length && args.some(function (param) {
            return param && param.triggerCancel;
        });
        if (config.onCancel && triggerCancel) {
            config.onCancel.apply(config, args);
        }
    }
    function render(props) {
        ReactDOM.render(React.createElement(ConfirmDialog, props), div);
    }
    render(currentConfig);
    return {
        destroy: close,
        update: update
    };
}
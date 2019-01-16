'use strict';

Object.defineProperty(exports, "__esModule", {
    value: true
});

var _extends2 = require('babel-runtime/helpers/extends');

var _extends3 = _interopRequireDefault(_extends2);

var _defineProperty2 = require('babel-runtime/helpers/defineProperty');

var _defineProperty3 = _interopRequireDefault(_defineProperty2);

exports['default'] = confirm;

var _react = require('react');

var React = _interopRequireWildcard(_react);

var _reactDom = require('react-dom');

var ReactDOM = _interopRequireWildcard(_reactDom);

var _classnames = require('classnames');

var _classnames2 = _interopRequireDefault(_classnames);

var _icon = require('../icon');

var _icon2 = _interopRequireDefault(_icon);

var _Modal = require('./Modal');

var _Modal2 = _interopRequireDefault(_Modal);

var _ActionButton = require('./ActionButton');

var _ActionButton2 = _interopRequireDefault(_ActionButton);

var _locale = require('./locale');

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj['default'] = obj; return newObj; } }

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { 'default': obj }; }

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
    var runtimeLocale = (0, _locale.getConfirmLocale)();
    var okText = props.okText || (okCancel ? runtimeLocale.okText : runtimeLocale.justOkText);
    var cancelText = props.cancelText || runtimeLocale.cancelText;
    var autoFocusButton = props.autoFocusButton === null ? false : props.autoFocusButton || 'ok';
    var classString = (0, _classnames2['default'])(contentPrefixCls, contentPrefixCls + '-' + props.type, props.className);
    var cancelButton = okCancel && React.createElement(
        _ActionButton2['default'],
        { actionFn: onCancel, closeModal: close, autoFocus: autoFocusButton === 'cancel', buttonProps: cancelButtonProps },
        cancelText
    );
    return React.createElement(
        _Modal2['default'],
        { prefixCls: prefixCls, className: classString, wrapClassName: (0, _classnames2['default'])((0, _defineProperty3['default'])({}, contentPrefixCls + '-centered', !!props.centered)), onCancel: close.bind(undefined, { triggerCancel: true }), visible: visible, title: '', transitionName: 'zoom', footer: '', maskTransitionName: 'fade', maskClosable: maskClosable, maskStyle: maskStyle, style: style, width: width, zIndex: zIndex, afterClose: afterClose, keyboard: keyboard, centered: centered, getContainer: getContainer },
        React.createElement(
            'div',
            { className: contentPrefixCls + '-body-wrapper' },
            React.createElement(
                'div',
                { className: contentPrefixCls + '-body' },
                React.createElement(_icon2['default'], { type: iconType }),
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
                    _ActionButton2['default'],
                    { type: okType, actionFn: onOk, closeModal: close, autoFocus: autoFocusButton === 'ok', buttonProps: okButtonProps },
                    okText
                )
            )
        )
    );
};
function confirm(config) {
    var div = document.createElement('div');
    document.body.appendChild(div);
    var currentConfig = (0, _extends3['default'])({}, config, { close: close, visible: true });
    function close() {
        for (var _len = arguments.length, args = Array(_len), _key = 0; _key < _len; _key++) {
            args[_key] = arguments[_key];
        }

        currentConfig = (0, _extends3['default'])({}, currentConfig, { visible: false, afterClose: destroy.bind.apply(destroy, [this].concat(args)) });
        if (IS_REACT_16) {
            render(currentConfig);
        } else {
            destroy.apply(undefined, args);
        }
    }
    function update(newConfig) {
        currentConfig = (0, _extends3['default'])({}, currentConfig, newConfig);
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
module.exports = exports['default'];
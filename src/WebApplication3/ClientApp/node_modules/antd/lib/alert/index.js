'use strict';

Object.defineProperty(exports, "__esModule", {
    value: true
});

var _extends2 = require('babel-runtime/helpers/extends');

var _extends3 = _interopRequireDefault(_extends2);

var _defineProperty2 = require('babel-runtime/helpers/defineProperty');

var _defineProperty3 = _interopRequireDefault(_defineProperty2);

var _classCallCheck2 = require('babel-runtime/helpers/classCallCheck');

var _classCallCheck3 = _interopRequireDefault(_classCallCheck2);

var _createClass2 = require('babel-runtime/helpers/createClass');

var _createClass3 = _interopRequireDefault(_createClass2);

var _possibleConstructorReturn2 = require('babel-runtime/helpers/possibleConstructorReturn');

var _possibleConstructorReturn3 = _interopRequireDefault(_possibleConstructorReturn2);

var _inherits2 = require('babel-runtime/helpers/inherits');

var _inherits3 = _interopRequireDefault(_inherits2);

var _react = require('react');

var React = _interopRequireWildcard(_react);

var _reactDom = require('react-dom');

var ReactDOM = _interopRequireWildcard(_reactDom);

var _rcAnimate = require('rc-animate');

var _rcAnimate2 = _interopRequireDefault(_rcAnimate);

var _icon = require('../icon');

var _icon2 = _interopRequireDefault(_icon);

var _classnames = require('classnames');

var _classnames2 = _interopRequireDefault(_classnames);

var _getDataOrAriaProps = require('../_util/getDataOrAriaProps');

var _getDataOrAriaProps2 = _interopRequireDefault(_getDataOrAriaProps);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj['default'] = obj; return newObj; } }

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { 'default': obj }; }

function noop() {}

var Alert = function (_React$Component) {
    (0, _inherits3['default'])(Alert, _React$Component);

    function Alert() {
        (0, _classCallCheck3['default'])(this, Alert);

        var _this = (0, _possibleConstructorReturn3['default'])(this, (Alert.__proto__ || Object.getPrototypeOf(Alert)).apply(this, arguments));

        _this.state = {
            closing: true,
            closed: false
        };
        _this.handleClose = function (e) {
            e.preventDefault();
            var dom = ReactDOM.findDOMNode(_this);
            dom.style.height = dom.offsetHeight + 'px';
            // Magic code
            // 重复一次后才能正确设置 height
            dom.style.height = dom.offsetHeight + 'px';
            _this.setState({
                closing: false
            });
            (_this.props.onClose || noop)(e);
        };
        _this.animationEnd = function () {
            _this.setState({
                closed: true,
                closing: true
            });
            (_this.props.afterClose || noop)();
        };
        return _this;
    }

    (0, _createClass3['default'])(Alert, [{
        key: 'render',
        value: function render() {
            var _classNames, _classNames2;

            var _props = this.props,
                description = _props.description,
                _props$prefixCls = _props.prefixCls,
                prefixCls = _props$prefixCls === undefined ? 'ant-alert' : _props$prefixCls,
                message = _props.message,
                closeText = _props.closeText,
                banner = _props.banner,
                _props$className = _props.className,
                className = _props$className === undefined ? '' : _props$className,
                style = _props.style,
                icon = _props.icon;
            var _props2 = this.props,
                closable = _props2.closable,
                type = _props2.type,
                showIcon = _props2.showIcon,
                iconType = _props2.iconType;
            // banner模式默认有 Icon

            showIcon = banner && showIcon === undefined ? true : showIcon;
            // banner模式默认为警告
            type = banner && type === undefined ? 'warning' : type || 'info';
            var iconTheme = 'filled';
            // should we give a warning?
            // warning(!iconType, `The property 'iconType' is deprecated. Use the property 'icon' instead.`);
            if (!iconType) {
                switch (type) {
                    case 'success':
                        iconType = 'check-circle';
                        break;
                    case 'info':
                        iconType = 'info-circle';
                        break;
                    case 'error':
                        iconType = 'close-circle';
                        break;
                    case 'warning':
                        iconType = 'exclamation-circle';
                        break;
                    default:
                        iconType = 'default';
                }
                // use outline icon in alert with description
                if (!!description) {
                    iconTheme = 'outlined';
                }
            }
            // closeable when closeText is assigned
            if (closeText) {
                closable = true;
            }
            var alertCls = (0, _classnames2['default'])(prefixCls, prefixCls + '-' + type, (_classNames = {}, (0, _defineProperty3['default'])(_classNames, prefixCls + '-close', !this.state.closing), (0, _defineProperty3['default'])(_classNames, prefixCls + '-with-description', !!description), (0, _defineProperty3['default'])(_classNames, prefixCls + '-no-icon', !showIcon), (0, _defineProperty3['default'])(_classNames, prefixCls + '-banner', !!banner), (0, _defineProperty3['default'])(_classNames, prefixCls + '-closable', closable), _classNames), className);
            var closeIcon = closable ? React.createElement(
                'a',
                { onClick: this.handleClose, className: prefixCls + '-close-icon' },
                closeText || React.createElement(_icon2['default'], { type: 'close' })
            ) : null;
            var dataOrAriaProps = (0, _getDataOrAriaProps2['default'])(this.props);
            var iconNode = icon && (React.isValidElement(icon) ? React.cloneElement(icon, {
                className: (0, _classnames2['default'])((_classNames2 = {}, (0, _defineProperty3['default'])(_classNames2, icon.props.className, icon.props.className), (0, _defineProperty3['default'])(_classNames2, prefixCls + '-icon', true), _classNames2))
            }) : React.createElement(
                'span',
                { className: prefixCls + '-icon' },
                icon
            )) || React.createElement(_icon2['default'], { className: prefixCls + '-icon', type: iconType, theme: iconTheme });
            return this.state.closed ? null : React.createElement(
                _rcAnimate2['default'],
                { component: '', showProp: 'data-show', transitionName: prefixCls + '-slide-up', onEnd: this.animationEnd },
                React.createElement(
                    'div',
                    (0, _extends3['default'])({ 'data-show': this.state.closing, className: alertCls, style: style }, dataOrAriaProps),
                    showIcon ? iconNode : null,
                    React.createElement(
                        'span',
                        { className: prefixCls + '-message' },
                        message
                    ),
                    React.createElement(
                        'span',
                        { className: prefixCls + '-description' },
                        description
                    ),
                    closeIcon
                )
            );
        }
    }]);
    return Alert;
}(React.Component);

exports['default'] = Alert;
module.exports = exports['default'];
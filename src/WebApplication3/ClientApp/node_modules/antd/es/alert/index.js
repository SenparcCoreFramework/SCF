import _extends from 'babel-runtime/helpers/extends';
import _defineProperty from 'babel-runtime/helpers/defineProperty';
import _classCallCheck from 'babel-runtime/helpers/classCallCheck';
import _createClass from 'babel-runtime/helpers/createClass';
import _possibleConstructorReturn from 'babel-runtime/helpers/possibleConstructorReturn';
import _inherits from 'babel-runtime/helpers/inherits';
import * as React from 'react';
import * as ReactDOM from 'react-dom';
import Animate from 'rc-animate';
import Icon from '../icon';
import classNames from 'classnames';
import getDataOrAriaProps from '../_util/getDataOrAriaProps';
function noop() {}

var Alert = function (_React$Component) {
    _inherits(Alert, _React$Component);

    function Alert() {
        _classCallCheck(this, Alert);

        var _this = _possibleConstructorReturn(this, (Alert.__proto__ || Object.getPrototypeOf(Alert)).apply(this, arguments));

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

    _createClass(Alert, [{
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
            var alertCls = classNames(prefixCls, prefixCls + '-' + type, (_classNames = {}, _defineProperty(_classNames, prefixCls + '-close', !this.state.closing), _defineProperty(_classNames, prefixCls + '-with-description', !!description), _defineProperty(_classNames, prefixCls + '-no-icon', !showIcon), _defineProperty(_classNames, prefixCls + '-banner', !!banner), _defineProperty(_classNames, prefixCls + '-closable', closable), _classNames), className);
            var closeIcon = closable ? React.createElement(
                'a',
                { onClick: this.handleClose, className: prefixCls + '-close-icon' },
                closeText || React.createElement(Icon, { type: 'close' })
            ) : null;
            var dataOrAriaProps = getDataOrAriaProps(this.props);
            var iconNode = icon && (React.isValidElement(icon) ? React.cloneElement(icon, {
                className: classNames((_classNames2 = {}, _defineProperty(_classNames2, icon.props.className, icon.props.className), _defineProperty(_classNames2, prefixCls + '-icon', true), _classNames2))
            }) : React.createElement(
                'span',
                { className: prefixCls + '-icon' },
                icon
            )) || React.createElement(Icon, { className: prefixCls + '-icon', type: iconType, theme: iconTheme });
            return this.state.closed ? null : React.createElement(
                Animate,
                { component: '', showProp: 'data-show', transitionName: prefixCls + '-slide-up', onEnd: this.animationEnd },
                React.createElement(
                    'div',
                    _extends({ 'data-show': this.state.closing, className: alertCls, style: style }, dataOrAriaProps),
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

export default Alert;
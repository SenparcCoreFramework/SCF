import _extends from 'babel-runtime/helpers/extends';
import _classCallCheck from 'babel-runtime/helpers/classCallCheck';
import _createClass from 'babel-runtime/helpers/createClass';
import _possibleConstructorReturn from 'babel-runtime/helpers/possibleConstructorReturn';
import _inherits from 'babel-runtime/helpers/inherits';
var __rest = this && this.__rest || function (s, e) {
    var t = {};
    for (var p in s) {
        if (Object.prototype.hasOwnProperty.call(s, p) && e.indexOf(p) < 0) t[p] = s[p];
    }if (s != null && typeof Object.getOwnPropertySymbols === "function") for (var i = 0, p = Object.getOwnPropertySymbols(s); i < p.length; i++) {
        if (e.indexOf(p[i]) < 0) t[p[i]] = s[p[i]];
    }return t;
};
import * as React from 'react';
import { polyfill } from 'react-lifecycles-compat';
import Tooltip from '../tooltip';
import Icon from '../icon';
import Button from '../button';
import LocaleReceiver from '../locale-provider/LocaleReceiver';
import defaultLocale from '../locale-provider/default';

var Popconfirm = function (_React$Component) {
    _inherits(Popconfirm, _React$Component);

    function Popconfirm(props) {
        _classCallCheck(this, Popconfirm);

        var _this = _possibleConstructorReturn(this, (Popconfirm.__proto__ || Object.getPrototypeOf(Popconfirm)).call(this, props));

        _this.onConfirm = function (e) {
            _this.setVisible(false, e);
            var onConfirm = _this.props.onConfirm;

            if (onConfirm) {
                onConfirm.call(_this, e);
            }
        };
        _this.onCancel = function (e) {
            _this.setVisible(false, e);
            var onCancel = _this.props.onCancel;

            if (onCancel) {
                onCancel.call(_this, e);
            }
        };
        _this.onVisibleChange = function (visible) {
            _this.setVisible(visible);
        };
        _this.saveTooltip = function (node) {
            _this.tooltip = node;
        };
        _this.renderOverlay = function (popconfirmLocale) {
            var _this$props = _this.props,
                prefixCls = _this$props.prefixCls,
                okButtonProps = _this$props.okButtonProps,
                cancelButtonProps = _this$props.cancelButtonProps,
                title = _this$props.title,
                cancelText = _this$props.cancelText,
                okText = _this$props.okText,
                okType = _this$props.okType,
                icon = _this$props.icon;

            return React.createElement(
                'div',
                null,
                React.createElement(
                    'div',
                    { className: prefixCls + '-inner-content' },
                    React.createElement(
                        'div',
                        { className: prefixCls + '-message' },
                        icon,
                        React.createElement(
                            'div',
                            { className: prefixCls + '-message-title' },
                            title
                        )
                    ),
                    React.createElement(
                        'div',
                        { className: prefixCls + '-buttons' },
                        React.createElement(
                            Button,
                            _extends({ onClick: _this.onCancel, size: 'small' }, cancelButtonProps),
                            cancelText || popconfirmLocale.cancelText
                        ),
                        React.createElement(
                            Button,
                            _extends({ onClick: _this.onConfirm, type: okType, size: 'small' }, okButtonProps),
                            okText || popconfirmLocale.okText
                        )
                    )
                )
            );
        };
        _this.state = {
            visible: props.visible
        };
        return _this;
    }

    _createClass(Popconfirm, [{
        key: 'getPopupDomNode',
        value: function getPopupDomNode() {
            return this.tooltip.getPopupDomNode();
        }
    }, {
        key: 'setVisible',
        value: function setVisible(visible, e) {
            var props = this.props;
            if (!('visible' in props)) {
                this.setState({ visible: visible });
            }
            var onVisibleChange = props.onVisibleChange;

            if (onVisibleChange) {
                onVisibleChange(visible, e);
            }
        }
    }, {
        key: 'render',
        value: function render() {
            var _a = this.props,
                prefixCls = _a.prefixCls,
                placement = _a.placement,
                restProps = __rest(_a, ["prefixCls", "placement"]);
            var overlay = React.createElement(
                LocaleReceiver,
                { componentName: 'Popconfirm', defaultLocale: defaultLocale.Popconfirm },
                this.renderOverlay
            );
            return React.createElement(Tooltip, _extends({}, restProps, { prefixCls: prefixCls, placement: placement, onVisibleChange: this.onVisibleChange, visible: this.state.visible, overlay: overlay, ref: this.saveTooltip }));
        }
    }], [{
        key: 'getDerivedStateFromProps',
        value: function getDerivedStateFromProps(nextProps) {
            if ('visible' in nextProps) {
                return { visible: nextProps.visible };
            } else if ('defaultVisible' in nextProps) {
                return { visible: nextProps.defaultVisible };
            }
            return null;
        }
    }]);

    return Popconfirm;
}(React.Component);

Popconfirm.defaultProps = {
    prefixCls: 'ant-popover',
    transitionName: 'zoom-big',
    placement: 'top',
    trigger: 'click',
    okType: 'primary',
    icon: React.createElement(Icon, { type: 'exclamation-circle', theme: 'filled' })
};
polyfill(Popconfirm);
export default Popconfirm;
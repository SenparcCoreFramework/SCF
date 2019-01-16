import _extends from 'babel-runtime/helpers/extends';
import _classCallCheck from 'babel-runtime/helpers/classCallCheck';
import _createClass from 'babel-runtime/helpers/createClass';
import _possibleConstructorReturn from 'babel-runtime/helpers/possibleConstructorReturn';
import _inherits from 'babel-runtime/helpers/inherits';
import * as React from 'react';
import RcDropdown from 'rc-dropdown';
import classNames from 'classnames';
import { ConfigConsumer } from '../config-provider';
import warning from '../_util/warning';
import Icon from '../icon';

var Dropdown = function (_React$Component) {
    _inherits(Dropdown, _React$Component);

    function Dropdown() {
        _classCallCheck(this, Dropdown);

        var _this = _possibleConstructorReturn(this, (Dropdown.__proto__ || Object.getPrototypeOf(Dropdown)).apply(this, arguments));

        _this.renderDropDown = function (_ref) {
            var getContextPopupContainer = _ref.getPopupContainer;
            var _this$props = _this.props,
                children = _this$props.children,
                prefixCls = _this$props.prefixCls,
                overlayElements = _this$props.overlay,
                trigger = _this$props.trigger,
                disabled = _this$props.disabled,
                getPopupContainer = _this$props.getPopupContainer;

            var child = React.Children.only(children);
            var overlay = React.Children.only(overlayElements);
            var dropdownTrigger = React.cloneElement(child, {
                className: classNames(child.props.className, prefixCls + '-trigger'),
                disabled: disabled
            });
            // menu cannot be selectable in dropdown defaultly
            // menu should be focusable in dropdown defaultly
            var _overlay$props = overlay.props,
                _overlay$props$select = _overlay$props.selectable,
                selectable = _overlay$props$select === undefined ? false : _overlay$props$select,
                _overlay$props$focusa = _overlay$props.focusable,
                focusable = _overlay$props$focusa === undefined ? true : _overlay$props$focusa;

            var expandIcon = React.createElement(
                'span',
                { className: prefixCls + '-menu-submenu-arrow' },
                React.createElement(Icon, { type: 'right', className: prefixCls + '-menu-submenu-arrow-icon' })
            );
            var fixedModeOverlay = typeof overlay.type === 'string' ? overlay : React.cloneElement(overlay, {
                mode: 'vertical',
                selectable: selectable,
                focusable: focusable,
                expandIcon: expandIcon
            });
            var triggerActions = disabled ? [] : trigger;
            var alignPoint = void 0;
            if (triggerActions && triggerActions.indexOf('contextMenu') !== -1) {
                alignPoint = true;
            }
            return React.createElement(
                RcDropdown,
                _extends({ alignPoint: alignPoint }, _this.props, { getPopupContainer: getPopupContainer || getContextPopupContainer, transitionName: _this.getTransitionName(), trigger: triggerActions, overlay: fixedModeOverlay }),
                dropdownTrigger
            );
        };
        return _this;
    }

    _createClass(Dropdown, [{
        key: 'getTransitionName',
        value: function getTransitionName() {
            var _props = this.props,
                _props$placement = _props.placement,
                placement = _props$placement === undefined ? '' : _props$placement,
                transitionName = _props.transitionName;

            if (transitionName !== undefined) {
                return transitionName;
            }
            if (placement.indexOf('top') >= 0) {
                return 'slide-down';
            }
            return 'slide-up';
        }
    }, {
        key: 'componentDidMount',
        value: function componentDidMount() {
            var overlay = this.props.overlay;

            if (overlay) {
                var overlayProps = overlay.props;
                warning(!overlayProps.mode || overlayProps.mode === 'vertical', 'mode="' + overlayProps.mode + '" is not supported for Dropdown\'s Menu.');
            }
        }
    }, {
        key: 'render',
        value: function render() {
            return React.createElement(
                ConfigConsumer,
                null,
                this.renderDropDown
            );
        }
    }]);

    return Dropdown;
}(React.Component);

export default Dropdown;

Dropdown.defaultProps = {
    prefixCls: 'ant-dropdown',
    mouseEnterDelay: 0.15,
    mouseLeaveDelay: 0.1,
    placement: 'bottomLeft'
};
'use strict';

Object.defineProperty(exports, "__esModule", {
    value: true
});

var _extends2 = require('babel-runtime/helpers/extends');

var _extends3 = _interopRequireDefault(_extends2);

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

var _rcDropdown = require('rc-dropdown');

var _rcDropdown2 = _interopRequireDefault(_rcDropdown);

var _classnames = require('classnames');

var _classnames2 = _interopRequireDefault(_classnames);

var _configProvider = require('../config-provider');

var _warning = require('../_util/warning');

var _warning2 = _interopRequireDefault(_warning);

var _icon = require('../icon');

var _icon2 = _interopRequireDefault(_icon);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj['default'] = obj; return newObj; } }

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { 'default': obj }; }

var Dropdown = function (_React$Component) {
    (0, _inherits3['default'])(Dropdown, _React$Component);

    function Dropdown() {
        (0, _classCallCheck3['default'])(this, Dropdown);

        var _this = (0, _possibleConstructorReturn3['default'])(this, (Dropdown.__proto__ || Object.getPrototypeOf(Dropdown)).apply(this, arguments));

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
                className: (0, _classnames2['default'])(child.props.className, prefixCls + '-trigger'),
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
                React.createElement(_icon2['default'], { type: 'right', className: prefixCls + '-menu-submenu-arrow-icon' })
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
                _rcDropdown2['default'],
                (0, _extends3['default'])({ alignPoint: alignPoint }, _this.props, { getPopupContainer: getPopupContainer || getContextPopupContainer, transitionName: _this.getTransitionName(), trigger: triggerActions, overlay: fixedModeOverlay }),
                dropdownTrigger
            );
        };
        return _this;
    }

    (0, _createClass3['default'])(Dropdown, [{
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
                (0, _warning2['default'])(!overlayProps.mode || overlayProps.mode === 'vertical', 'mode="' + overlayProps.mode + '" is not supported for Dropdown\'s Menu.');
            }
        }
    }, {
        key: 'render',
        value: function render() {
            return React.createElement(
                _configProvider.ConfigConsumer,
                null,
                this.renderDropDown
            );
        }
    }]);
    return Dropdown;
}(React.Component);

exports['default'] = Dropdown;

Dropdown.defaultProps = {
    prefixCls: 'ant-dropdown',
    mouseEnterDelay: 0.15,
    mouseLeaveDelay: 0.1,
    placement: 'bottomLeft'
};
module.exports = exports['default'];
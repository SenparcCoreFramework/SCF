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

var _propTypes = require('prop-types');

var PropTypes = _interopRequireWildcard(_propTypes);

var _rcSelect = require('rc-select');

var _rcSelect2 = _interopRequireDefault(_rcSelect);

var _classnames = require('classnames');

var _classnames2 = _interopRequireDefault(_classnames);

var _LocaleReceiver = require('../locale-provider/LocaleReceiver');

var _LocaleReceiver2 = _interopRequireDefault(_LocaleReceiver);

var _default = require('../locale-provider/default');

var _default2 = _interopRequireDefault(_default);

var _configProvider = require('../config-provider');

var _omit = require('omit.js');

var _omit2 = _interopRequireDefault(_omit);

var _warning = require('warning');

var _warning2 = _interopRequireDefault(_warning);

var _icon = require('../icon');

var _icon2 = _interopRequireDefault(_icon);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj['default'] = obj; return newObj; } }

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { 'default': obj }; }

var __rest = undefined && undefined.__rest || function (s, e) {
    var t = {};
    for (var p in s) {
        if (Object.prototype.hasOwnProperty.call(s, p) && e.indexOf(p) < 0) t[p] = s[p];
    }if (s != null && typeof Object.getOwnPropertySymbols === "function") for (var i = 0, p = Object.getOwnPropertySymbols(s); i < p.length; i++) {
        if (e.indexOf(p[i]) < 0) t[p[i]] = s[p[i]];
    }return t;
};

var SelectPropTypes = {
    prefixCls: PropTypes.string,
    className: PropTypes.string,
    size: PropTypes.oneOf(['default', 'large', 'small']),
    notFoundContent: PropTypes.any,
    showSearch: PropTypes.bool,
    optionLabelProp: PropTypes.string,
    transitionName: PropTypes.string,
    choiceTransitionName: PropTypes.string,
    id: PropTypes.string
};
// => It is needless to export the declaration of below two inner components.
// export { Option, OptGroup };

var Select = function (_React$Component) {
    (0, _inherits3['default'])(Select, _React$Component);

    function Select(props) {
        (0, _classCallCheck3['default'])(this, Select);

        var _this = (0, _possibleConstructorReturn3['default'])(this, (Select.__proto__ || Object.getPrototypeOf(Select)).call(this, props));

        _this.saveSelect = function (node) {
            _this.rcSelect = node;
        };
        _this.renderSelect = function (locale) {
            var _classNames;

            var _a = _this.props,
                prefixCls = _a.prefixCls,
                _a$className = _a.className,
                className = _a$className === undefined ? '' : _a$className,
                size = _a.size,
                mode = _a.mode,
                getPopupContainer = _a.getPopupContainer,
                removeIcon = _a.removeIcon,
                clearIcon = _a.clearIcon,
                menuItemSelectedIcon = _a.menuItemSelectedIcon,
                restProps = __rest(_a, ["prefixCls", "className", "size", "mode", "getPopupContainer", "removeIcon", "clearIcon", "menuItemSelectedIcon"]);
            var rest = (0, _omit2['default'])(restProps, ['inputIcon']);
            var cls = (0, _classnames2['default'])((_classNames = {}, (0, _defineProperty3['default'])(_classNames, prefixCls + '-lg', size === 'large'), (0, _defineProperty3['default'])(_classNames, prefixCls + '-sm', size === 'small'), _classNames), className);
            var optionLabelProp = _this.props.optionLabelProp;

            if (_this.isCombobox()) {
                // children 带 dom 结构时，无法填入输入框
                optionLabelProp = optionLabelProp || 'value';
            }
            var modeConfig = {
                multiple: mode === 'multiple',
                tags: mode === 'tags',
                combobox: _this.isCombobox()
            };
            var finalRemoveIcon = removeIcon && (React.isValidElement(removeIcon) ? React.cloneElement(removeIcon, {
                className: (0, _classnames2['default'])(removeIcon.props.className, prefixCls + '-remove-icon')
            }) : removeIcon) || React.createElement(_icon2['default'], { type: 'close', className: prefixCls + '-remove-icon' });
            var finalClearIcon = clearIcon && (React.isValidElement(clearIcon) ? React.cloneElement(clearIcon, {
                className: (0, _classnames2['default'])(clearIcon.props.className, prefixCls + '-clear-icon')
            }) : clearIcon) || React.createElement(_icon2['default'], { type: 'close-circle', theme: 'filled', className: prefixCls + '-clear-icon' });
            var finalMenuItemSelectedIcon = menuItemSelectedIcon && (React.isValidElement(menuItemSelectedIcon) ? React.cloneElement(menuItemSelectedIcon, {
                className: (0, _classnames2['default'])(menuItemSelectedIcon.props.className, prefixCls + '-selected-icon')
            }) : menuItemSelectedIcon) || React.createElement(_icon2['default'], { type: 'check', className: prefixCls + '-selected-icon' });
            return React.createElement(
                _configProvider.ConfigConsumer,
                null,
                function (_ref) {
                    var getContextPopupContainer = _ref.getPopupContainer;

                    return React.createElement(_rcSelect2['default'], (0, _extends3['default'])({ inputIcon: _this.renderSuffixIcon(), removeIcon: finalRemoveIcon, clearIcon: finalClearIcon, menuItemSelectedIcon: finalMenuItemSelectedIcon }, rest, modeConfig, { prefixCls: prefixCls, className: cls, optionLabelProp: optionLabelProp || 'children', notFoundContent: _this.getNotFoundContent(locale), getPopupContainer: getPopupContainer || getContextPopupContainer, ref: _this.saveSelect }));
                }
            );
        };
        (0, _warning2['default'])(props.mode !== 'combobox', 'The combobox mode of Select is deprecated,' + 'it will be removed in next major version,' + 'please use AutoComplete instead');
        return _this;
    }

    (0, _createClass3['default'])(Select, [{
        key: 'focus',
        value: function focus() {
            this.rcSelect.focus();
        }
    }, {
        key: 'blur',
        value: function blur() {
            this.rcSelect.blur();
        }
    }, {
        key: 'getNotFoundContent',
        value: function getNotFoundContent(locale) {
            var notFoundContent = this.props.notFoundContent;

            if (this.isCombobox()) {
                // AutoComplete don't have notFoundContent defaultly
                return notFoundContent === undefined ? null : notFoundContent;
            }
            return notFoundContent === undefined ? locale.notFoundContent : notFoundContent;
        }
    }, {
        key: 'isCombobox',
        value: function isCombobox() {
            var mode = this.props.mode;

            return mode === 'combobox' || mode === Select.SECRET_COMBOBOX_MODE_DO_NOT_USE;
        }
    }, {
        key: 'renderSuffixIcon',
        value: function renderSuffixIcon() {
            var _props = this.props,
                prefixCls = _props.prefixCls,
                loading = _props.loading,
                suffixIcon = _props.suffixIcon;

            if (suffixIcon) {
                return React.isValidElement(suffixIcon) ? React.cloneElement(suffixIcon, {
                    className: (0, _classnames2['default'])(suffixIcon.props.className, prefixCls + '-arrow-icon')
                }) : suffixIcon;
            }
            if (loading) {
                return React.createElement(_icon2['default'], { type: 'loading' });
            }
            return React.createElement(_icon2['default'], { type: 'down', className: prefixCls + '-arrow-icon' });
        }
    }, {
        key: 'render',
        value: function render() {
            return React.createElement(
                _LocaleReceiver2['default'],
                { componentName: 'Select', defaultLocale: _default2['default'].Select },
                this.renderSelect
            );
        }
    }]);
    return Select;
}(React.Component);

exports['default'] = Select;

Select.Option = _rcSelect.Option;
Select.OptGroup = _rcSelect.OptGroup;
Select.SECRET_COMBOBOX_MODE_DO_NOT_USE = 'SECRET_COMBOBOX_MODE_DO_NOT_USE';
Select.defaultProps = {
    prefixCls: 'ant-select',
    showSearch: false,
    transitionName: 'slide-up',
    choiceTransitionName: 'zoom'
};
Select.propTypes = SelectPropTypes;
module.exports = exports['default'];
import _extends from 'babel-runtime/helpers/extends';
import _defineProperty from 'babel-runtime/helpers/defineProperty';
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
import * as PropTypes from 'prop-types';
import RcSelect, { Option, OptGroup } from 'rc-select';
import classNames from 'classnames';
import LocaleReceiver from '../locale-provider/LocaleReceiver';
import defaultLocale from '../locale-provider/default';
import { ConfigConsumer } from '../config-provider';
import omit from 'omit.js';
import warning from 'warning';
import Icon from '../icon';
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
    _inherits(Select, _React$Component);

    function Select(props) {
        _classCallCheck(this, Select);

        var _this = _possibleConstructorReturn(this, (Select.__proto__ || Object.getPrototypeOf(Select)).call(this, props));

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
            var rest = omit(restProps, ['inputIcon']);
            var cls = classNames((_classNames = {}, _defineProperty(_classNames, prefixCls + '-lg', size === 'large'), _defineProperty(_classNames, prefixCls + '-sm', size === 'small'), _classNames), className);
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
                className: classNames(removeIcon.props.className, prefixCls + '-remove-icon')
            }) : removeIcon) || React.createElement(Icon, { type: 'close', className: prefixCls + '-remove-icon' });
            var finalClearIcon = clearIcon && (React.isValidElement(clearIcon) ? React.cloneElement(clearIcon, {
                className: classNames(clearIcon.props.className, prefixCls + '-clear-icon')
            }) : clearIcon) || React.createElement(Icon, { type: 'close-circle', theme: 'filled', className: prefixCls + '-clear-icon' });
            var finalMenuItemSelectedIcon = menuItemSelectedIcon && (React.isValidElement(menuItemSelectedIcon) ? React.cloneElement(menuItemSelectedIcon, {
                className: classNames(menuItemSelectedIcon.props.className, prefixCls + '-selected-icon')
            }) : menuItemSelectedIcon) || React.createElement(Icon, { type: 'check', className: prefixCls + '-selected-icon' });
            return React.createElement(
                ConfigConsumer,
                null,
                function (_ref) {
                    var getContextPopupContainer = _ref.getPopupContainer;

                    return React.createElement(RcSelect, _extends({ inputIcon: _this.renderSuffixIcon(), removeIcon: finalRemoveIcon, clearIcon: finalClearIcon, menuItemSelectedIcon: finalMenuItemSelectedIcon }, rest, modeConfig, { prefixCls: prefixCls, className: cls, optionLabelProp: optionLabelProp || 'children', notFoundContent: _this.getNotFoundContent(locale), getPopupContainer: getPopupContainer || getContextPopupContainer, ref: _this.saveSelect }));
                }
            );
        };
        warning(props.mode !== 'combobox', 'The combobox mode of Select is deprecated,' + 'it will be removed in next major version,' + 'please use AutoComplete instead');
        return _this;
    }

    _createClass(Select, [{
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
                    className: classNames(suffixIcon.props.className, prefixCls + '-arrow-icon')
                }) : suffixIcon;
            }
            if (loading) {
                return React.createElement(Icon, { type: 'loading' });
            }
            return React.createElement(Icon, { type: 'down', className: prefixCls + '-arrow-icon' });
        }
    }, {
        key: 'render',
        value: function render() {
            return React.createElement(
                LocaleReceiver,
                { componentName: 'Select', defaultLocale: defaultLocale.Select },
                this.renderSelect
            );
        }
    }]);

    return Select;
}(React.Component);

export default Select;

Select.Option = Option;
Select.OptGroup = OptGroup;
Select.SECRET_COMBOBOX_MODE_DO_NOT_USE = 'SECRET_COMBOBOX_MODE_DO_NOT_USE';
Select.defaultProps = {
    prefixCls: 'ant-select',
    showSearch: false,
    transitionName: 'slide-up',
    choiceTransitionName: 'zoom'
};
Select.propTypes = SelectPropTypes;
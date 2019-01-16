import _extends from 'babel-runtime/helpers/extends';
import _defineProperty from 'babel-runtime/helpers/defineProperty';
import _classCallCheck from 'babel-runtime/helpers/classCallCheck';
import _createClass from 'babel-runtime/helpers/createClass';
import _possibleConstructorReturn from 'babel-runtime/helpers/possibleConstructorReturn';
import _inherits from 'babel-runtime/helpers/inherits';
import * as React from 'react';
import * as moment from 'moment';
import { polyfill } from 'react-lifecycles-compat';
import Calendar from 'rc-calendar';
import RcDatePicker from 'rc-calendar/es/Picker';
import classNames from 'classnames';
import Icon from '../icon';
import interopDefault from '../_util/interopDefault';
function formatValue(value, format) {
    return value && value.format(format) || '';
}

var WeekPicker = function (_React$Component) {
    _inherits(WeekPicker, _React$Component);

    function WeekPicker(props) {
        _classCallCheck(this, WeekPicker);

        var _this = _possibleConstructorReturn(this, (WeekPicker.__proto__ || Object.getPrototypeOf(WeekPicker)).call(this, props));

        _this.weekDateRender = function (current) {
            var selectedValue = _this.state.value;
            var prefixCls = _this.props.prefixCls;

            if (selectedValue && current.year() === selectedValue.year() && current.week() === selectedValue.week()) {
                return React.createElement(
                    'div',
                    { className: prefixCls + '-selected-day' },
                    React.createElement(
                        'div',
                        { className: prefixCls + '-date' },
                        current.date()
                    )
                );
            }
            return React.createElement(
                'div',
                { className: prefixCls + '-date' },
                current.date()
            );
        };
        _this.handleChange = function (value) {
            if (!('value' in _this.props)) {
                _this.setState({ value: value });
            }
            _this.props.onChange(value, formatValue(value, _this.props.format));
        };
        _this.handleOpenChange = function (open) {
            var onOpenChange = _this.props.onOpenChange;

            if (!('open' in _this.props)) {
                _this.setState({ open: open });
            }
            if (onOpenChange) {
                onOpenChange(open);
            }
            if (!open) {
                _this.focus();
            }
        };
        _this.clearSelection = function (e) {
            e.preventDefault();
            e.stopPropagation();
            _this.handleChange(null);
        };
        _this.saveInput = function (node) {
            _this.input = node;
        };
        var value = props.value || props.defaultValue;
        if (value && !interopDefault(moment).isMoment(value)) {
            throw new Error('The value/defaultValue of DatePicker or MonthPicker must be ' + 'a moment object after `antd@2.0`, see: https://u.ant.design/date-picker-value');
        }
        _this.state = {
            value: value,
            open: props.open
        };
        return _this;
    }

    _createClass(WeekPicker, [{
        key: 'focus',
        value: function focus() {
            this.input.focus();
        }
    }, {
        key: 'blur',
        value: function blur() {
            this.input.blur();
        }
    }, {
        key: 'render',
        value: function render() {
            var _classNames,
                _this2 = this;

            var _props = this.props,
                prefixCls = _props.prefixCls,
                className = _props.className,
                disabled = _props.disabled,
                pickerClass = _props.pickerClass,
                popupStyle = _props.popupStyle,
                pickerInputClass = _props.pickerInputClass,
                format = _props.format,
                allowClear = _props.allowClear,
                locale = _props.locale,
                localeCode = _props.localeCode,
                disabledDate = _props.disabledDate,
                style = _props.style,
                onFocus = _props.onFocus,
                onBlur = _props.onBlur,
                id = _props.id,
                suffixIcon = _props.suffixIcon;
            var open = this.state.open;

            var pickerValue = this.state.value;
            if (pickerValue && localeCode) {
                pickerValue.locale(localeCode);
            }
            var placeholder = 'placeholder' in this.props ? this.props.placeholder : locale.lang.placeholder;
            var calendar = React.createElement(Calendar, { showWeekNumber: true, dateRender: this.weekDateRender, prefixCls: prefixCls, format: format, locale: locale.lang, showDateInput: false, showToday: false, disabledDate: disabledDate });
            var clearIcon = !disabled && allowClear && this.state.value ? React.createElement(Icon, { type: 'close-circle', className: prefixCls + '-picker-clear', onClick: this.clearSelection, theme: 'filled' }) : null;
            var inputIcon = suffixIcon && (React.isValidElement(suffixIcon) ? React.cloneElement(suffixIcon, {
                className: classNames((_classNames = {}, _defineProperty(_classNames, suffixIcon.props.className, suffixIcon.props.className), _defineProperty(_classNames, prefixCls + '-picker-icon', true), _classNames))
            }) : React.createElement(
                'span',
                { className: prefixCls + '-picker-icon' },
                suffixIcon
            )) || React.createElement(Icon, { type: 'calendar', className: prefixCls + '-picker-icon' });
            var input = function input(_ref) {
                var value = _ref.value;

                return React.createElement(
                    'span',
                    { style: { display: 'inline-block' } },
                    React.createElement('input', { ref: _this2.saveInput, disabled: disabled, readOnly: true, value: value && value.format(format) || '', placeholder: placeholder, className: pickerInputClass, onFocus: onFocus, onBlur: onBlur }),
                    clearIcon,
                    inputIcon
                );
            };
            return React.createElement(
                'span',
                { className: classNames(className, pickerClass), style: style, id: id },
                React.createElement(
                    RcDatePicker,
                    _extends({}, this.props, { calendar: calendar, prefixCls: prefixCls + '-picker-container', value: pickerValue, onChange: this.handleChange, open: open, onOpenChange: this.handleOpenChange, style: popupStyle }),
                    input
                )
            );
        }
    }], [{
        key: 'getDerivedStateFromProps',
        value: function getDerivedStateFromProps(nextProps) {
            if ('value' in nextProps || 'open' in nextProps) {
                var state = {};
                if ('value' in nextProps) {
                    state.value = nextProps.value;
                }
                if ('open' in nextProps) {
                    state.open = nextProps.open;
                }
                return state;
            }
            return null;
        }
    }]);

    return WeekPicker;
}(React.Component);

WeekPicker.defaultProps = {
    format: 'gggg-wo',
    allowClear: true
};
polyfill(WeekPicker);
export default WeekPicker;
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

var _moment = require('moment');

var moment = _interopRequireWildcard(_moment);

var _reactLifecyclesCompat = require('react-lifecycles-compat');

var _rcCalendar = require('rc-calendar');

var _rcCalendar2 = _interopRequireDefault(_rcCalendar);

var _Picker = require('rc-calendar/lib/Picker');

var _Picker2 = _interopRequireDefault(_Picker);

var _classnames = require('classnames');

var _classnames2 = _interopRequireDefault(_classnames);

var _icon = require('../icon');

var _icon2 = _interopRequireDefault(_icon);

var _interopDefault = require('../_util/interopDefault');

var _interopDefault2 = _interopRequireDefault(_interopDefault);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj['default'] = obj; return newObj; } }

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { 'default': obj }; }

function formatValue(value, format) {
    return value && value.format(format) || '';
}

var WeekPicker = function (_React$Component) {
    (0, _inherits3['default'])(WeekPicker, _React$Component);

    function WeekPicker(props) {
        (0, _classCallCheck3['default'])(this, WeekPicker);

        var _this = (0, _possibleConstructorReturn3['default'])(this, (WeekPicker.__proto__ || Object.getPrototypeOf(WeekPicker)).call(this, props));

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
        if (value && !(0, _interopDefault2['default'])(moment).isMoment(value)) {
            throw new Error('The value/defaultValue of DatePicker or MonthPicker must be ' + 'a moment object after `antd@2.0`, see: https://u.ant.design/date-picker-value');
        }
        _this.state = {
            value: value,
            open: props.open
        };
        return _this;
    }

    (0, _createClass3['default'])(WeekPicker, [{
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
            var calendar = React.createElement(_rcCalendar2['default'], { showWeekNumber: true, dateRender: this.weekDateRender, prefixCls: prefixCls, format: format, locale: locale.lang, showDateInput: false, showToday: false, disabledDate: disabledDate });
            var clearIcon = !disabled && allowClear && this.state.value ? React.createElement(_icon2['default'], { type: 'close-circle', className: prefixCls + '-picker-clear', onClick: this.clearSelection, theme: 'filled' }) : null;
            var inputIcon = suffixIcon && (React.isValidElement(suffixIcon) ? React.cloneElement(suffixIcon, {
                className: (0, _classnames2['default'])((_classNames = {}, (0, _defineProperty3['default'])(_classNames, suffixIcon.props.className, suffixIcon.props.className), (0, _defineProperty3['default'])(_classNames, prefixCls + '-picker-icon', true), _classNames))
            }) : React.createElement(
                'span',
                { className: prefixCls + '-picker-icon' },
                suffixIcon
            )) || React.createElement(_icon2['default'], { type: 'calendar', className: prefixCls + '-picker-icon' });
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
                { className: (0, _classnames2['default'])(className, pickerClass), style: style, id: id },
                React.createElement(
                    _Picker2['default'],
                    (0, _extends3['default'])({}, this.props, { calendar: calendar, prefixCls: prefixCls + '-picker-container', value: pickerValue, onChange: this.handleChange, open: open, onOpenChange: this.handleOpenChange, style: popupStyle }),
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
(0, _reactLifecyclesCompat.polyfill)(WeekPicker);
exports['default'] = WeekPicker;
module.exports = exports['default'];
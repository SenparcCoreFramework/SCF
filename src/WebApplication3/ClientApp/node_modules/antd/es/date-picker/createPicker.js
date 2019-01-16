import _extends from 'babel-runtime/helpers/extends';
import _defineProperty from 'babel-runtime/helpers/defineProperty';
import _classCallCheck from 'babel-runtime/helpers/classCallCheck';
import _createClass from 'babel-runtime/helpers/createClass';
import _possibleConstructorReturn from 'babel-runtime/helpers/possibleConstructorReturn';
import _inherits from 'babel-runtime/helpers/inherits';
import * as React from 'react';
import * as moment from 'moment';
import { polyfill } from 'react-lifecycles-compat';
import MonthCalendar from 'rc-calendar/es/MonthCalendar';
import RcDatePicker from 'rc-calendar/es/Picker';
import classNames from 'classnames';
import omit from 'omit.js';
import Icon from '../icon';
import warning from '../_util/warning';
import interopDefault from '../_util/interopDefault';
import getDataOrAriaProps from '../_util/getDataOrAriaProps';
export default function createPicker(TheCalendar) {
    var CalenderWrapper = function (_React$Component) {
        _inherits(CalenderWrapper, _React$Component);

        function CalenderWrapper(props) {
            _classCallCheck(this, CalenderWrapper);

            var _this = _possibleConstructorReturn(this, (CalenderWrapper.__proto__ || Object.getPrototypeOf(CalenderWrapper)).call(this, props));

            _this.renderFooter = function () {
                var _this$props = _this.props,
                    prefixCls = _this$props.prefixCls,
                    renderExtraFooter = _this$props.renderExtraFooter;

                return renderExtraFooter ? React.createElement(
                    'div',
                    { className: prefixCls + '-footer-extra' },
                    renderExtraFooter.apply(undefined, arguments)
                ) : null;
            };
            _this.clearSelection = function (e) {
                e.preventDefault();
                e.stopPropagation();
                _this.handleChange(null);
            };
            _this.handleChange = function (value) {
                var props = _this.props;
                if (!('value' in props)) {
                    _this.setState({
                        value: value,
                        showDate: value
                    });
                }
                props.onChange(value, value && value.format(props.format) || '');
            };
            _this.handleCalendarChange = function (value) {
                _this.setState({ showDate: value });
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
            _this.saveInput = function (node) {
                _this.input = node;
            };
            var value = props.value || props.defaultValue;
            if (value && !interopDefault(moment).isMoment(value)) {
                throw new Error('The value/defaultValue of DatePicker or MonthPicker must be ' + 'a moment object after `antd@2.0`, see: https://u.ant.design/date-picker-value');
            }
            _this.state = {
                value: value,
                showDate: value,
                open: false
            };
            return _this;
        }

        _createClass(CalenderWrapper, [{
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
                    _classNames2,
                    _this2 = this;

                var _state = this.state,
                    value = _state.value,
                    showDate = _state.showDate,
                    open = _state.open;

                var props = omit(this.props, ['onChange']);
                var prefixCls = props.prefixCls,
                    locale = props.locale,
                    localeCode = props.localeCode,
                    suffixIcon = props.suffixIcon;

                var placeholder = 'placeholder' in props ? props.placeholder : locale.lang.placeholder;
                var disabledTime = props.showTime ? props.disabledTime : null;
                var calendarClassName = classNames((_classNames = {}, _defineProperty(_classNames, prefixCls + '-time', props.showTime), _defineProperty(_classNames, prefixCls + '-month', MonthCalendar === TheCalendar), _classNames));
                if (value && localeCode) {
                    value.locale(localeCode);
                }
                var pickerProps = {};
                var calendarProps = {};
                var pickerStyle = {};
                if (props.showTime) {
                    calendarProps = {
                        // fix https://github.com/ant-design/ant-design/issues/1902
                        onSelect: this.handleChange
                    };
                    pickerStyle.width = 195;
                } else {
                    pickerProps = {
                        onChange: this.handleChange
                    };
                }
                if ('mode' in props) {
                    calendarProps.mode = props.mode;
                }
                warning(!('onOK' in props), 'It should be `DatePicker[onOk]` or `MonthPicker[onOk]`, instead of `onOK`!');
                var calendar = React.createElement(TheCalendar, _extends({}, calendarProps, { disabledDate: props.disabledDate, disabledTime: disabledTime, locale: locale.lang, timePicker: props.timePicker, defaultValue: props.defaultPickerValue || interopDefault(moment)(), dateInputPlaceholder: placeholder, prefixCls: prefixCls, className: calendarClassName, onOk: props.onOk, dateRender: props.dateRender, format: props.format, showToday: props.showToday, monthCellContentRender: props.monthCellContentRender, renderFooter: this.renderFooter, onPanelChange: props.onPanelChange, onChange: this.handleCalendarChange, value: showDate }));
                var clearIcon = !props.disabled && props.allowClear && value ? React.createElement(Icon, { type: 'close-circle', className: prefixCls + '-picker-clear', onClick: this.clearSelection, theme: 'filled' }) : null;
                var inputIcon = suffixIcon && (React.isValidElement(suffixIcon) ? React.cloneElement(suffixIcon, {
                    className: classNames((_classNames2 = {}, _defineProperty(_classNames2, suffixIcon.props.className, suffixIcon.props.className), _defineProperty(_classNames2, prefixCls + '-picker-icon', true), _classNames2))
                }) : React.createElement(
                    'span',
                    { className: prefixCls + '-picker-icon' },
                    suffixIcon
                )) || React.createElement(Icon, { type: 'calendar', className: prefixCls + '-picker-icon' });
                var dataOrAriaProps = getDataOrAriaProps(props);
                var input = function input(_ref) {
                    var inputValue = _ref.value;
                    return React.createElement(
                        'div',
                        null,
                        React.createElement('input', _extends({ ref: _this2.saveInput, disabled: props.disabled, readOnly: true, value: inputValue && inputValue.format(props.format) || '', placeholder: placeholder, className: props.pickerInputClass, tabIndex: props.tabIndex }, dataOrAriaProps)),
                        clearIcon,
                        inputIcon
                    );
                };
                return React.createElement(
                    'span',
                    { id: props.id, className: classNames(props.className, props.pickerClass), style: _extends({}, pickerStyle, props.style), onFocus: props.onFocus, onBlur: props.onBlur, onMouseEnter: props.onMouseEnter, onMouseLeave: props.onMouseLeave },
                    React.createElement(
                        RcDatePicker,
                        _extends({}, props, pickerProps, { calendar: calendar, value: value, prefixCls: prefixCls + '-picker-container', style: props.popupStyle, open: open, onOpenChange: this.handleOpenChange }),
                        input
                    )
                );
            }
        }], [{
            key: 'getDerivedStateFromProps',
            value: function getDerivedStateFromProps(nextProps, prevState) {
                var state = {};
                var open = prevState.open;
                if ('open' in nextProps) {
                    state.open = nextProps.open;
                    open = nextProps.open || false;
                }
                if ('value' in nextProps) {
                    state.value = nextProps.value;
                    if (nextProps.value !== prevState.value || !open && nextProps.value !== prevState.showDate) {
                        state.showDate = nextProps.value;
                    }
                }
                return Object.keys(state).length > 0 ? state : null;
            }
        }]);

        return CalenderWrapper;
    }(React.Component);

    CalenderWrapper.defaultProps = {
        prefixCls: 'ant-calendar',
        allowClear: true,
        showToday: true
    };
    polyfill(CalenderWrapper);
    return CalenderWrapper;
}
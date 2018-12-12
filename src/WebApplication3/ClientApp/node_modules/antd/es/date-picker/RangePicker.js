import _extends from 'babel-runtime/helpers/extends';
import _defineProperty from 'babel-runtime/helpers/defineProperty';
import _classCallCheck from 'babel-runtime/helpers/classCallCheck';
import _createClass from 'babel-runtime/helpers/createClass';
import _possibleConstructorReturn from 'babel-runtime/helpers/possibleConstructorReturn';
import _inherits from 'babel-runtime/helpers/inherits';
import _slicedToArray from 'babel-runtime/helpers/slicedToArray';
/* tslint:disable jsx-no-multiline-js */
import * as React from 'react';
import * as moment from 'moment';
import { polyfill } from 'react-lifecycles-compat';
import RangeCalendar from 'rc-calendar/es/RangeCalendar';
import RcDatePicker from 'rc-calendar/es/Picker';
import classNames from 'classnames';
import shallowequal from 'shallowequal';
import Icon from '../icon';
import Tag from '../tag';
import warning from '../_util/warning';
import interopDefault from '../_util/interopDefault';
function getShowDateFromValue(value) {
    var _value = _slicedToArray(value, 2),
        start = _value[0],
        end = _value[1];
    // value could be an empty array, then we should not reset showDate


    if (!start && !end) {
        return;
    }
    var newEnd = end && end.isSame(start, 'month') ? end.clone().add(1, 'month') : end;
    return [start, newEnd];
}
function formatValue(value, format) {
    return value && value.format(format) || '';
}
function pickerValueAdapter(value) {
    if (!value) {
        return;
    }
    if (Array.isArray(value)) {
        return value;
    }
    return [value, value.clone().add(1, 'month')];
}
function isEmptyArray(arr) {
    if (Array.isArray(arr)) {
        return arr.length === 0 || arr.every(function (i) {
            return !i;
        });
    }
    return false;
}
function fixLocale(value, localeCode) {
    if (!localeCode) {
        return;
    }
    if (!value || value.length === 0) {
        return;
    }

    var _value2 = _slicedToArray(value, 2),
        start = _value2[0],
        end = _value2[1];

    if (start) {
        start.locale(localeCode);
    }
    if (end) {
        end.locale(localeCode);
    }
}

var RangePicker = function (_React$Component) {
    _inherits(RangePicker, _React$Component);

    function RangePicker(props) {
        _classCallCheck(this, RangePicker);

        var _this = _possibleConstructorReturn(this, (RangePicker.__proto__ || Object.getPrototypeOf(RangePicker)).call(this, props));

        _this.clearSelection = function (e) {
            e.preventDefault();
            e.stopPropagation();
            _this.setState({ value: [] });
            _this.handleChange([]);
        };
        _this.clearHoverValue = function () {
            return _this.setState({ hoverValue: [] });
        };
        _this.handleChange = function (value) {
            var props = _this.props;
            if (!('value' in props)) {
                _this.setState(function (_ref) {
                    var showDate = _ref.showDate;
                    return {
                        value: value,
                        showDate: getShowDateFromValue(value) || showDate
                    };
                });
            }

            var _value3 = _slicedToArray(value, 2),
                start = _value3[0],
                end = _value3[1];

            props.onChange(value, [formatValue(start, props.format), formatValue(end, props.format)]);
        };
        _this.handleOpenChange = function (open) {
            if (!('open' in _this.props)) {
                _this.setState({ open: open });
            }
            if (open === false) {
                _this.clearHoverValue();
            }
            var onOpenChange = _this.props.onOpenChange;

            if (onOpenChange) {
                onOpenChange(open);
            }
            if (!open) {
                _this.focus();
            }
        };
        _this.handleShowDateChange = function (showDate) {
            return _this.setState({ showDate: showDate });
        };
        _this.handleHoverChange = function (hoverValue) {
            return _this.setState({ hoverValue: hoverValue });
        };
        _this.handleRangeMouseLeave = function () {
            if (_this.state.open) {
                _this.clearHoverValue();
            }
        };
        _this.handleCalendarInputSelect = function (value) {
            var _value4 = _slicedToArray(value, 1),
                start = _value4[0];

            if (!start) {
                return;
            }
            _this.setState(function (_ref2) {
                var showDate = _ref2.showDate;
                return {
                    value: value,
                    showDate: getShowDateFromValue(value) || showDate
                };
            });
        };
        _this.handleRangeClick = function (value) {
            if (typeof value === 'function') {
                value = value();
            }
            _this.setValue(value, true);
            var _this$props = _this.props,
                onOk = _this$props.onOk,
                onOpenChange = _this$props.onOpenChange;

            if (onOk) {
                onOk(value);
            }
            if (onOpenChange) {
                onOpenChange(false);
            }
        };
        _this.savePicker = function (node) {
            _this.picker = node;
        };
        _this.renderFooter = function () {
            var _this$props2 = _this.props,
                prefixCls = _this$props2.prefixCls,
                ranges = _this$props2.ranges,
                renderExtraFooter = _this$props2.renderExtraFooter,
                tagPrefixCls = _this$props2.tagPrefixCls;

            if (!ranges && !renderExtraFooter) {
                return null;
            }
            var customFooter = renderExtraFooter ? React.createElement(
                'div',
                { className: prefixCls + '-footer-extra', key: 'extra' },
                renderExtraFooter.apply(undefined, arguments)
            ) : null;
            var operations = Object.keys(ranges || {}).map(function (range) {
                var value = ranges[range];
                return React.createElement(
                    Tag,
                    { key: range, prefixCls: tagPrefixCls, color: 'blue', onClick: function onClick() {
                            return _this.handleRangeClick(value);
                        }, onMouseEnter: function onMouseEnter() {
                            return _this.setState({ hoverValue: value });
                        }, onMouseLeave: _this.handleRangeMouseLeave },
                    range
                );
            });
            var rangeNode = operations && operations.length > 0 ? React.createElement(
                'div',
                { className: prefixCls + '-footer-extra ' + prefixCls + '-range-quick-selector', key: 'range' },
                operations
            ) : null;
            return [rangeNode, customFooter];
        };
        var value = props.value || props.defaultValue || [];

        var _value5 = _slicedToArray(value, 2),
            start = _value5[0],
            end = _value5[1];

        if (start && !interopDefault(moment).isMoment(start) || end && !interopDefault(moment).isMoment(end)) {
            throw new Error('The value/defaultValue of RangePicker must be a moment object array after `antd@2.0`, ' + 'see: https://u.ant.design/date-picker-value');
        }
        var pickerValue = !value || isEmptyArray(value) ? props.defaultPickerValue : value;
        _this.state = {
            value: value,
            showDate: pickerValueAdapter(pickerValue || interopDefault(moment)()),
            open: props.open,
            hoverValue: []
        };
        return _this;
    }

    _createClass(RangePicker, [{
        key: 'setValue',
        value: function setValue(value, hidePanel) {
            this.handleChange(value);
            if ((hidePanel || !this.props.showTime) && !('open' in this.props)) {
                this.setState({ open: false });
            }
        }
    }, {
        key: 'focus',
        value: function focus() {
            this.picker.focus();
        }
    }, {
        key: 'blur',
        value: function blur() {
            this.picker.blur();
        }
    }, {
        key: 'render',
        value: function render() {
            var _classNames,
                _this2 = this,
                _classNames2;

            var state = this.state,
                props = this.props;
            var value = state.value,
                showDate = state.showDate,
                hoverValue = state.hoverValue,
                open = state.open;
            var prefixCls = props.prefixCls,
                popupStyle = props.popupStyle,
                style = props.style,
                disabledDate = props.disabledDate,
                disabledTime = props.disabledTime,
                showTime = props.showTime,
                showToday = props.showToday,
                ranges = props.ranges,
                onOk = props.onOk,
                locale = props.locale,
                localeCode = props.localeCode,
                format = props.format,
                dateRender = props.dateRender,
                onCalendarChange = props.onCalendarChange,
                suffixIcon = props.suffixIcon;

            fixLocale(value, localeCode);
            fixLocale(showDate, localeCode);
            warning(!('onOK' in props), 'It should be `RangePicker[onOk]`, instead of `onOK`!');
            var calendarClassName = classNames((_classNames = {}, _defineProperty(_classNames, prefixCls + '-time', showTime), _defineProperty(_classNames, prefixCls + '-range-with-ranges', ranges), _classNames));
            // 需要选择时间时，点击 ok 时才触发 onChange
            var pickerChangeHandler = {
                onChange: this.handleChange
            };
            var calendarProps = {
                onOk: this.handleChange
            };
            if (props.timePicker) {
                pickerChangeHandler.onChange = function (changedValue) {
                    return _this2.handleChange(changedValue);
                };
            } else {
                calendarProps = {};
            }
            if ('mode' in props) {
                calendarProps.mode = props.mode;
            }
            var startPlaceholder = 'placeholder' in props ? props.placeholder[0] : locale.lang.rangePlaceholder[0];
            var endPlaceholder = 'placeholder' in props ? props.placeholder[1] : locale.lang.rangePlaceholder[1];
            var calendar = React.createElement(RangeCalendar, _extends({}, calendarProps, { onChange: onCalendarChange, format: format, prefixCls: prefixCls, className: calendarClassName, renderFooter: this.renderFooter, timePicker: props.timePicker, disabledDate: disabledDate, disabledTime: disabledTime, dateInputPlaceholder: [startPlaceholder, endPlaceholder], locale: locale.lang, onOk: onOk, dateRender: dateRender, value: showDate, onValueChange: this.handleShowDateChange, hoverValue: hoverValue, onHoverChange: this.handleHoverChange, onPanelChange: props.onPanelChange, showToday: showToday, onInputSelect: this.handleCalendarInputSelect }));
            // default width for showTime
            var pickerStyle = {};
            if (props.showTime) {
                pickerStyle.width = style && style.width || 350;
            }

            var _value6 = _slicedToArray(value, 2),
                startValue = _value6[0],
                endValue = _value6[1];

            var clearIcon = !props.disabled && props.allowClear && value && (startValue || endValue) ? React.createElement(Icon, { type: 'close-circle', className: prefixCls + '-picker-clear', onClick: this.clearSelection, theme: 'filled' }) : null;
            var inputIcon = suffixIcon && (React.isValidElement(suffixIcon) ? React.cloneElement(suffixIcon, {
                className: classNames((_classNames2 = {}, _defineProperty(_classNames2, suffixIcon.props.className, suffixIcon.props.className), _defineProperty(_classNames2, prefixCls + '-picker-icon', true), _classNames2))
            }) : React.createElement(
                'span',
                { className: prefixCls + '-picker-icon' },
                suffixIcon
            )) || React.createElement(Icon, { type: 'calendar', className: prefixCls + '-picker-icon' });
            var input = function input(_ref3) {
                var inputValue = _ref3.value;

                var _inputValue = _slicedToArray(inputValue, 2),
                    start = _inputValue[0],
                    end = _inputValue[1];

                return React.createElement(
                    'span',
                    { className: props.pickerInputClass },
                    React.createElement('input', { disabled: props.disabled, readOnly: true, value: start && start.format(props.format) || '', placeholder: startPlaceholder, className: prefixCls + '-range-picker-input', tabIndex: -1 }),
                    React.createElement(
                        'span',
                        { className: prefixCls + '-range-picker-separator' },
                        ' ~ '
                    ),
                    React.createElement('input', { disabled: props.disabled, readOnly: true, value: end && end.format(props.format) || '', placeholder: endPlaceholder, className: prefixCls + '-range-picker-input', tabIndex: -1 }),
                    clearIcon,
                    inputIcon
                );
            };
            return React.createElement(
                'span',
                { ref: this.savePicker, id: props.id, className: classNames(props.className, props.pickerClass), style: _extends({}, style, pickerStyle), tabIndex: props.disabled ? -1 : 0, onFocus: props.onFocus, onBlur: props.onBlur, onMouseEnter: props.onMouseEnter, onMouseLeave: props.onMouseLeave },
                React.createElement(
                    RcDatePicker,
                    _extends({}, props, pickerChangeHandler, { calendar: calendar, value: value, open: open, onOpenChange: this.handleOpenChange, prefixCls: prefixCls + '-picker-container', style: popupStyle }),
                    input
                )
            );
        }
    }], [{
        key: 'getDerivedStateFromProps',
        value: function getDerivedStateFromProps(nextProps, prevState) {
            var state = null;
            if ('value' in nextProps) {
                var value = nextProps.value || [];
                state = {
                    value: value
                };
                if (!shallowequal(nextProps.value, prevState.value)) {
                    state = _extends({}, state, { showDate: getShowDateFromValue(value) || prevState.showDate });
                }
            }
            if ('open' in nextProps && prevState.open !== nextProps.open) {
                state = _extends({}, state, { open: nextProps.open });
            }
            return state;
        }
    }]);

    return RangePicker;
}(React.Component);

RangePicker.defaultProps = {
    prefixCls: 'ant-calendar',
    tagPrefixCls: 'ant-tag',
    allowClear: true,
    showToday: false
};
polyfill(RangePicker);
export default RangePicker;
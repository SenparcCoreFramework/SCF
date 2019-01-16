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
import RcPagination from 'rc-pagination';
import classNames from 'classnames';
import LocaleReceiver from '../locale-provider/LocaleReceiver';
import Select from '../select';
import MiniSelect from './MiniSelect';
import Icon from '../icon';

var Pagination = function (_React$Component) {
    _inherits(Pagination, _React$Component);

    function Pagination() {
        _classCallCheck(this, Pagination);

        var _this = _possibleConstructorReturn(this, (Pagination.__proto__ || Object.getPrototypeOf(Pagination)).apply(this, arguments));

        _this.getIconsProps = function () {
            var prefixCls = _this.props.prefixCls;

            var prevIcon = React.createElement(
                'a',
                { className: prefixCls + '-item-link' },
                React.createElement(Icon, { type: 'left' })
            );
            var nextIcon = React.createElement(
                'a',
                { className: prefixCls + '-item-link' },
                React.createElement(Icon, { type: 'right' })
            );
            var jumpPrevIcon = React.createElement(
                'a',
                { className: prefixCls + '-item-link' },
                React.createElement(
                    'div',
                    { className: prefixCls + '-item-container' },
                    React.createElement(Icon, { className: prefixCls + '-item-link-icon', type: 'double-left' }),
                    React.createElement(
                        'span',
                        { className: prefixCls + '-item-ellipsis' },
                        '\u2022\u2022\u2022'
                    )
                )
            );
            var jumpNextIcon = React.createElement(
                'a',
                { className: prefixCls + '-item-link' },
                React.createElement(
                    'div',
                    { className: prefixCls + '-item-container' },
                    React.createElement(Icon, { className: prefixCls + '-item-link-icon', type: 'double-right' }),
                    React.createElement(
                        'span',
                        { className: prefixCls + '-item-ellipsis' },
                        '\u2022\u2022\u2022'
                    )
                )
            );
            return {
                prevIcon: prevIcon,
                nextIcon: nextIcon,
                jumpPrevIcon: jumpPrevIcon,
                jumpNextIcon: jumpNextIcon
            };
        };
        _this.renderPagination = function (contextLocale) {
            var _a = _this.props,
                className = _a.className,
                size = _a.size,
                customLocale = _a.locale,
                restProps = __rest(_a, ["className", "size", "locale"]);
            var locale = _extends({}, contextLocale, customLocale);
            var isSmall = size === 'small';
            return React.createElement(RcPagination, _extends({}, restProps, _this.getIconsProps(), { className: classNames(className, { mini: isSmall }), selectComponentClass: isSmall ? MiniSelect : Select, locale: locale }));
        };
        return _this;
    }

    _createClass(Pagination, [{
        key: 'render',
        value: function render() {
            return React.createElement(
                LocaleReceiver,
                { componentName: 'Pagination' },
                this.renderPagination
            );
        }
    }]);

    return Pagination;
}(React.Component);

export default Pagination;

Pagination.defaultProps = {
    prefixCls: 'ant-pagination',
    selectPrefixCls: 'ant-select'
};
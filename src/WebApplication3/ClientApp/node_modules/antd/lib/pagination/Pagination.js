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

var _rcPagination = require('rc-pagination');

var _rcPagination2 = _interopRequireDefault(_rcPagination);

var _classnames = require('classnames');

var _classnames2 = _interopRequireDefault(_classnames);

var _LocaleReceiver = require('../locale-provider/LocaleReceiver');

var _LocaleReceiver2 = _interopRequireDefault(_LocaleReceiver);

var _select = require('../select');

var _select2 = _interopRequireDefault(_select);

var _MiniSelect = require('./MiniSelect');

var _MiniSelect2 = _interopRequireDefault(_MiniSelect);

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

var Pagination = function (_React$Component) {
    (0, _inherits3['default'])(Pagination, _React$Component);

    function Pagination() {
        (0, _classCallCheck3['default'])(this, Pagination);

        var _this = (0, _possibleConstructorReturn3['default'])(this, (Pagination.__proto__ || Object.getPrototypeOf(Pagination)).apply(this, arguments));

        _this.getIconsProps = function () {
            var prefixCls = _this.props.prefixCls;

            var prevIcon = React.createElement(
                'a',
                { className: prefixCls + '-item-link' },
                React.createElement(_icon2['default'], { type: 'left' })
            );
            var nextIcon = React.createElement(
                'a',
                { className: prefixCls + '-item-link' },
                React.createElement(_icon2['default'], { type: 'right' })
            );
            var jumpPrevIcon = React.createElement(
                'a',
                { className: prefixCls + '-item-link' },
                React.createElement(
                    'div',
                    { className: prefixCls + '-item-container' },
                    React.createElement(_icon2['default'], { className: prefixCls + '-item-link-icon', type: 'double-left' }),
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
                    React.createElement(_icon2['default'], { className: prefixCls + '-item-link-icon', type: 'double-right' }),
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
            var locale = (0, _extends3['default'])({}, contextLocale, customLocale);
            var isSmall = size === 'small';
            return React.createElement(_rcPagination2['default'], (0, _extends3['default'])({}, restProps, _this.getIconsProps(), { className: (0, _classnames2['default'])(className, { mini: isSmall }), selectComponentClass: isSmall ? _MiniSelect2['default'] : _select2['default'], locale: locale }));
        };
        return _this;
    }

    (0, _createClass3['default'])(Pagination, [{
        key: 'render',
        value: function render() {
            return React.createElement(
                _LocaleReceiver2['default'],
                { componentName: 'Pagination' },
                this.renderPagination
            );
        }
    }]);
    return Pagination;
}(React.Component);

exports['default'] = Pagination;

Pagination.defaultProps = {
    prefixCls: 'ant-pagination',
    selectPrefixCls: 'ant-select'
};
module.exports = exports['default'];
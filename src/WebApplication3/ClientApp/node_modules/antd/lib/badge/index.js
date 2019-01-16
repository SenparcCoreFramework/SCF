'use strict';

Object.defineProperty(exports, "__esModule", {
    value: true
});

var _typeof2 = require('babel-runtime/helpers/typeof');

var _typeof3 = _interopRequireDefault(_typeof2);

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

var _rcAnimate = require('rc-animate');

var _rcAnimate2 = _interopRequireDefault(_rcAnimate);

var _ScrollNumber = require('./ScrollNumber');

var _ScrollNumber2 = _interopRequireDefault(_ScrollNumber);

var _classnames = require('classnames');

var _classnames2 = _interopRequireDefault(_classnames);

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

var Badge = function (_React$Component) {
    (0, _inherits3['default'])(Badge, _React$Component);

    function Badge() {
        (0, _classCallCheck3['default'])(this, Badge);
        return (0, _possibleConstructorReturn3['default'])(this, (Badge.__proto__ || Object.getPrototypeOf(Badge)).apply(this, arguments));
    }

    (0, _createClass3['default'])(Badge, [{
        key: 'getBadgeClassName',
        value: function getBadgeClassName() {
            var _classNames;

            var _props = this.props,
                prefixCls = _props.prefixCls,
                className = _props.className,
                status = _props.status,
                children = _props.children;

            return (0, _classnames2['default'])(className, prefixCls, (_classNames = {}, (0, _defineProperty3['default'])(_classNames, prefixCls + '-status', !!status), (0, _defineProperty3['default'])(_classNames, prefixCls + '-not-a-wrapper', !children), _classNames));
        }
    }, {
        key: 'isZero',
        value: function isZero() {
            var numberedDispayCount = this.getNumberedDispayCount();
            return numberedDispayCount === '0' || numberedDispayCount === 0;
        }
    }, {
        key: 'isDot',
        value: function isDot() {
            var _props2 = this.props,
                dot = _props2.dot,
                status = _props2.status;

            var isZero = this.isZero();
            return dot && !isZero || status;
        }
    }, {
        key: 'isHidden',
        value: function isHidden() {
            var showZero = this.props.showZero;

            var displayCount = this.getDispayCount();
            var isZero = this.isZero();
            var isDot = this.isDot();
            var isEmpty = displayCount === null || displayCount === undefined || displayCount === '';
            return (isEmpty || isZero && !showZero) && !isDot;
        }
    }, {
        key: 'getNumberedDispayCount',
        value: function getNumberedDispayCount() {
            var _props3 = this.props,
                count = _props3.count,
                overflowCount = _props3.overflowCount;

            var displayCount = count > overflowCount ? overflowCount + '+' : count;
            return displayCount;
        }
    }, {
        key: 'getDispayCount',
        value: function getDispayCount() {
            var isDot = this.isDot();
            // dot mode don't need count
            if (isDot) {
                return '';
            }
            return this.getNumberedDispayCount();
        }
    }, {
        key: 'getScollNumberTitle',
        value: function getScollNumberTitle() {
            var _props4 = this.props,
                title = _props4.title,
                count = _props4.count;

            if (title) {
                return title;
            }
            return typeof count === 'string' || typeof count === 'number' ? count : undefined;
        }
    }, {
        key: 'getStyleWithOffset',
        value: function getStyleWithOffset() {
            var _props5 = this.props,
                offset = _props5.offset,
                style = _props5.style;

            return offset ? (0, _extends3['default'])({ right: -parseInt(offset[0], 10), marginTop: offset[1] }, style) : style;
        }
    }, {
        key: 'renderStatusText',
        value: function renderStatusText() {
            var _props6 = this.props,
                prefixCls = _props6.prefixCls,
                text = _props6.text;

            var hidden = this.isHidden();
            return hidden || !text ? null : React.createElement(
                'span',
                { className: prefixCls + '-status-text' },
                text
            );
        }
    }, {
        key: 'renderDispayComponent',
        value: function renderDispayComponent() {
            var count = this.props.count;

            return count && (typeof count === 'undefined' ? 'undefined' : (0, _typeof3['default'])(count)) === 'object' ? count : undefined;
        }
    }, {
        key: 'renderBadgeNumber',
        value: function renderBadgeNumber() {
            var _classNames2;

            var _props7 = this.props,
                count = _props7.count,
                prefixCls = _props7.prefixCls,
                scrollNumberPrefixCls = _props7.scrollNumberPrefixCls,
                status = _props7.status;

            var displayCount = this.getDispayCount();
            var isDot = this.isDot();
            var hidden = this.isHidden();
            var scrollNumberCls = (0, _classnames2['default'])((_classNames2 = {}, (0, _defineProperty3['default'])(_classNames2, prefixCls + '-dot', isDot), (0, _defineProperty3['default'])(_classNames2, prefixCls + '-count', !isDot), (0, _defineProperty3['default'])(_classNames2, prefixCls + '-multiple-words', !isDot && count && count.toString && count.toString().length > 1), (0, _defineProperty3['default'])(_classNames2, prefixCls + '-status-' + status, !!status), _classNames2));
            var styleWithOffset = this.getStyleWithOffset();
            return hidden ? null : React.createElement(_ScrollNumber2['default'], { prefixCls: scrollNumberPrefixCls, 'data-show': !hidden, className: scrollNumberCls, count: displayCount, displayComponent: this.renderDispayComponent() // <Badge status="success" count={<Icon type="xxx" />}></Badge>
                , title: this.getScollNumberTitle(), style: styleWithOffset, key: 'scrollNumber' });
        }
    }, {
        key: 'render',
        value: function render() {
            var _classNames3;

            var _a = this.props,
                count = _a.count,
                showZero = _a.showZero,
                prefixCls = _a.prefixCls,
                scrollNumberPrefixCls = _a.scrollNumberPrefixCls,
                overflowCount = _a.overflowCount,
                className = _a.className,
                style = _a.style,
                children = _a.children,
                dot = _a.dot,
                status = _a.status,
                text = _a.text,
                offset = _a.offset,
                title = _a.title,
                restProps = __rest(_a, ["count", "showZero", "prefixCls", "scrollNumberPrefixCls", "overflowCount", "className", "style", "children", "dot", "status", "text", "offset", "title"]);
            var scrollNumber = this.renderBadgeNumber();
            var statusText = this.renderStatusText();
            var statusCls = (0, _classnames2['default'])((_classNames3 = {}, (0, _defineProperty3['default'])(_classNames3, prefixCls + '-status-dot', !!status), (0, _defineProperty3['default'])(_classNames3, prefixCls + '-status-' + status, !!status), _classNames3));
            var styleWithOffset = this.getStyleWithOffset();
            // <Badge status="success" />
            if (!children && status) {
                return React.createElement(
                    'span',
                    (0, _extends3['default'])({}, restProps, { className: this.getBadgeClassName(), style: styleWithOffset }),
                    React.createElement('span', { className: statusCls }),
                    React.createElement(
                        'span',
                        { className: prefixCls + '-status-text' },
                        text
                    )
                );
            }
            return React.createElement(
                'span',
                (0, _extends3['default'])({}, restProps, { className: this.getBadgeClassName() }),
                children,
                React.createElement(
                    _rcAnimate2['default'],
                    { component: '', showProp: 'data-show', transitionName: children ? prefixCls + '-zoom' : '', transitionAppear: true },
                    scrollNumber
                ),
                statusText
            );
        }
    }]);
    return Badge;
}(React.Component);

exports['default'] = Badge;

Badge.defaultProps = {
    prefixCls: 'ant-badge',
    scrollNumberPrefixCls: 'ant-scroll-number',
    count: null,
    showZero: false,
    dot: false,
    overflowCount: 99
};
Badge.propTypes = {
    count: PropTypes.node,
    showZero: PropTypes.bool,
    dot: PropTypes.bool,
    overflowCount: PropTypes.number
};
module.exports = exports['default'];
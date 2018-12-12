import _typeof from 'babel-runtime/helpers/typeof';
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
import Animate from 'rc-animate';
import ScrollNumber from './ScrollNumber';
import classNames from 'classnames';

var Badge = function (_React$Component) {
    _inherits(Badge, _React$Component);

    function Badge() {
        _classCallCheck(this, Badge);

        return _possibleConstructorReturn(this, (Badge.__proto__ || Object.getPrototypeOf(Badge)).apply(this, arguments));
    }

    _createClass(Badge, [{
        key: 'getBadgeClassName',
        value: function getBadgeClassName() {
            var _classNames;

            var _props = this.props,
                prefixCls = _props.prefixCls,
                className = _props.className,
                status = _props.status,
                children = _props.children;

            return classNames(className, prefixCls, (_classNames = {}, _defineProperty(_classNames, prefixCls + '-status', !!status), _defineProperty(_classNames, prefixCls + '-not-a-wrapper', !children), _classNames));
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

            return offset ? _extends({ right: -parseInt(offset[0], 10), marginTop: offset[1] }, style) : style;
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

            return count && (typeof count === 'undefined' ? 'undefined' : _typeof(count)) === 'object' ? count : undefined;
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
            var scrollNumberCls = classNames((_classNames2 = {}, _defineProperty(_classNames2, prefixCls + '-dot', isDot), _defineProperty(_classNames2, prefixCls + '-count', !isDot), _defineProperty(_classNames2, prefixCls + '-multiple-words', !isDot && count && count.toString && count.toString().length > 1), _defineProperty(_classNames2, prefixCls + '-status-' + status, !!status), _classNames2));
            var styleWithOffset = this.getStyleWithOffset();
            return hidden ? null : React.createElement(ScrollNumber, { prefixCls: scrollNumberPrefixCls, 'data-show': !hidden, className: scrollNumberCls, count: displayCount, displayComponent: this.renderDispayComponent() // <Badge status="success" count={<Icon type="xxx" />}></Badge>
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
            var statusCls = classNames((_classNames3 = {}, _defineProperty(_classNames3, prefixCls + '-status-dot', !!status), _defineProperty(_classNames3, prefixCls + '-status-' + status, !!status), _classNames3));
            var styleWithOffset = this.getStyleWithOffset();
            // <Badge status="success" />
            if (!children && status) {
                return React.createElement(
                    'span',
                    _extends({}, restProps, { className: this.getBadgeClassName(), style: styleWithOffset }),
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
                _extends({}, restProps, { className: this.getBadgeClassName() }),
                children,
                React.createElement(
                    Animate,
                    { component: '', showProp: 'data-show', transitionName: children ? prefixCls + '-zoom' : '', transitionAppear: true },
                    scrollNumber
                ),
                statusText
            );
        }
    }]);

    return Badge;
}(React.Component);

export default Badge;

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
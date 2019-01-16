'use strict';

Object.defineProperty(exports, "__esModule", {
    value: true
});

var _extends2 = require('babel-runtime/helpers/extends');

var _extends3 = _interopRequireDefault(_extends2);

var _defineProperty2 = require('babel-runtime/helpers/defineProperty');

var _defineProperty3 = _interopRequireDefault(_defineProperty2);

var _typeof2 = require('babel-runtime/helpers/typeof');

var _typeof3 = _interopRequireDefault(_typeof2);

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

var _ScrollableInkTabBar = require('rc-tabs/lib/ScrollableInkTabBar');

var _ScrollableInkTabBar2 = _interopRequireDefault(_ScrollableInkTabBar);

var _classnames = require('classnames');

var _classnames2 = _interopRequireDefault(_classnames);

var _icon = require('../icon');

var _icon2 = _interopRequireDefault(_icon);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj['default'] = obj; return newObj; } }

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { 'default': obj }; }

var TabBar = function (_React$Component) {
    (0, _inherits3['default'])(TabBar, _React$Component);

    function TabBar() {
        (0, _classCallCheck3['default'])(this, TabBar);
        return (0, _possibleConstructorReturn3['default'])(this, (TabBar.__proto__ || Object.getPrototypeOf(TabBar)).apply(this, arguments));
    }

    (0, _createClass3['default'])(TabBar, [{
        key: 'render',
        value: function render() {
            var _classNames;

            var _props = this.props,
                tabBarStyle = _props.tabBarStyle,
                animated = _props.animated,
                renderTabBar = _props.renderTabBar,
                tabBarExtraContent = _props.tabBarExtraContent,
                tabPosition = _props.tabPosition,
                prefixCls = _props.prefixCls,
                className = _props.className,
                size = _props.size,
                type = _props.type;

            var inkBarAnimated = (typeof animated === 'undefined' ? 'undefined' : (0, _typeof3['default'])(animated)) === 'object' ? animated.inkBar : animated;
            var isVertical = tabPosition === 'left' || tabPosition === 'right';
            var prevIconType = isVertical ? 'up' : 'left';
            var nextIconType = isVertical ? 'down' : 'right';
            var prevIcon = React.createElement(
                'span',
                { className: prefixCls + '-tab-prev-icon' },
                React.createElement(_icon2['default'], { type: prevIconType, className: prefixCls + '-tab-prev-icon-target' })
            );
            var nextIcon = React.createElement(
                'span',
                { className: prefixCls + '-tab-next-icon' },
                React.createElement(_icon2['default'], { type: nextIconType, className: prefixCls + '-tab-next-icon-target' })
            );
            // Additional className for style usage
            var cls = (0, _classnames2['default'])(prefixCls + '-' + tabPosition + '-bar', (_classNames = {}, (0, _defineProperty3['default'])(_classNames, prefixCls + '-' + size + '-bar', !!size), (0, _defineProperty3['default'])(_classNames, prefixCls + '-card-bar', type && type.indexOf('card') >= 0), _classNames), className);
            var renderProps = (0, _extends3['default'])({}, this.props, { inkBarAnimated: inkBarAnimated, extraContent: tabBarExtraContent, style: tabBarStyle, prevIcon: prevIcon,
                nextIcon: nextIcon, className: cls });
            var RenderTabBar = void 0;
            if (renderTabBar) {
                RenderTabBar = renderTabBar(renderProps, _ScrollableInkTabBar2['default']);
            } else {
                RenderTabBar = React.createElement(_ScrollableInkTabBar2['default'], renderProps);
            }
            return React.cloneElement(RenderTabBar);
        }
    }]);
    return TabBar;
}(React.Component);

exports['default'] = TabBar;

TabBar.defaultProps = {
    animated: true,
    type: 'line'
};
module.exports = exports['default'];
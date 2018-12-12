import _extends from 'babel-runtime/helpers/extends';
import _defineProperty from 'babel-runtime/helpers/defineProperty';
import _typeof from 'babel-runtime/helpers/typeof';
import _classCallCheck from 'babel-runtime/helpers/classCallCheck';
import _createClass from 'babel-runtime/helpers/createClass';
import _possibleConstructorReturn from 'babel-runtime/helpers/possibleConstructorReturn';
import _inherits from 'babel-runtime/helpers/inherits';
import * as React from 'react';
import ScrollableInkTabBar from 'rc-tabs/es/ScrollableInkTabBar';
import classNames from 'classnames';
import Icon from '../icon';

var TabBar = function (_React$Component) {
    _inherits(TabBar, _React$Component);

    function TabBar() {
        _classCallCheck(this, TabBar);

        return _possibleConstructorReturn(this, (TabBar.__proto__ || Object.getPrototypeOf(TabBar)).apply(this, arguments));
    }

    _createClass(TabBar, [{
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

            var inkBarAnimated = (typeof animated === 'undefined' ? 'undefined' : _typeof(animated)) === 'object' ? animated.inkBar : animated;
            var isVertical = tabPosition === 'left' || tabPosition === 'right';
            var prevIconType = isVertical ? 'up' : 'left';
            var nextIconType = isVertical ? 'down' : 'right';
            var prevIcon = React.createElement(
                'span',
                { className: prefixCls + '-tab-prev-icon' },
                React.createElement(Icon, { type: prevIconType, className: prefixCls + '-tab-prev-icon-target' })
            );
            var nextIcon = React.createElement(
                'span',
                { className: prefixCls + '-tab-next-icon' },
                React.createElement(Icon, { type: nextIconType, className: prefixCls + '-tab-next-icon-target' })
            );
            // Additional className for style usage
            var cls = classNames(prefixCls + '-' + tabPosition + '-bar', (_classNames = {}, _defineProperty(_classNames, prefixCls + '-' + size + '-bar', !!size), _defineProperty(_classNames, prefixCls + '-card-bar', type && type.indexOf('card') >= 0), _classNames), className);
            var renderProps = _extends({}, this.props, { inkBarAnimated: inkBarAnimated, extraContent: tabBarExtraContent, style: tabBarStyle, prevIcon: prevIcon,
                nextIcon: nextIcon, className: cls });
            var RenderTabBar = void 0;
            if (renderTabBar) {
                RenderTabBar = renderTabBar(renderProps, ScrollableInkTabBar);
            } else {
                RenderTabBar = React.createElement(ScrollableInkTabBar, renderProps);
            }
            return React.cloneElement(RenderTabBar);
        }
    }]);

    return TabBar;
}(React.Component);

export default TabBar;

TabBar.defaultProps = {
    animated: true,
    type: 'line'
};
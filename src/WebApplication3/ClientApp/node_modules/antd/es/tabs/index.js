import _extends from 'babel-runtime/helpers/extends';
import _defineProperty from 'babel-runtime/helpers/defineProperty';
import _typeof from 'babel-runtime/helpers/typeof';
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
import * as ReactDOM from 'react-dom';
import RcTabs, { TabPane } from 'rc-tabs';
import TabContent from 'rc-tabs/es/TabContent';
import TabBar from './TabBar';
import classNames from 'classnames';
import Icon from '../icon';
import warning from '../_util/warning';
import isFlexSupported from '../_util/isFlexSupported';

var Tabs = function (_React$Component) {
    _inherits(Tabs, _React$Component);

    function Tabs() {
        _classCallCheck(this, Tabs);

        var _this = _possibleConstructorReturn(this, (Tabs.__proto__ || Object.getPrototypeOf(Tabs)).apply(this, arguments));

        _this.removeTab = function (targetKey, e) {
            e.stopPropagation();
            if (!targetKey) {
                return;
            }
            var onEdit = _this.props.onEdit;
            if (onEdit) {
                onEdit(targetKey, 'remove');
            }
        };
        _this.handleChange = function (activeKey) {
            var onChange = _this.props.onChange;
            if (onChange) {
                onChange(activeKey);
            }
        };
        _this.createNewTab = function (targetKey) {
            var onEdit = _this.props.onEdit;

            if (onEdit) {
                onEdit(targetKey, 'add');
            }
        };
        return _this;
    }

    _createClass(Tabs, [{
        key: 'componentDidMount',
        value: function componentDidMount() {
            var NO_FLEX = ' no-flex';
            var tabNode = ReactDOM.findDOMNode(this);
            if (tabNode && !isFlexSupported() && tabNode.className.indexOf(NO_FLEX) === -1) {
                tabNode.className += NO_FLEX;
            }
        }
    }, {
        key: 'render',
        value: function render() {
            var _classNames,
                _this2 = this;

            var _props = this.props,
                prefixCls = _props.prefixCls,
                _props$className = _props.className,
                className = _props$className === undefined ? '' : _props$className,
                size = _props.size,
                _props$type = _props.type,
                type = _props$type === undefined ? 'line' : _props$type,
                tabPosition = _props.tabPosition,
                children = _props.children,
                _props$animated = _props.animated,
                animated = _props$animated === undefined ? true : _props$animated,
                hideAdd = _props.hideAdd;
            var tabBarExtraContent = this.props.tabBarExtraContent;

            var tabPaneAnimated = (typeof animated === 'undefined' ? 'undefined' : _typeof(animated)) === 'object' ? animated.tabPane : animated;
            // card tabs should not have animation
            if (type !== 'line') {
                tabPaneAnimated = 'animated' in this.props ? tabPaneAnimated : false;
            }
            warning(!(type.indexOf('card') >= 0 && (size === 'small' || size === 'large')), "Tabs[type=card|editable-card] doesn't have small or large size, it's by design.");
            var cls = classNames(className, (_classNames = {}, _defineProperty(_classNames, prefixCls + '-vertical', tabPosition === 'left' || tabPosition === 'right'), _defineProperty(_classNames, prefixCls + '-' + size, !!size), _defineProperty(_classNames, prefixCls + '-card', type.indexOf('card') >= 0), _defineProperty(_classNames, prefixCls + '-' + type, true), _defineProperty(_classNames, prefixCls + '-no-animation', !tabPaneAnimated), _classNames));
            // only card type tabs can be added and closed
            var childrenWithClose = [];
            if (type === 'editable-card') {
                childrenWithClose = [];
                React.Children.forEach(children, function (child, index) {
                    var closable = child.props.closable;
                    closable = typeof closable === 'undefined' ? true : closable;
                    var closeIcon = closable ? React.createElement(Icon, { type: 'close', className: prefixCls + '-close-x', onClick: function onClick(e) {
                            return _this2.removeTab(child.key, e);
                        } }) : null;
                    childrenWithClose.push(React.cloneElement(child, {
                        tab: React.createElement(
                            'div',
                            { className: closable ? undefined : prefixCls + '-tab-unclosable' },
                            child.props.tab,
                            closeIcon
                        ),
                        key: child.key || index
                    }));
                });
                // Add new tab handler
                if (!hideAdd) {
                    tabBarExtraContent = React.createElement(
                        'span',
                        null,
                        React.createElement(Icon, { type: 'plus', className: prefixCls + '-new-tab', onClick: this.createNewTab }),
                        tabBarExtraContent
                    );
                }
            }
            tabBarExtraContent = tabBarExtraContent ? React.createElement(
                'div',
                { className: prefixCls + '-extra-content' },
                tabBarExtraContent
            ) : null;
            var _a = this.props,
                dropped = _a.className,
                tabBarProps = __rest(_a, ["className"]);
            var contentCls = classNames(prefixCls + '-' + tabPosition + '-content', type.indexOf('card') >= 0 && prefixCls + '-card-content');
            return React.createElement(
                RcTabs,
                _extends({}, this.props, { className: cls, tabBarPosition: tabPosition, renderTabBar: function renderTabBar() {
                        return React.createElement(TabBar, _extends({}, tabBarProps, { tabBarExtraContent: tabBarExtraContent }));
                    }, renderTabContent: function renderTabContent() {
                        return React.createElement(TabContent, { className: contentCls, animated: tabPaneAnimated, animatedWithMargin: true });
                    }, onChange: this.handleChange }),
                childrenWithClose.length > 0 ? childrenWithClose : children
            );
        }
    }]);

    return Tabs;
}(React.Component);

export default Tabs;

Tabs.TabPane = TabPane;
Tabs.defaultProps = {
    prefixCls: 'ant-tabs',
    hideAdd: false,
    tabPosition: 'top'
};
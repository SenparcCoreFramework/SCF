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
import { Item } from 'rc-menu';
import * as PropTypes from 'prop-types';
import Tooltip from '../tooltip';

var MenuItem = function (_React$Component) {
    _inherits(MenuItem, _React$Component);

    function MenuItem() {
        _classCallCheck(this, MenuItem);

        var _this = _possibleConstructorReturn(this, (MenuItem.__proto__ || Object.getPrototypeOf(MenuItem)).apply(this, arguments));

        _this.onKeyDown = function (e) {
            _this.menuItem.onKeyDown(e);
        };
        _this.saveMenuItem = function (menuItem) {
            _this.menuItem = menuItem;
        };
        return _this;
    }

    _createClass(MenuItem, [{
        key: 'render',
        value: function render() {
            var inlineCollapsed = this.context.inlineCollapsed;
            var _props = this.props,
                level = _props.level,
                children = _props.children,
                rootPrefixCls = _props.rootPrefixCls;
            var _a = this.props,
                title = _a.title,
                rest = __rest(_a, ["title"]);
            var titleNode = void 0;
            if (inlineCollapsed) {
                titleNode = title || (level === 1 ? children : '');
            }
            return React.createElement(
                Tooltip,
                { title: titleNode, placement: 'right', overlayClassName: rootPrefixCls + '-inline-collapsed-tooltip' },
                React.createElement(Item, _extends({}, rest, { title: inlineCollapsed ? null : title, ref: this.saveMenuItem }))
            );
        }
    }]);

    return MenuItem;
}(React.Component);

MenuItem.contextTypes = {
    inlineCollapsed: PropTypes.bool
};
MenuItem.isMenuItem = 1;
export default MenuItem;
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
import RcTreeSelect, { TreeNode, SHOW_ALL, SHOW_PARENT, SHOW_CHILD } from 'rc-tree-select';
import classNames from 'classnames';
import LocaleReceiver from '../locale-provider/LocaleReceiver';
import { ConfigConsumer } from '../config-provider';
import warning from '../_util/warning';
import Icon from '../icon';
import omit from 'omit.js';

var TreeSelect = function (_React$Component) {
    _inherits(TreeSelect, _React$Component);

    function TreeSelect(props) {
        _classCallCheck(this, TreeSelect);

        var _this = _possibleConstructorReturn(this, (TreeSelect.__proto__ || Object.getPrototypeOf(TreeSelect)).call(this, props));

        _this.saveTreeSelect = function (node) {
            _this.rcTreeSelect = node;
        };
        _this.renderSwitcherIcon = function (_ref) {
            var isLeaf = _ref.isLeaf,
                loading = _ref.loading;
            var prefixCls = _this.props.prefixCls;

            if (loading) {
                return React.createElement(Icon, { type: 'loading', className: prefixCls + '-switcher-loading-icon' });
            }
            if (isLeaf) {
                return null;
            }
            return React.createElement(Icon, { type: 'caret-down', className: prefixCls + '-switcher-icon' });
        };
        _this.renderTreeSelect = function (locale) {
            var _classNames;

            var _a = _this.props,
                prefixCls = _a.prefixCls,
                className = _a.className,
                size = _a.size,
                notFoundContent = _a.notFoundContent,
                dropdownStyle = _a.dropdownStyle,
                dropdownClassName = _a.dropdownClassName,
                suffixIcon = _a.suffixIcon,
                getPopupContainer = _a.getPopupContainer,
                restProps = __rest(_a, ["prefixCls", "className", "size", "notFoundContent", "dropdownStyle", "dropdownClassName", "suffixIcon", "getPopupContainer"]);
            var rest = omit(restProps, ['inputIcon', 'removeIcon', 'clearIcon', 'switcherIcon']);
            var cls = classNames((_classNames = {}, _defineProperty(_classNames, prefixCls + '-lg', size === 'large'), _defineProperty(_classNames, prefixCls + '-sm', size === 'small'), _classNames), className);
            var checkable = rest.treeCheckable;
            if (checkable) {
                checkable = React.createElement('span', { className: prefixCls + '-tree-checkbox-inner' });
            }
            var inputIcon = suffixIcon && (React.isValidElement(suffixIcon) ? React.cloneElement(suffixIcon) : suffixIcon) || React.createElement(Icon, { type: 'down', className: prefixCls + '-arrow-icon' });
            var removeIcon = React.createElement(Icon, { type: 'close', className: prefixCls + '-remove-icon' });
            var clearIcon = React.createElement(Icon, { type: 'close-circle', className: prefixCls + '-clear-icon', theme: 'filled' });
            return React.createElement(
                ConfigConsumer,
                null,
                function (_ref2) {
                    var getContextPopupContainer = _ref2.getPopupContainer;

                    return React.createElement(RcTreeSelect, _extends({ switcherIcon: _this.renderSwitcherIcon, inputIcon: inputIcon, removeIcon: removeIcon, clearIcon: clearIcon }, rest, { getPopupContainer: getPopupContainer || getContextPopupContainer, dropdownClassName: classNames(dropdownClassName, prefixCls + '-tree-dropdown'), prefixCls: prefixCls, className: cls, dropdownStyle: _extends({ maxHeight: '100vh', overflow: 'auto' }, dropdownStyle), treeCheckable: checkable, notFoundContent: notFoundContent || locale.notFoundContent, ref: _this.saveTreeSelect }));
                }
            );
        };
        warning(props.multiple !== false || !props.treeCheckable, '`multiple` will alway be `true` when `treeCheckable` is true');
        return _this;
    }

    _createClass(TreeSelect, [{
        key: 'focus',
        value: function focus() {
            this.rcTreeSelect.focus();
        }
    }, {
        key: 'blur',
        value: function blur() {
            this.rcTreeSelect.blur();
        }
    }, {
        key: 'render',
        value: function render() {
            return React.createElement(
                LocaleReceiver,
                { componentName: 'Select', defaultLocale: {} },
                this.renderTreeSelect
            );
        }
    }]);

    return TreeSelect;
}(React.Component);

export default TreeSelect;

TreeSelect.TreeNode = TreeNode;
TreeSelect.SHOW_ALL = SHOW_ALL;
TreeSelect.SHOW_PARENT = SHOW_PARENT;
TreeSelect.SHOW_CHILD = SHOW_CHILD;
TreeSelect.defaultProps = {
    prefixCls: 'ant-select',
    transitionName: 'slide-up',
    choiceTransitionName: 'zoom',
    showSearch: false
};
import _extends from 'babel-runtime/helpers/extends';
import _classCallCheck from 'babel-runtime/helpers/classCallCheck';
import _createClass from 'babel-runtime/helpers/createClass';
import _possibleConstructorReturn from 'babel-runtime/helpers/possibleConstructorReturn';
import _inherits from 'babel-runtime/helpers/inherits';
import * as React from 'react';
import RcTree, { TreeNode } from 'rc-tree';
import DirectoryTree from './DirectoryTree';
import classNames from 'classnames';
import animation from '../_util/openAnimation';
import Icon from '../icon';

var Tree = function (_React$Component) {
    _inherits(Tree, _React$Component);

    function Tree() {
        _classCallCheck(this, Tree);

        var _this = _possibleConstructorReturn(this, (Tree.__proto__ || Object.getPrototypeOf(Tree)).apply(this, arguments));

        _this.renderSwitcherIcon = function (_ref) {
            var isLeaf = _ref.isLeaf,
                expanded = _ref.expanded,
                loading = _ref.loading;
            var _this$props = _this.props,
                prefixCls = _this$props.prefixCls,
                showLine = _this$props.showLine;

            if (loading) {
                return React.createElement(Icon, { type: 'loading', className: prefixCls + '-switcher-loading-icon' });
            }
            if (showLine) {
                if (isLeaf) {
                    return React.createElement(Icon, { type: 'file', className: prefixCls + '-switcher-line-icon' });
                }
                return React.createElement(Icon, { type: expanded ? 'minus-square' : 'plus-square', className: prefixCls + '-switcher-line-icon', theme: 'outlined' });
            } else {
                if (isLeaf) {
                    return null;
                }
                return React.createElement(Icon, { type: 'caret-down', className: prefixCls + '-switcher-icon', theme: 'filled' });
            }
        };
        _this.setTreeRef = function (node) {
            _this.tree = node;
        };
        return _this;
    }

    _createClass(Tree, [{
        key: 'render',
        value: function render() {
            var props = this.props;
            var prefixCls = props.prefixCls,
                className = props.className,
                showIcon = props.showIcon;

            var checkable = props.checkable;
            return React.createElement(
                RcTree,
                _extends({ ref: this.setTreeRef }, props, { className: classNames(!showIcon && prefixCls + '-icon-hide', className), checkable: checkable ? React.createElement('span', { className: prefixCls + '-checkbox-inner' }) : checkable, switcherIcon: this.renderSwitcherIcon }),
                this.props.children
            );
        }
    }]);

    return Tree;
}(React.Component);

export default Tree;

Tree.TreeNode = TreeNode;
Tree.DirectoryTree = DirectoryTree;
Tree.defaultProps = {
    prefixCls: 'ant-tree',
    checkable: false,
    showIcon: false,
    openAnimation: _extends({}, animation, { appear: null })
};
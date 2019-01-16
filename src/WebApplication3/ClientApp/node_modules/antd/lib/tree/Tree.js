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

var _rcTree = require('rc-tree');

var _rcTree2 = _interopRequireDefault(_rcTree);

var _DirectoryTree = require('./DirectoryTree');

var _DirectoryTree2 = _interopRequireDefault(_DirectoryTree);

var _classnames = require('classnames');

var _classnames2 = _interopRequireDefault(_classnames);

var _openAnimation = require('../_util/openAnimation');

var _openAnimation2 = _interopRequireDefault(_openAnimation);

var _icon = require('../icon');

var _icon2 = _interopRequireDefault(_icon);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj['default'] = obj; return newObj; } }

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { 'default': obj }; }

var Tree = function (_React$Component) {
    (0, _inherits3['default'])(Tree, _React$Component);

    function Tree() {
        (0, _classCallCheck3['default'])(this, Tree);

        var _this = (0, _possibleConstructorReturn3['default'])(this, (Tree.__proto__ || Object.getPrototypeOf(Tree)).apply(this, arguments));

        _this.renderSwitcherIcon = function (_ref) {
            var isLeaf = _ref.isLeaf,
                expanded = _ref.expanded,
                loading = _ref.loading;
            var _this$props = _this.props,
                prefixCls = _this$props.prefixCls,
                showLine = _this$props.showLine;

            if (loading) {
                return React.createElement(_icon2['default'], { type: 'loading', className: prefixCls + '-switcher-loading-icon' });
            }
            if (showLine) {
                if (isLeaf) {
                    return React.createElement(_icon2['default'], { type: 'file', className: prefixCls + '-switcher-line-icon' });
                }
                return React.createElement(_icon2['default'], { type: expanded ? 'minus-square' : 'plus-square', className: prefixCls + '-switcher-line-icon', theme: 'outlined' });
            } else {
                if (isLeaf) {
                    return null;
                }
                return React.createElement(_icon2['default'], { type: 'caret-down', className: prefixCls + '-switcher-icon', theme: 'filled' });
            }
        };
        _this.setTreeRef = function (node) {
            _this.tree = node;
        };
        return _this;
    }

    (0, _createClass3['default'])(Tree, [{
        key: 'render',
        value: function render() {
            var props = this.props;
            var prefixCls = props.prefixCls,
                className = props.className,
                showIcon = props.showIcon;

            var checkable = props.checkable;
            return React.createElement(
                _rcTree2['default'],
                (0, _extends3['default'])({ ref: this.setTreeRef }, props, { className: (0, _classnames2['default'])(!showIcon && prefixCls + '-icon-hide', className), checkable: checkable ? React.createElement('span', { className: prefixCls + '-checkbox-inner' }) : checkable, switcherIcon: this.renderSwitcherIcon }),
                this.props.children
            );
        }
    }]);
    return Tree;
}(React.Component);

exports['default'] = Tree;

Tree.TreeNode = _rcTree.TreeNode;
Tree.DirectoryTree = _DirectoryTree2['default'];
Tree.defaultProps = {
    prefixCls: 'ant-tree',
    checkable: false,
    showIcon: false,
    openAnimation: (0, _extends3['default'])({}, _openAnimation2['default'], { appear: null })
};
module.exports = exports['default'];
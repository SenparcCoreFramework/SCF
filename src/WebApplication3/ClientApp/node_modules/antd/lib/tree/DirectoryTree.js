'use strict';

Object.defineProperty(exports, "__esModule", {
    value: true
});

var _extends2 = require('babel-runtime/helpers/extends');

var _extends3 = _interopRequireDefault(_extends2);

var _toConsumableArray2 = require('babel-runtime/helpers/toConsumableArray');

var _toConsumableArray3 = _interopRequireDefault(_toConsumableArray2);

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

var _classnames = require('classnames');

var _classnames2 = _interopRequireDefault(_classnames);

var _omit = require('omit.js');

var _omit2 = _interopRequireDefault(_omit);

var _debounce = require('lodash/debounce');

var _debounce2 = _interopRequireDefault(_debounce);

var _util = require('rc-tree/lib/util');

var _Tree = require('./Tree');

var _Tree2 = _interopRequireDefault(_Tree);

var _util2 = require('./util');

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

function getIcon(props) {
    var isLeaf = props.isLeaf,
        expanded = props.expanded;

    if (isLeaf) {
        return React.createElement(_icon2['default'], { type: 'file' });
    }
    return React.createElement(_icon2['default'], { type: expanded ? 'folder-open' : 'folder' });
}

var DirectoryTree = function (_React$Component) {
    (0, _inherits3['default'])(DirectoryTree, _React$Component);

    function DirectoryTree(props) {
        (0, _classCallCheck3['default'])(this, DirectoryTree);

        var _this = (0, _possibleConstructorReturn3['default'])(this, (DirectoryTree.__proto__ || Object.getPrototypeOf(DirectoryTree)).call(this, props));

        _this.onExpand = function (expandedKeys, info) {
            var onExpand = _this.props.onExpand;

            _this.setUncontrolledState({ expandedKeys: expandedKeys });
            // Call origin function
            if (onExpand) {
                return onExpand(expandedKeys, info);
            }
            return undefined;
        };
        _this.onClick = function (event, node) {
            var _this$props = _this.props,
                onClick = _this$props.onClick,
                expandAction = _this$props.expandAction;
            // Expand the tree

            if (expandAction === 'click') {
                _this.onDebounceExpand(event, node);
            }
            if (onClick) {
                onClick(event, node);
            }
        };
        _this.onDoubleClick = function (event, node) {
            var _this$props2 = _this.props,
                onDoubleClick = _this$props2.onDoubleClick,
                expandAction = _this$props2.expandAction;
            // Expand the tree

            if (expandAction === 'doubleClick') {
                _this.onDebounceExpand(event, node);
            }
            if (onDoubleClick) {
                onDoubleClick(event, node);
            }
        };
        _this.onSelect = function (keys, event) {
            var _this$props3 = _this.props,
                onSelect = _this$props3.onSelect,
                multiple = _this$props3.multiple,
                children = _this$props3.children;
            var _this$state = _this.state,
                _this$state$expandedK = _this$state.expandedKeys,
                expandedKeys = _this$state$expandedK === undefined ? [] : _this$state$expandedK,
                _this$state$selectedK = _this$state.selectedKeys,
                selectedKeys = _this$state$selectedK === undefined ? [] : _this$state$selectedK;
            var node = event.node,
                nativeEvent = event.nativeEvent;
            var _node$props$eventKey = node.props.eventKey,
                eventKey = _node$props$eventKey === undefined ? '' : _node$props$eventKey;

            var newState = {};
            // Windows / Mac single pick
            var ctrlPick = nativeEvent.ctrlKey || nativeEvent.metaKey;
            var shiftPick = nativeEvent.shiftKey;
            // Generate new selected keys
            var newSelectedKeys = selectedKeys.slice();
            if (multiple && ctrlPick) {
                // Control click
                newSelectedKeys = keys;
                _this.lastSelectedKey = eventKey;
                _this.cachedSelectedKeys = newSelectedKeys;
            } else if (multiple && shiftPick) {
                // Shift click
                newSelectedKeys = Array.from(new Set([].concat((0, _toConsumableArray3['default'])(_this.cachedSelectedKeys || []), (0, _toConsumableArray3['default'])((0, _util2.calcRangeKeys)(children, expandedKeys, eventKey, _this.lastSelectedKey)))));
            } else {
                // Single click
                newSelectedKeys = [eventKey];
                _this.lastSelectedKey = eventKey;
                _this.cachedSelectedKeys = newSelectedKeys;
            }
            newState.selectedKeys = newSelectedKeys;
            if (onSelect) {
                onSelect(newSelectedKeys, event);
            }
            _this.setUncontrolledState(newState);
        };
        _this.setTreeRef = function (node) {
            _this.tree = node;
        };
        _this.expandFolderNode = function (event, node) {
            var isLeaf = node.props.isLeaf;

            if (isLeaf || event.shiftKey || event.metaKey || event.ctrlKey) {
                return;
            }
            // Get internal rc-tree
            var internalTree = _this.tree.tree;
            // Call internal rc-tree expand function
            // https://github.com/ant-design/ant-design/issues/12567
            internalTree.onNodeExpand(event, node);
        };
        _this.setUncontrolledState = function (state) {
            var newState = (0, _omit2['default'])(state, Object.keys(_this.props));
            if (Object.keys(newState).length) {
                _this.setState(newState);
            }
        };
        var defaultExpandAll = props.defaultExpandAll,
            defaultExpandParent = props.defaultExpandParent,
            expandedKeys = props.expandedKeys,
            defaultExpandedKeys = props.defaultExpandedKeys,
            children = props.children;

        var _convertTreeToEntitie = (0, _util.convertTreeToEntities)(children),
            keyEntities = _convertTreeToEntitie.keyEntities;
        // Selected keys


        _this.state = {
            selectedKeys: props.selectedKeys || props.defaultSelectedKeys || []
        };
        // Expanded keys
        if (defaultExpandAll) {
            _this.state.expandedKeys = (0, _util2.getFullKeyList)(props.children);
        } else if (defaultExpandParent) {
            _this.state.expandedKeys = (0, _util.conductExpandParent)(expandedKeys || defaultExpandedKeys, keyEntities);
        } else {
            _this.state.expandedKeys = expandedKeys || defaultExpandedKeys;
        }
        _this.onDebounceExpand = (0, _debounce2['default'])(_this.expandFolderNode, 200, {
            leading: true
        });
        return _this;
    }

    (0, _createClass3['default'])(DirectoryTree, [{
        key: 'componentWillReceiveProps',
        value: function componentWillReceiveProps(nextProps) {
            if ('expandedKeys' in nextProps) {
                this.setState({ expandedKeys: nextProps.expandedKeys });
            }
            if ('selectedKeys' in nextProps) {
                this.setState({ selectedKeys: nextProps.selectedKeys });
            }
        }
    }, {
        key: 'render',
        value: function render() {
            var _a = this.props,
                prefixCls = _a.prefixCls,
                className = _a.className,
                props = __rest(_a, ["prefixCls", "className"]);var _state = this.state,
                expandedKeys = _state.expandedKeys,
                selectedKeys = _state.selectedKeys;

            var connectClassName = (0, _classnames2['default'])(prefixCls + '-directory', className);
            return React.createElement(_Tree2['default'], (0, _extends3['default'])({ icon: getIcon, ref: this.setTreeRef }, props, { prefixCls: prefixCls, className: connectClassName, expandedKeys: expandedKeys, selectedKeys: selectedKeys, onSelect: this.onSelect, onClick: this.onClick, onDoubleClick: this.onDoubleClick, onExpand: this.onExpand }));
        }
    }]);
    return DirectoryTree;
}(React.Component);

exports['default'] = DirectoryTree;

DirectoryTree.defaultProps = {
    prefixCls: 'ant-tree',
    showIcon: true,
    expandAction: 'click'
};
module.exports = exports['default'];
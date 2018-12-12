import _extends from 'babel-runtime/helpers/extends';
import _toConsumableArray from 'babel-runtime/helpers/toConsumableArray';
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
import classNames from 'classnames';
import omit from 'omit.js';
import debounce from 'lodash/debounce';
import { conductExpandParent, convertTreeToEntities } from 'rc-tree/es/util';
import Tree from './Tree';
import { calcRangeKeys, getFullKeyList } from './util';
import Icon from '../icon';
function getIcon(props) {
    var isLeaf = props.isLeaf,
        expanded = props.expanded;

    if (isLeaf) {
        return React.createElement(Icon, { type: 'file' });
    }
    return React.createElement(Icon, { type: expanded ? 'folder-open' : 'folder' });
}

var DirectoryTree = function (_React$Component) {
    _inherits(DirectoryTree, _React$Component);

    function DirectoryTree(props) {
        _classCallCheck(this, DirectoryTree);

        var _this = _possibleConstructorReturn(this, (DirectoryTree.__proto__ || Object.getPrototypeOf(DirectoryTree)).call(this, props));

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
                newSelectedKeys = Array.from(new Set([].concat(_toConsumableArray(_this.cachedSelectedKeys || []), _toConsumableArray(calcRangeKeys(children, expandedKeys, eventKey, _this.lastSelectedKey)))));
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
            var newState = omit(state, Object.keys(_this.props));
            if (Object.keys(newState).length) {
                _this.setState(newState);
            }
        };
        var defaultExpandAll = props.defaultExpandAll,
            defaultExpandParent = props.defaultExpandParent,
            expandedKeys = props.expandedKeys,
            defaultExpandedKeys = props.defaultExpandedKeys,
            children = props.children;

        var _convertTreeToEntitie = convertTreeToEntities(children),
            keyEntities = _convertTreeToEntitie.keyEntities;
        // Selected keys


        _this.state = {
            selectedKeys: props.selectedKeys || props.defaultSelectedKeys || []
        };
        // Expanded keys
        if (defaultExpandAll) {
            _this.state.expandedKeys = getFullKeyList(props.children);
        } else if (defaultExpandParent) {
            _this.state.expandedKeys = conductExpandParent(expandedKeys || defaultExpandedKeys, keyEntities);
        } else {
            _this.state.expandedKeys = expandedKeys || defaultExpandedKeys;
        }
        _this.onDebounceExpand = debounce(_this.expandFolderNode, 200, {
            leading: true
        });
        return _this;
    }

    _createClass(DirectoryTree, [{
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

            var connectClassName = classNames(prefixCls + '-directory', className);
            return React.createElement(Tree, _extends({ icon: getIcon, ref: this.setTreeRef }, props, { prefixCls: prefixCls, className: connectClassName, expandedKeys: expandedKeys, selectedKeys: selectedKeys, onSelect: this.onSelect, onClick: this.onClick, onDoubleClick: this.onDoubleClick, onExpand: this.onExpand }));
        }
    }]);

    return DirectoryTree;
}(React.Component);

export default DirectoryTree;

DirectoryTree.defaultProps = {
    prefixCls: 'ant-tree',
    showIcon: true,
    expandAction: 'click'
};
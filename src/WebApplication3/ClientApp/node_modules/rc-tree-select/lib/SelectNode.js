'use strict';

Object.defineProperty(exports, "__esModule", {
  value: true
});

var _extends2 = require('babel-runtime/helpers/extends');

var _extends3 = _interopRequireDefault(_extends2);

var _react = require('react');

var _react2 = _interopRequireDefault(_react);

var _rcTree = require('rc-tree');

var _propTypes = require('./propTypes');

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { 'default': obj }; }

/**
 * SelectNode wrapped the tree node.
 * Let's use SelectNode instead of TreeNode
 * since TreeNode is so confuse here.
 */
var SelectNode = function SelectNode(props) {
  return _react2['default'].createElement(_rcTree.TreeNode, props);
};

SelectNode.propTypes = (0, _extends3['default'])({}, _rcTree.TreeNode.propTypes, {
  value: _propTypes.valueProp
});

// Let Tree trade as TreeNode to reuse this for performance saving.
SelectNode.isTreeNode = 1;

exports['default'] = SelectNode;
module.exports = exports['default'];
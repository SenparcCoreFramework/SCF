import _extends from 'babel-runtime/helpers/extends';
import React from 'react';
import { TreeNode } from 'rc-tree';
import { valueProp } from './propTypes';

/**
 * SelectNode wrapped the tree node.
 * Let's use SelectNode instead of TreeNode
 * since TreeNode is so confuse here.
 */
var SelectNode = function SelectNode(props) {
  return React.createElement(TreeNode, props);
};

SelectNode.propTypes = _extends({}, TreeNode.propTypes, {
  value: valueProp
});

// Let Tree trade as TreeNode to reuse this for performance saving.
SelectNode.isTreeNode = 1;

export default SelectNode;
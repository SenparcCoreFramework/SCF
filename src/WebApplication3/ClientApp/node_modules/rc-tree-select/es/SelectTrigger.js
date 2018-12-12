import _defineProperty from 'babel-runtime/helpers/defineProperty';
import _classCallCheck from 'babel-runtime/helpers/classCallCheck';
import _createClass from 'babel-runtime/helpers/createClass';
import _possibleConstructorReturn from 'babel-runtime/helpers/possibleConstructorReturn';
import _inherits from 'babel-runtime/helpers/inherits';
import React from 'react';
import PropTypes from 'prop-types';
import { polyfill } from 'react-lifecycles-compat';
import Trigger from 'rc-trigger';
import classNames from 'classnames';

import { createRef } from './util';

var BUILT_IN_PLACEMENTS = {
  bottomLeft: {
    points: ['tl', 'bl'],
    offset: [0, 4],
    overflow: {
      adjustX: 0,
      adjustY: 1
    },
    ignoreShake: true
  },
  topLeft: {
    points: ['bl', 'tl'],
    offset: [0, -4],
    overflow: {
      adjustX: 0,
      adjustY: 1
    },
    ignoreShake: true
  }
};

var SelectTrigger = function (_React$Component) {
  _inherits(SelectTrigger, _React$Component);

  function SelectTrigger() {
    _classCallCheck(this, SelectTrigger);

    var _this = _possibleConstructorReturn(this, (SelectTrigger.__proto__ || Object.getPrototypeOf(SelectTrigger)).call(this));

    _this.getDropdownTransitionName = function () {
      var _this$props = _this.props,
          transitionName = _this$props.transitionName,
          animation = _this$props.animation,
          dropdownPrefixCls = _this$props.dropdownPrefixCls;

      if (!transitionName && animation) {
        return dropdownPrefixCls + '-' + animation;
      }
      return transitionName;
    };

    _this.forcePopupAlign = function () {
      var $trigger = _this.triggerRef.current;

      if ($trigger) {
        $trigger.forcePopupAlign();
      }
    };

    _this.triggerRef = createRef();
    return _this;
  }

  _createClass(SelectTrigger, [{
    key: 'render',
    value: function render() {
      var _classNames;

      var _props = this.props,
          disabled = _props.disabled,
          isMultiple = _props.isMultiple,
          dropdownPopupAlign = _props.dropdownPopupAlign,
          dropdownMatchSelectWidth = _props.dropdownMatchSelectWidth,
          dropdownClassName = _props.dropdownClassName,
          dropdownStyle = _props.dropdownStyle,
          onDropdownVisibleChange = _props.onDropdownVisibleChange,
          getPopupContainer = _props.getPopupContainer,
          dropdownPrefixCls = _props.dropdownPrefixCls,
          popupElement = _props.popupElement,
          open = _props.open,
          children = _props.children;

      // TODO: [Legacy] Use new action when trigger fixed: https://github.com/react-component/trigger/pull/86

      // When false do nothing with the width
      // ref: https://github.com/ant-design/ant-design/issues/10927

      var stretch = void 0;
      if (dropdownMatchSelectWidth !== false) {
        stretch = dropdownMatchSelectWidth ? 'width' : 'minWidth';
      }

      return React.createElement(
        Trigger,
        {
          ref: this.triggerRef,
          action: disabled ? [] : ['click'],
          popupPlacement: 'bottomLeft',
          builtinPlacements: BUILT_IN_PLACEMENTS,
          popupAlign: dropdownPopupAlign,
          prefixCls: dropdownPrefixCls,
          popupTransitionName: this.getDropdownTransitionName(),
          onPopupVisibleChange: onDropdownVisibleChange,
          popup: popupElement,
          popupVisible: open,
          getPopupContainer: getPopupContainer,
          stretch: stretch,
          popupClassName: classNames(dropdownClassName, (_classNames = {}, _defineProperty(_classNames, dropdownPrefixCls + '--multiple', isMultiple), _defineProperty(_classNames, dropdownPrefixCls + '--single', !isMultiple), _classNames)),
          popupStyle: dropdownStyle
        },
        children
      );
    }
  }]);

  return SelectTrigger;
}(React.Component);

SelectTrigger.propTypes = {
  // Pass by outside user props
  disabled: PropTypes.bool,
  showSearch: PropTypes.bool,
  prefixCls: PropTypes.string,
  dropdownPopupAlign: PropTypes.object,
  dropdownClassName: PropTypes.string,
  dropdownStyle: PropTypes.object,
  transitionName: PropTypes.string,
  animation: PropTypes.string,
  getPopupContainer: PropTypes.func,
  children: PropTypes.node,

  dropdownMatchSelectWidth: PropTypes.bool,

  // Pass by Select
  isMultiple: PropTypes.bool,
  dropdownPrefixCls: PropTypes.string,
  onDropdownVisibleChange: PropTypes.func,
  popupElement: PropTypes.node,
  open: PropTypes.bool
};


polyfill(SelectTrigger);

export default SelectTrigger;
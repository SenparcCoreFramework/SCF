import _extends from 'babel-runtime/helpers/extends';
import _classCallCheck from 'babel-runtime/helpers/classCallCheck';
import _createClass from 'babel-runtime/helpers/createClass';
import _possibleConstructorReturn from 'babel-runtime/helpers/possibleConstructorReturn';
import _inherits from 'babel-runtime/helpers/inherits';
import React from 'react';
import PropTypes from 'prop-types';
import Animate from 'rc-animate';

import generateSelector, { selectorPropTypes } from '../../Base/BaseSelector';
import SearchInput from '../../SearchInput';
import Selection from './Selection';
import { createRef } from '../../util';

var TREE_SELECT_EMPTY_VALUE_KEY = 'RC_TREE_SELECT_EMPTY_VALUE_KEY';

var Selector = generateSelector('multiple');

export var multipleSelectorContextTypes = {
  onMultipleSelectorRemove: PropTypes.func.isRequired
};

var MultipleSelector = function (_React$Component) {
  _inherits(MultipleSelector, _React$Component);

  function MultipleSelector() {
    _classCallCheck(this, MultipleSelector);

    var _this = _possibleConstructorReturn(this, (MultipleSelector.__proto__ || Object.getPrototypeOf(MultipleSelector)).call(this));

    _this.onPlaceholderClick = function () {
      _this.inputRef.current.focus();
    };

    _this.focus = function () {
      _this.inputRef.current.focus();
    };

    _this.blur = function () {
      _this.inputRef.current.blur();
    };

    _this.renderPlaceholder = function () {
      var _this$props = _this.props,
          prefixCls = _this$props.prefixCls,
          placeholder = _this$props.placeholder,
          searchPlaceholder = _this$props.searchPlaceholder,
          searchValue = _this$props.searchValue,
          selectorValueList = _this$props.selectorValueList;


      var currentPlaceholder = placeholder || searchPlaceholder;

      if (!currentPlaceholder) return null;

      var hidden = searchValue || selectorValueList.length;

      // [Legacy] Not remove the placeholder
      return React.createElement(
        'span',
        {
          style: {
            display: hidden ? 'none' : 'block'
          },
          onClick: _this.onPlaceholderClick,
          className: prefixCls + '-search__field__placeholder'
        },
        currentPlaceholder
      );
    };

    _this.renderSelection = function () {
      var _this$props2 = _this.props,
          selectorValueList = _this$props2.selectorValueList,
          choiceTransitionName = _this$props2.choiceTransitionName,
          prefixCls = _this$props2.prefixCls,
          onChoiceAnimationLeave = _this$props2.onChoiceAnimationLeave,
          labelInValue = _this$props2.labelInValue,
          maxTagCount = _this$props2.maxTagCount,
          maxTagPlaceholder = _this$props2.maxTagPlaceholder;
      var onMultipleSelectorRemove = _this.context.rcTreeSelect.onMultipleSelectorRemove;

      // Check if `maxTagCount` is set

      var myValueList = selectorValueList;
      if (maxTagCount >= 0) {
        myValueList = selectorValueList.slice(0, maxTagCount);
      }

      // Selector node list
      var selectedValueNodes = myValueList.map(function (_ref) {
        var label = _ref.label,
            value = _ref.value;
        return React.createElement(Selection, _extends({}, _this.props, {
          key: value || TREE_SELECT_EMPTY_VALUE_KEY,
          label: label,
          value: value,
          onRemove: onMultipleSelectorRemove
        }));
      });

      // Rest node count
      if (maxTagCount >= 0 && maxTagCount < selectorValueList.length) {
        var content = '+ ' + (selectorValueList.length - maxTagCount) + ' ...';
        if (typeof maxTagPlaceholder === 'string') {
          content = maxTagPlaceholder;
        } else if (typeof maxTagPlaceholder === 'function') {
          var restValueList = selectorValueList.slice(maxTagCount);
          content = maxTagPlaceholder(labelInValue ? restValueList : restValueList.map(function (_ref2) {
            var value = _ref2.value;
            return value;
          }));
        }

        var restNodeSelect = React.createElement(Selection, _extends({}, _this.props, {
          key: 'rc-tree-select-internal-max-tag-counter',
          label: content,
          value: null
        }));

        selectedValueNodes.push(restNodeSelect);
      }

      selectedValueNodes.push(React.createElement(
        'li',
        {
          className: prefixCls + '-search ' + prefixCls + '-search--inline',
          key: '__input'
        },
        React.createElement(SearchInput, _extends({}, _this.props, { ref: _this.inputRef, needAlign: true }))
      ));
      var className = prefixCls + '-selection__rendered';
      if (choiceTransitionName) {
        return React.createElement(
          Animate,
          {
            className: className,
            component: 'ul',
            transitionName: choiceTransitionName,
            onLeave: onChoiceAnimationLeave
          },
          selectedValueNodes
        );
      }
      return React.createElement(
        'ul',
        { className: className, role: 'menubar' },
        selectedValueNodes
      );
    };

    _this.inputRef = createRef();
    return _this;
  }

  _createClass(MultipleSelector, [{
    key: 'render',
    value: function render() {
      return React.createElement(Selector, _extends({}, this.props, {
        tabIndex: -1,
        showArrow: false,
        renderSelection: this.renderSelection,
        renderPlaceholder: this.renderPlaceholder
      }));
    }
  }]);

  return MultipleSelector;
}(React.Component);

MultipleSelector.propTypes = _extends({}, selectorPropTypes, {
  selectorValueList: PropTypes.array,
  disabled: PropTypes.bool,
  searchValue: PropTypes.string,
  labelInValue: PropTypes.bool,
  maxTagCount: PropTypes.number,
  maxTagPlaceholder: PropTypes.oneOfType([PropTypes.node, PropTypes.func]),

  onChoiceAnimationLeave: PropTypes.func
});
MultipleSelector.contextTypes = {
  rcTreeSelect: PropTypes.shape(_extends({}, multipleSelectorContextTypes, {

    onSearchInputChange: PropTypes.func
  }))
};


export default MultipleSelector;
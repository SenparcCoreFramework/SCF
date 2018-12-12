import _extends from 'babel-runtime/helpers/extends';
import _classCallCheck from 'babel-runtime/helpers/classCallCheck';
import _createClass from 'babel-runtime/helpers/createClass';
import _possibleConstructorReturn from 'babel-runtime/helpers/possibleConstructorReturn';
import _inherits from 'babel-runtime/helpers/inherits';
import React from 'react';
import generateSelector, { selectorPropTypes } from '../Base/BaseSelector';
import { toTitle, createRef } from '../util';

var Selector = generateSelector('single');

var SingleSelector = function (_React$Component) {
  _inherits(SingleSelector, _React$Component);

  function SingleSelector() {
    _classCallCheck(this, SingleSelector);

    var _this = _possibleConstructorReturn(this, (SingleSelector.__proto__ || Object.getPrototypeOf(SingleSelector)).call(this));

    _this.focus = function () {
      _this.selectorRef.current.focus();
    };

    _this.blur = function () {
      _this.selectorRef.current.blur();
    };

    _this.renderSelection = function () {
      var _this$props = _this.props,
          selectorValueList = _this$props.selectorValueList,
          placeholder = _this$props.placeholder,
          prefixCls = _this$props.prefixCls;


      var innerNode = void 0;

      if (selectorValueList.length) {
        var _selectorValueList$ = selectorValueList[0],
            label = _selectorValueList$.label,
            value = _selectorValueList$.value;

        innerNode = React.createElement(
          'span',
          {
            key: 'value',
            title: toTitle(label),
            className: prefixCls + '-selection-selected-value'
          },
          label || value
        );
      } else {
        innerNode = React.createElement(
          'span',
          {
            key: 'placeholder',
            className: prefixCls + '-selection__placeholder'
          },
          placeholder
        );
      }

      return React.createElement(
        'span',
        { className: prefixCls + '-selection__rendered' },
        innerNode
      );
    };

    _this.selectorRef = createRef();
    return _this;
  }

  _createClass(SingleSelector, [{
    key: 'render',
    value: function render() {
      return React.createElement(Selector, _extends({}, this.props, {
        ref: this.selectorRef,
        renderSelection: this.renderSelection
      }));
    }
  }]);

  return SingleSelector;
}(React.Component);

SingleSelector.propTypes = _extends({}, selectorPropTypes);


export default SingleSelector;
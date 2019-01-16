import _extends from 'babel-runtime/helpers/extends';
import _classCallCheck from 'babel-runtime/helpers/classCallCheck';
import _createClass from 'babel-runtime/helpers/createClass';
import _possibleConstructorReturn from 'babel-runtime/helpers/possibleConstructorReturn';
import _inherits from 'babel-runtime/helpers/inherits';
import React from 'react';
import PropTypes from 'prop-types';
import { toTitle, UNSELECTABLE_ATTRIBUTE, UNSELECTABLE_STYLE } from '../../util';

var Selection = function (_React$Component) {
  _inherits(Selection, _React$Component);

  function Selection() {
    var _ref;

    var _temp, _this, _ret;

    _classCallCheck(this, Selection);

    for (var _len = arguments.length, args = Array(_len), _key = 0; _key < _len; _key++) {
      args[_key] = arguments[_key];
    }

    return _ret = (_temp = (_this = _possibleConstructorReturn(this, (_ref = Selection.__proto__ || Object.getPrototypeOf(Selection)).call.apply(_ref, [this].concat(args))), _this), _this.onRemove = function (event) {
      var _this$props = _this.props,
          onRemove = _this$props.onRemove,
          value = _this$props.value;

      onRemove(event, value);

      event.stopPropagation();
    }, _temp), _possibleConstructorReturn(_this, _ret);
  }

  _createClass(Selection, [{
    key: 'render',
    value: function render() {
      var _props = this.props,
          prefixCls = _props.prefixCls,
          maxTagTextLength = _props.maxTagTextLength,
          label = _props.label,
          value = _props.value,
          onRemove = _props.onRemove,
          removeIcon = _props.removeIcon;


      var content = label || value;
      if (maxTagTextLength && typeof content === 'string' && content.length > maxTagTextLength) {
        content = content.slice(0, maxTagTextLength) + '...';
      }

      return React.createElement(
        'li',
        _extends({
          style: UNSELECTABLE_STYLE
        }, UNSELECTABLE_ATTRIBUTE, {
          role: 'menuitem',
          className: prefixCls + '-selection__choice',
          title: toTitle(label)
        }),
        onRemove && React.createElement(
          'span',
          {
            className: prefixCls + '-selection__choice__remove',
            onClick: this.onRemove
          },
          typeof removeIcon === 'function' ? React.createElement(removeIcon, _extends({}, this.props)) : removeIcon
        ),
        React.createElement(
          'span',
          { className: prefixCls + '-selection__choice__content' },
          content
        )
      );
    }
  }]);

  return Selection;
}(React.Component);

Selection.propTypes = {
  prefixCls: PropTypes.string,
  maxTagTextLength: PropTypes.number,
  onRemove: PropTypes.func,

  label: PropTypes.node,
  value: PropTypes.oneOfType([PropTypes.string, PropTypes.number]),
  removeIcon: PropTypes.oneOfType([PropTypes.node, PropTypes.func])
};


export default Selection;
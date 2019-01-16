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

var _react2 = _interopRequireDefault(_react);

var _propTypes = require('prop-types');

var _propTypes2 = _interopRequireDefault(_propTypes);

var _util = require('../../util');

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { 'default': obj }; }

var Selection = function (_React$Component) {
  (0, _inherits3['default'])(Selection, _React$Component);

  function Selection() {
    var _ref;

    var _temp, _this, _ret;

    (0, _classCallCheck3['default'])(this, Selection);

    for (var _len = arguments.length, args = Array(_len), _key = 0; _key < _len; _key++) {
      args[_key] = arguments[_key];
    }

    return _ret = (_temp = (_this = (0, _possibleConstructorReturn3['default'])(this, (_ref = Selection.__proto__ || Object.getPrototypeOf(Selection)).call.apply(_ref, [this].concat(args))), _this), _this.onRemove = function (event) {
      var _this$props = _this.props,
          onRemove = _this$props.onRemove,
          value = _this$props.value;

      onRemove(event, value);

      event.stopPropagation();
    }, _temp), (0, _possibleConstructorReturn3['default'])(_this, _ret);
  }

  (0, _createClass3['default'])(Selection, [{
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

      return _react2['default'].createElement(
        'li',
        (0, _extends3['default'])({
          style: _util.UNSELECTABLE_STYLE
        }, _util.UNSELECTABLE_ATTRIBUTE, {
          role: 'menuitem',
          className: prefixCls + '-selection__choice',
          title: (0, _util.toTitle)(label)
        }),
        onRemove && _react2['default'].createElement(
          'span',
          {
            className: prefixCls + '-selection__choice__remove',
            onClick: this.onRemove
          },
          typeof removeIcon === 'function' ? _react2['default'].createElement(removeIcon, (0, _extends3['default'])({}, this.props)) : removeIcon
        ),
        _react2['default'].createElement(
          'span',
          { className: prefixCls + '-selection__choice__content' },
          content
        )
      );
    }
  }]);
  return Selection;
}(_react2['default'].Component);

Selection.propTypes = {
  prefixCls: _propTypes2['default'].string,
  maxTagTextLength: _propTypes2['default'].number,
  onRemove: _propTypes2['default'].func,

  label: _propTypes2['default'].node,
  value: _propTypes2['default'].oneOfType([_propTypes2['default'].string, _propTypes2['default'].number]),
  removeIcon: _propTypes2['default'].oneOfType([_propTypes2['default'].node, _propTypes2['default'].func])
};
exports['default'] = Selection;
module.exports = exports['default'];
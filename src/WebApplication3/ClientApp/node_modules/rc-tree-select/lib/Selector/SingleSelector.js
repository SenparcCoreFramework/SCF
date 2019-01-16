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

var _BaseSelector = require('../Base/BaseSelector');

var _BaseSelector2 = _interopRequireDefault(_BaseSelector);

var _util = require('../util');

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { 'default': obj }; }

var Selector = (0, _BaseSelector2['default'])('single');

var SingleSelector = function (_React$Component) {
  (0, _inherits3['default'])(SingleSelector, _React$Component);

  function SingleSelector() {
    (0, _classCallCheck3['default'])(this, SingleSelector);

    var _this = (0, _possibleConstructorReturn3['default'])(this, (SingleSelector.__proto__ || Object.getPrototypeOf(SingleSelector)).call(this));

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

        innerNode = _react2['default'].createElement(
          'span',
          {
            key: 'value',
            title: (0, _util.toTitle)(label),
            className: prefixCls + '-selection-selected-value'
          },
          label || value
        );
      } else {
        innerNode = _react2['default'].createElement(
          'span',
          {
            key: 'placeholder',
            className: prefixCls + '-selection__placeholder'
          },
          placeholder
        );
      }

      return _react2['default'].createElement(
        'span',
        { className: prefixCls + '-selection__rendered' },
        innerNode
      );
    };

    _this.selectorRef = (0, _util.createRef)();
    return _this;
  }

  (0, _createClass3['default'])(SingleSelector, [{
    key: 'render',
    value: function render() {
      return _react2['default'].createElement(Selector, (0, _extends3['default'])({}, this.props, {
        ref: this.selectorRef,
        renderSelection: this.renderSelection
      }));
    }
  }]);
  return SingleSelector;
}(_react2['default'].Component);

SingleSelector.propTypes = (0, _extends3['default'])({}, _BaseSelector.selectorPropTypes);
exports['default'] = SingleSelector;
module.exports = exports['default'];
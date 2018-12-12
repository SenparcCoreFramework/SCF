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

var _BasePopup = require('../Base/BasePopup');

var _BasePopup2 = _interopRequireDefault(_BasePopup);

var _SearchInput = require('../SearchInput');

var _SearchInput2 = _interopRequireDefault(_SearchInput);

var _util = require('../util');

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { 'default': obj }; }

var SinglePopup = function (_React$Component) {
  (0, _inherits3['default'])(SinglePopup, _React$Component);

  function SinglePopup() {
    (0, _classCallCheck3['default'])(this, SinglePopup);

    var _this = (0, _possibleConstructorReturn3['default'])(this, (SinglePopup.__proto__ || Object.getPrototypeOf(SinglePopup)).call(this));

    _this.onPlaceholderClick = function () {
      _this.inputRef.current.focus();
    };

    _this.renderPlaceholder = function () {
      var _this$props = _this.props,
          searchPlaceholder = _this$props.searchPlaceholder,
          searchValue = _this$props.searchValue,
          prefixCls = _this$props.prefixCls;


      if (!searchPlaceholder) {
        return null;
      }

      return _react2['default'].createElement(
        'span',
        {
          style: {
            display: searchValue ? 'none' : 'block'
          },
          onClick: _this.onPlaceholderClick,
          className: prefixCls + '-search__field__placeholder'
        },
        searchPlaceholder
      );
    };

    _this.renderSearch = function () {
      var _this$props2 = _this.props,
          showSearch = _this$props2.showSearch,
          dropdownPrefixCls = _this$props2.dropdownPrefixCls;


      if (!showSearch) {
        return null;
      }

      return _react2['default'].createElement(
        'span',
        { className: dropdownPrefixCls + '-search' },
        _react2['default'].createElement(_SearchInput2['default'], (0, _extends3['default'])({}, _this.props, {
          ref: _this.inputRef,
          renderPlaceholder: _this.renderPlaceholder
        }))
      );
    };

    _this.inputRef = (0, _util.createRef)();
    return _this;
  }

  (0, _createClass3['default'])(SinglePopup, [{
    key: 'render',
    value: function render() {
      return _react2['default'].createElement(_BasePopup2['default'], (0, _extends3['default'])({}, this.props, {
        renderSearch: this.renderSearch
      }));
    }
  }]);
  return SinglePopup;
}(_react2['default'].Component);

SinglePopup.propTypes = (0, _extends3['default'])({}, _BasePopup2['default'].propTypes, {
  searchValue: _propTypes2['default'].string,
  showSearch: _propTypes2['default'].bool,
  dropdownPrefixCls: _propTypes2['default'].string,
  disabled: _propTypes2['default'].bool,
  searchPlaceholder: _propTypes2['default'].string
});
exports['default'] = SinglePopup;
module.exports = exports['default'];
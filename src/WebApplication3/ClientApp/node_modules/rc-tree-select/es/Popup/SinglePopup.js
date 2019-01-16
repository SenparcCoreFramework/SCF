import _extends from 'babel-runtime/helpers/extends';
import _classCallCheck from 'babel-runtime/helpers/classCallCheck';
import _createClass from 'babel-runtime/helpers/createClass';
import _possibleConstructorReturn from 'babel-runtime/helpers/possibleConstructorReturn';
import _inherits from 'babel-runtime/helpers/inherits';
import React from 'react';
import PropTypes from 'prop-types';
import BasePopup from '../Base/BasePopup';
import SearchInput from '../SearchInput';
import { createRef } from '../util';

var SinglePopup = function (_React$Component) {
  _inherits(SinglePopup, _React$Component);

  function SinglePopup() {
    _classCallCheck(this, SinglePopup);

    var _this = _possibleConstructorReturn(this, (SinglePopup.__proto__ || Object.getPrototypeOf(SinglePopup)).call(this));

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

      return React.createElement(
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

      return React.createElement(
        'span',
        { className: dropdownPrefixCls + '-search' },
        React.createElement(SearchInput, _extends({}, _this.props, {
          ref: _this.inputRef,
          renderPlaceholder: _this.renderPlaceholder
        }))
      );
    };

    _this.inputRef = createRef();
    return _this;
  }

  _createClass(SinglePopup, [{
    key: 'render',
    value: function render() {
      return React.createElement(BasePopup, _extends({}, this.props, {
        renderSearch: this.renderSearch
      }));
    }
  }]);

  return SinglePopup;
}(React.Component);

SinglePopup.propTypes = _extends({}, BasePopup.propTypes, {
  searchValue: PropTypes.string,
  showSearch: PropTypes.bool,
  dropdownPrefixCls: PropTypes.string,
  disabled: PropTypes.bool,
  searchPlaceholder: PropTypes.string
});


export default SinglePopup;
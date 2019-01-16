import _extends from 'babel-runtime/helpers/extends';
import _classCallCheck from 'babel-runtime/helpers/classCallCheck';
import _createClass from 'babel-runtime/helpers/createClass';
import _possibleConstructorReturn from 'babel-runtime/helpers/possibleConstructorReturn';
import _inherits from 'babel-runtime/helpers/inherits';
/**
 * Since search box is in different position with different mode.
 * - Single: in the popup box
 * - multiple: in the selector
 * Move the code as a SearchInput for easy management.
 */

import React from 'react';
import PropTypes from 'prop-types';
import { polyfill } from 'react-lifecycles-compat';
import { createRef } from './util';

export var searchContextTypes = {
  onSearchInputChange: PropTypes.func.isRequired
};

var SearchInput = function (_React$Component) {
  _inherits(SearchInput, _React$Component);

  function SearchInput() {
    _classCallCheck(this, SearchInput);

    var _this = _possibleConstructorReturn(this, (SearchInput.__proto__ || Object.getPrototypeOf(SearchInput)).call(this));

    _this.alignInputWidth = function () {
      _this.inputRef.current.style.width = _this.mirrorInputRef.current.clientWidth + 'px';
    };

    _this.focus = function (isDidMount) {
      if (_this.inputRef.current) {
        _this.inputRef.current.focus();
        if (isDidMount) {
          setTimeout(function () {
            _this.inputRef.current.focus();
          }, 0);
        }
      }
    };

    _this.blur = function () {
      if (_this.inputRef.current) {
        _this.inputRef.current.blur();
      }
    };

    _this.inputRef = createRef();
    _this.mirrorInputRef = createRef();
    return _this;
  }

  _createClass(SearchInput, [{
    key: 'componentDidMount',
    value: function componentDidMount() {
      var _props = this.props,
          open = _props.open,
          needAlign = _props.needAlign;

      if (needAlign) {
        this.alignInputWidth();
      }

      if (open) {
        this.focus(true);
      }
    }
  }, {
    key: 'componentDidUpdate',
    value: function componentDidUpdate(prevProps) {
      var _props2 = this.props,
          open = _props2.open,
          searchValue = _props2.searchValue,
          needAlign = _props2.needAlign;


      if (open && prevProps.open !== open) {
        this.focus();
      }

      if (needAlign && searchValue !== prevProps.searchValue) {
        this.alignInputWidth();
      }
    }

    /**
     * `scrollWidth` is not correct in IE, do the workaround.
     * ref: https://github.com/react-component/tree-select/issues/65
     */


    /**
     * Need additional timeout for focus cause parent dom is not ready when didMount trigger
     */

  }, {
    key: 'render',
    value: function render() {
      var _props3 = this.props,
          searchValue = _props3.searchValue,
          prefixCls = _props3.prefixCls,
          disabled = _props3.disabled,
          renderPlaceholder = _props3.renderPlaceholder,
          open = _props3.open,
          ariaId = _props3.ariaId;
      var _context$rcTreeSelect = this.context.rcTreeSelect,
          onSearchInputChange = _context$rcTreeSelect.onSearchInputChange,
          onSearchInputKeyDown = _context$rcTreeSelect.onSearchInputKeyDown;


      return React.createElement(
        'span',
        { className: prefixCls + '-search__field__wrap' },
        React.createElement('input', {
          type: 'text',
          ref: this.inputRef,
          onChange: onSearchInputChange,
          onKeyDown: onSearchInputKeyDown,
          value: searchValue,
          disabled: disabled,
          className: prefixCls + '-search__field',

          'aria-label': 'filter select',
          'aria-autocomplete': 'list',
          'aria-controls': open ? ariaId : undefined,
          'aria-multiline': 'false'
        }),
        React.createElement(
          'span',
          {
            ref: this.mirrorInputRef,
            className: prefixCls + '-search__field__mirror'
          },
          searchValue,
          '\xA0'
        ),
        renderPlaceholder ? renderPlaceholder() : null
      );
    }
  }]);

  return SearchInput;
}(React.Component);

SearchInput.propTypes = {
  open: PropTypes.bool,
  searchValue: PropTypes.string,
  prefixCls: PropTypes.string,
  disabled: PropTypes.bool,
  renderPlaceholder: PropTypes.func,
  needAlign: PropTypes.bool,
  ariaId: PropTypes.string
};
SearchInput.contextTypes = {
  rcTreeSelect: PropTypes.shape(_extends({}, searchContextTypes))
};


polyfill(SearchInput);

export default SearchInput;
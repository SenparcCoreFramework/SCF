'use strict';

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.searchContextTypes = undefined;

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

var _reactLifecyclesCompat = require('react-lifecycles-compat');

var _util = require('./util');

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { 'default': obj }; }

/**
 * Since search box is in different position with different mode.
 * - Single: in the popup box
 * - multiple: in the selector
 * Move the code as a SearchInput for easy management.
 */

var searchContextTypes = exports.searchContextTypes = {
  onSearchInputChange: _propTypes2['default'].func.isRequired
};

var SearchInput = function (_React$Component) {
  (0, _inherits3['default'])(SearchInput, _React$Component);

  function SearchInput() {
    (0, _classCallCheck3['default'])(this, SearchInput);

    var _this = (0, _possibleConstructorReturn3['default'])(this, (SearchInput.__proto__ || Object.getPrototypeOf(SearchInput)).call(this));

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

    _this.inputRef = (0, _util.createRef)();
    _this.mirrorInputRef = (0, _util.createRef)();
    return _this;
  }

  (0, _createClass3['default'])(SearchInput, [{
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


      return _react2['default'].createElement(
        'span',
        { className: prefixCls + '-search__field__wrap' },
        _react2['default'].createElement('input', {
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
        _react2['default'].createElement(
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
}(_react2['default'].Component);

SearchInput.propTypes = {
  open: _propTypes2['default'].bool,
  searchValue: _propTypes2['default'].string,
  prefixCls: _propTypes2['default'].string,
  disabled: _propTypes2['default'].bool,
  renderPlaceholder: _propTypes2['default'].func,
  needAlign: _propTypes2['default'].bool,
  ariaId: _propTypes2['default'].string
};
SearchInput.contextTypes = {
  rcTreeSelect: _propTypes2['default'].shape((0, _extends3['default'])({}, searchContextTypes))
};


(0, _reactLifecyclesCompat.polyfill)(SearchInput);

exports['default'] = SearchInput;
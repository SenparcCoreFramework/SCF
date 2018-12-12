'use strict';

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.multipleSelectorContextTypes = undefined;

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

var _rcAnimate = require('rc-animate');

var _rcAnimate2 = _interopRequireDefault(_rcAnimate);

var _BaseSelector = require('../../Base/BaseSelector');

var _BaseSelector2 = _interopRequireDefault(_BaseSelector);

var _SearchInput = require('../../SearchInput');

var _SearchInput2 = _interopRequireDefault(_SearchInput);

var _Selection = require('./Selection');

var _Selection2 = _interopRequireDefault(_Selection);

var _util = require('../../util');

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { 'default': obj }; }

var TREE_SELECT_EMPTY_VALUE_KEY = 'RC_TREE_SELECT_EMPTY_VALUE_KEY';

var Selector = (0, _BaseSelector2['default'])('multiple');

var multipleSelectorContextTypes = exports.multipleSelectorContextTypes = {
  onMultipleSelectorRemove: _propTypes2['default'].func.isRequired
};

var MultipleSelector = function (_React$Component) {
  (0, _inherits3['default'])(MultipleSelector, _React$Component);

  function MultipleSelector() {
    (0, _classCallCheck3['default'])(this, MultipleSelector);

    var _this = (0, _possibleConstructorReturn3['default'])(this, (MultipleSelector.__proto__ || Object.getPrototypeOf(MultipleSelector)).call(this));

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
      return _react2['default'].createElement(
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
        return _react2['default'].createElement(_Selection2['default'], (0, _extends3['default'])({}, _this.props, {
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

        var restNodeSelect = _react2['default'].createElement(_Selection2['default'], (0, _extends3['default'])({}, _this.props, {
          key: 'rc-tree-select-internal-max-tag-counter',
          label: content,
          value: null
        }));

        selectedValueNodes.push(restNodeSelect);
      }

      selectedValueNodes.push(_react2['default'].createElement(
        'li',
        {
          className: prefixCls + '-search ' + prefixCls + '-search--inline',
          key: '__input'
        },
        _react2['default'].createElement(_SearchInput2['default'], (0, _extends3['default'])({}, _this.props, { ref: _this.inputRef, needAlign: true }))
      ));
      var className = prefixCls + '-selection__rendered';
      if (choiceTransitionName) {
        return _react2['default'].createElement(
          _rcAnimate2['default'],
          {
            className: className,
            component: 'ul',
            transitionName: choiceTransitionName,
            onLeave: onChoiceAnimationLeave
          },
          selectedValueNodes
        );
      }
      return _react2['default'].createElement(
        'ul',
        { className: className, role: 'menubar' },
        selectedValueNodes
      );
    };

    _this.inputRef = (0, _util.createRef)();
    return _this;
  }

  (0, _createClass3['default'])(MultipleSelector, [{
    key: 'render',
    value: function render() {
      return _react2['default'].createElement(Selector, (0, _extends3['default'])({}, this.props, {
        tabIndex: -1,
        showArrow: false,
        renderSelection: this.renderSelection,
        renderPlaceholder: this.renderPlaceholder
      }));
    }
  }]);
  return MultipleSelector;
}(_react2['default'].Component);

MultipleSelector.propTypes = (0, _extends3['default'])({}, _BaseSelector.selectorPropTypes, {
  selectorValueList: _propTypes2['default'].array,
  disabled: _propTypes2['default'].bool,
  searchValue: _propTypes2['default'].string,
  labelInValue: _propTypes2['default'].bool,
  maxTagCount: _propTypes2['default'].number,
  maxTagPlaceholder: _propTypes2['default'].oneOfType([_propTypes2['default'].node, _propTypes2['default'].func]),

  onChoiceAnimationLeave: _propTypes2['default'].func
});
MultipleSelector.contextTypes = {
  rcTreeSelect: _propTypes2['default'].shape((0, _extends3['default'])({}, multipleSelectorContextTypes, {

    onSearchInputChange: _propTypes2['default'].func
  }))
};
exports['default'] = MultipleSelector;
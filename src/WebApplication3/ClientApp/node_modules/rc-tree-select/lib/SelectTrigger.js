'use strict';

Object.defineProperty(exports, "__esModule", {
  value: true
});

var _defineProperty2 = require('babel-runtime/helpers/defineProperty');

var _defineProperty3 = _interopRequireDefault(_defineProperty2);

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

var _rcTrigger = require('rc-trigger');

var _rcTrigger2 = _interopRequireDefault(_rcTrigger);

var _classnames = require('classnames');

var _classnames2 = _interopRequireDefault(_classnames);

var _util = require('./util');

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { 'default': obj }; }

var BUILT_IN_PLACEMENTS = {
  bottomLeft: {
    points: ['tl', 'bl'],
    offset: [0, 4],
    overflow: {
      adjustX: 0,
      adjustY: 1
    },
    ignoreShake: true
  },
  topLeft: {
    points: ['bl', 'tl'],
    offset: [0, -4],
    overflow: {
      adjustX: 0,
      adjustY: 1
    },
    ignoreShake: true
  }
};

var SelectTrigger = function (_React$Component) {
  (0, _inherits3['default'])(SelectTrigger, _React$Component);

  function SelectTrigger() {
    (0, _classCallCheck3['default'])(this, SelectTrigger);

    var _this = (0, _possibleConstructorReturn3['default'])(this, (SelectTrigger.__proto__ || Object.getPrototypeOf(SelectTrigger)).call(this));

    _this.getDropdownTransitionName = function () {
      var _this$props = _this.props,
          transitionName = _this$props.transitionName,
          animation = _this$props.animation,
          dropdownPrefixCls = _this$props.dropdownPrefixCls;

      if (!transitionName && animation) {
        return dropdownPrefixCls + '-' + animation;
      }
      return transitionName;
    };

    _this.forcePopupAlign = function () {
      var $trigger = _this.triggerRef.current;

      if ($trigger) {
        $trigger.forcePopupAlign();
      }
    };

    _this.triggerRef = (0, _util.createRef)();
    return _this;
  }

  (0, _createClass3['default'])(SelectTrigger, [{
    key: 'render',
    value: function render() {
      var _classNames;

      var _props = this.props,
          disabled = _props.disabled,
          isMultiple = _props.isMultiple,
          dropdownPopupAlign = _props.dropdownPopupAlign,
          dropdownMatchSelectWidth = _props.dropdownMatchSelectWidth,
          dropdownClassName = _props.dropdownClassName,
          dropdownStyle = _props.dropdownStyle,
          onDropdownVisibleChange = _props.onDropdownVisibleChange,
          getPopupContainer = _props.getPopupContainer,
          dropdownPrefixCls = _props.dropdownPrefixCls,
          popupElement = _props.popupElement,
          open = _props.open,
          children = _props.children;

      // TODO: [Legacy] Use new action when trigger fixed: https://github.com/react-component/trigger/pull/86

      // When false do nothing with the width
      // ref: https://github.com/ant-design/ant-design/issues/10927

      var stretch = void 0;
      if (dropdownMatchSelectWidth !== false) {
        stretch = dropdownMatchSelectWidth ? 'width' : 'minWidth';
      }

      return _react2['default'].createElement(
        _rcTrigger2['default'],
        {
          ref: this.triggerRef,
          action: disabled ? [] : ['click'],
          popupPlacement: 'bottomLeft',
          builtinPlacements: BUILT_IN_PLACEMENTS,
          popupAlign: dropdownPopupAlign,
          prefixCls: dropdownPrefixCls,
          popupTransitionName: this.getDropdownTransitionName(),
          onPopupVisibleChange: onDropdownVisibleChange,
          popup: popupElement,
          popupVisible: open,
          getPopupContainer: getPopupContainer,
          stretch: stretch,
          popupClassName: (0, _classnames2['default'])(dropdownClassName, (_classNames = {}, (0, _defineProperty3['default'])(_classNames, dropdownPrefixCls + '--multiple', isMultiple), (0, _defineProperty3['default'])(_classNames, dropdownPrefixCls + '--single', !isMultiple), _classNames)),
          popupStyle: dropdownStyle
        },
        children
      );
    }
  }]);
  return SelectTrigger;
}(_react2['default'].Component);

SelectTrigger.propTypes = {
  // Pass by outside user props
  disabled: _propTypes2['default'].bool,
  showSearch: _propTypes2['default'].bool,
  prefixCls: _propTypes2['default'].string,
  dropdownPopupAlign: _propTypes2['default'].object,
  dropdownClassName: _propTypes2['default'].string,
  dropdownStyle: _propTypes2['default'].object,
  transitionName: _propTypes2['default'].string,
  animation: _propTypes2['default'].string,
  getPopupContainer: _propTypes2['default'].func,
  children: _propTypes2['default'].node,

  dropdownMatchSelectWidth: _propTypes2['default'].bool,

  // Pass by Select
  isMultiple: _propTypes2['default'].bool,
  dropdownPrefixCls: _propTypes2['default'].string,
  onDropdownVisibleChange: _propTypes2['default'].func,
  popupElement: _propTypes2['default'].node,
  open: _propTypes2['default'].bool
};


(0, _reactLifecyclesCompat.polyfill)(SelectTrigger);

exports['default'] = SelectTrigger;
module.exports = exports['default'];
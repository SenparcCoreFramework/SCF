'use strict';

exports.__esModule = true;

var _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; };

var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; };

var _react = require('react');

var _react2 = _interopRequireDefault(_react);

var _propTypes = require('prop-types');

var _propTypes2 = _interopRequireDefault(_propTypes);

var _reactRouterDom = require('react-router-dom');

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _objectWithoutProperties(obj, keys) { var target = {}; for (var i in obj) { if (keys.indexOf(i) >= 0) continue; if (!Object.prototype.hasOwnProperty.call(obj, i)) continue; target[i] = obj[i]; } return target; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var isModifiedEvent = function isModifiedEvent(event) {
  return !!(event.metaKey || event.altKey || event.ctrlKey || event.shiftKey);
};

var LinkContainer = function (_Component) {
  _inherits(LinkContainer, _Component);

  function LinkContainer() {
    var _temp, _this, _ret;

    _classCallCheck(this, LinkContainer);

    for (var _len = arguments.length, args = Array(_len), _key = 0; _key < _len; _key++) {
      args[_key] = arguments[_key];
    }

    return _ret = (_temp = (_this = _possibleConstructorReturn(this, _Component.call.apply(_Component, [this].concat(args))), _this), _this.handleClick = function (event) {
      var _this$props = _this.props,
          children = _this$props.children,
          onClick = _this$props.onClick;


      if (children.props.onClick) {
        children.props.onClick(event);
      }

      if (onClick) {
        onClick(event);
      }

      if (!event.defaultPrevented && // onClick prevented default
      event.button === 0 && // ignore right clicks
      !isModifiedEvent(event) // ignore clicks with modifier keys
      ) {
          event.preventDefault();

          var history = _this.context.router.history;
          var _this$props2 = _this.props,
              replace = _this$props2.replace,
              to = _this$props2.to;


          if (replace) {
            history.replace(to);
          } else {
            history.push(to);
          }
        }
    }, _temp), _possibleConstructorReturn(_this, _ret);
  }

  LinkContainer.prototype.render = function render() {
    var _this2 = this;

    var _props = this.props,
        children = _props.children,
        replace = _props.replace,
        to = _props.to,
        exact = _props.exact,
        strict = _props.strict,
        activeClassName = _props.activeClassName,
        className = _props.className,
        activeStyle = _props.activeStyle,
        style = _props.style,
        getIsActive = _props.isActive,
        props = _objectWithoutProperties(_props, ['children', 'replace', 'to', 'exact', 'strict', 'activeClassName', 'className', 'activeStyle', 'style', 'isActive']);

    var href = this.context.router.history.createHref(typeof to === 'string' ? { pathname: to } : to);

    var child = _react2.default.Children.only(children);

    return _react2.default.createElement(_reactRouterDom.Route, {
      path: (typeof to === 'undefined' ? 'undefined' : _typeof(to)) === 'object' ? to.pathname : to,
      exact: exact,
      strict: strict,
      children: function children(_ref) {
        var location = _ref.location,
            match = _ref.match;

        var isActive = !!(getIsActive ? getIsActive(match, location) : match);

        return _react2.default.cloneElement(child, _extends({}, props, {
          className: [className, child.props.className, isActive ? activeClassName : null].join(' ').trim(),
          style: isActive ? _extends({}, style, activeStyle) : style,
          href: href,
          onClick: _this2.handleClick
        }));
      }
    });
  };

  return LinkContainer;
}(_react.Component);

LinkContainer.contextTypes = {
  router: _propTypes2.default.shape({
    history: _propTypes2.default.shape({
      push: _propTypes2.default.func.isRequired,
      replace: _propTypes2.default.func.isRequired,
      createHref: _propTypes2.default.func.isRequired
    }).isRequired
  }).isRequired
};
LinkContainer.propTypes = {
  children: _propTypes2.default.element.isRequired,
  onClick: _propTypes2.default.func,
  replace: _propTypes2.default.bool,
  to: _propTypes2.default.oneOfType([_propTypes2.default.string, _propTypes2.default.object]).isRequired,
  exact: _propTypes2.default.bool,
  strict: _propTypes2.default.bool,
  className: _propTypes2.default.string,
  activeClassName: _propTypes2.default.string,
  style: _propTypes2.default.object,
  activeStyle: _propTypes2.default.object,
  isActive: _propTypes2.default.func
};
LinkContainer.defaultProps = {
  replace: false,
  exact: false,
  strict: false,
  activeClassName: 'active'
};
exports.default = LinkContainer;
module.exports = exports['default'];
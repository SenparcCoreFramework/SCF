'use strict';

Object.defineProperty(exports, "__esModule", {
    value: true
});

var _extends2 = require('babel-runtime/helpers/extends');

var _extends3 = _interopRequireDefault(_extends2);

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

var React = _interopRequireWildcard(_react);

var _reactDom = require('react-dom');

var ReactDOM = _interopRequireWildcard(_reactDom);

var _rcAnimate = require('rc-animate');

var _rcAnimate2 = _interopRequireDefault(_rcAnimate);

var _classnames = require('classnames');

var _classnames2 = _interopRequireDefault(_classnames);

var _omit = require('omit.js');

var _omit2 = _interopRequireDefault(_omit);

var _reactLifecyclesCompat = require('react-lifecycles-compat');

var _icon = require('../icon');

var _icon2 = _interopRequireDefault(_icon);

var _CheckableTag = require('./CheckableTag');

var _CheckableTag2 = _interopRequireDefault(_CheckableTag);

var _wave = require('../_util/wave');

var _wave2 = _interopRequireDefault(_wave);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj['default'] = obj; return newObj; } }

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { 'default': obj }; }

var __rest = undefined && undefined.__rest || function (s, e) {
    var t = {};
    for (var p in s) {
        if (Object.prototype.hasOwnProperty.call(s, p) && e.indexOf(p) < 0) t[p] = s[p];
    }if (s != null && typeof Object.getOwnPropertySymbols === "function") for (var i = 0, p = Object.getOwnPropertySymbols(s); i < p.length; i++) {
        if (e.indexOf(p[i]) < 0) t[p[i]] = s[p[i]];
    }return t;
};

var Tag = function (_React$Component) {
    (0, _inherits3['default'])(Tag, _React$Component);

    function Tag() {
        (0, _classCallCheck3['default'])(this, Tag);

        var _this = (0, _possibleConstructorReturn3['default'])(this, (Tag.__proto__ || Object.getPrototypeOf(Tag)).apply(this, arguments));

        _this.state = {
            closing: false,
            closed: false,
            visible: true,
            mounted: false
        };
        _this.handleIconClick = function (e) {
            var onClose = _this.props.onClose;
            if (onClose) {
                onClose(e);
            }
            if (e.defaultPrevented || 'visible' in _this.props) {
                return;
            }
            _this.setState({ visible: false });
        };
        _this.close = function () {
            if (_this.state.closing || _this.state.closed) {
                return;
            }
            var dom = ReactDOM.findDOMNode(_this);
            dom.style.width = dom.getBoundingClientRect().width + 'px';
            // It's Magic Code, don't know why
            dom.style.width = dom.getBoundingClientRect().width + 'px';
            _this.setState({
                closing: true
            });
        };
        _this.show = function () {
            _this.setState({
                closed: false
            });
        };
        _this.animationEnd = function (_, existed) {
            if (!existed && !_this.state.closed) {
                _this.setState({
                    closed: true,
                    closing: false
                });
                var afterClose = _this.props.afterClose;
                if (afterClose) {
                    afterClose();
                }
            } else {
                _this.setState({
                    closed: false
                });
            }
        };
        return _this;
    }

    (0, _createClass3['default'])(Tag, [{
        key: 'componentDidUpdate',
        value: function componentDidUpdate(_prevProps, prevState) {
            if (prevState.visible && !this.state.visible) {
                this.close();
            } else if (!prevState.visible && this.state.visible) {
                this.show();
            }
        }
    }, {
        key: 'isPresetColor',
        value: function isPresetColor(color) {
            if (!color) {
                return false;
            }
            return (/^(pink|red|yellow|orange|cyan|green|blue|purple|geekblue|magenta|volcano|gold|lime)(-inverse)?$/.test(color)
            );
        }
    }, {
        key: 'render',
        value: function render() {
            var _classNames;

            var _a = this.props,
                prefixCls = _a.prefixCls,
                closable = _a.closable,
                color = _a.color,
                className = _a.className,
                children = _a.children,
                style = _a.style,
                otherProps = __rest(_a, ["prefixCls", "closable", "color", "className", "children", "style"]);
            var closeIcon = closable ? React.createElement(_icon2['default'], { type: 'close', onClick: this.handleIconClick }) : '';
            var isPresetColor = this.isPresetColor(color);
            var classString = (0, _classnames2['default'])(prefixCls, (_classNames = {}, (0, _defineProperty3['default'])(_classNames, prefixCls + '-' + color, isPresetColor), (0, _defineProperty3['default'])(_classNames, prefixCls + '-has-color', color && !isPresetColor), (0, _defineProperty3['default'])(_classNames, prefixCls + '-close', this.state.closing), _classNames), className);
            // fix https://fb.me/react-unknown-prop
            var divProps = (0, _omit2['default'])(otherProps, ['onClose', 'afterClose', 'visible']);
            var tagStyle = (0, _extends3['default'])({ backgroundColor: color && !isPresetColor ? color : null }, style);
            var tag = this.state.closed ? React.createElement('span', null) : React.createElement(
                'div',
                (0, _extends3['default'])({ 'data-show': !this.state.closing }, divProps, { className: classString, style: tagStyle }),
                children,
                closeIcon
            );
            return React.createElement(
                _wave2['default'],
                null,
                React.createElement(
                    _rcAnimate2['default'],
                    { component: '', showProp: 'data-show', transitionName: prefixCls + '-zoom', transitionAppear: true, onEnd: this.animationEnd },
                    tag
                )
            );
        }
    }], [{
        key: 'getDerivedStateFromProps',
        value: function getDerivedStateFromProps(nextProps, state) {
            if ('visible' in nextProps) {
                var newState = {
                    visible: nextProps.visible,
                    mounted: true
                };
                if (!state.mounted) {
                    newState = (0, _extends3['default'])({}, newState, { closed: !nextProps.visible });
                }
                return newState;
            }
            return null;
        }
    }]);
    return Tag;
}(React.Component);

Tag.CheckableTag = _CheckableTag2['default'];
Tag.defaultProps = {
    prefixCls: 'ant-tag',
    closable: false
};
(0, _reactLifecyclesCompat.polyfill)(Tag);
exports['default'] = Tag;
module.exports = exports['default'];
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

var React = _interopRequireWildcard(_react);

var _propTypes = require('prop-types');

var PropTypes = _interopRequireWildcard(_propTypes);

var _rcDrawer = require('rc-drawer');

var _rcDrawer2 = _interopRequireDefault(_rcDrawer);

var _createReactContext = require('create-react-context');

var _createReactContext2 = _interopRequireDefault(_createReactContext);

var _warning = require('warning');

var _warning2 = _interopRequireDefault(_warning);

var _classnames = require('classnames');

var _classnames2 = _interopRequireDefault(_classnames);

var _icon = require('../icon');

var _icon2 = _interopRequireDefault(_icon);

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

var DrawerContext = (0, _createReactContext2['default'])(null);

var Drawer = function (_React$Component) {
    (0, _inherits3['default'])(Drawer, _React$Component);

    function Drawer() {
        (0, _classCallCheck3['default'])(this, Drawer);

        var _this = (0, _possibleConstructorReturn3['default'])(this, (Drawer.__proto__ || Object.getPrototypeOf(Drawer)).apply(this, arguments));

        _this.state = {
            push: false
        };
        _this.close = function (e) {
            if (_this.props.visible !== undefined) {
                if (_this.props.onClose) {
                    _this.props.onClose(e);
                }
                return;
            }
        };
        _this.onMaskClick = function (e) {
            if (!_this.props.maskClosable) {
                return;
            }
            _this.close(e);
        };
        _this.push = function () {
            _this.setState({
                push: true
            });
        };
        _this.pull = function () {
            _this.setState({
                push: false
            });
        };
        _this.onDestoryTransitionEnd = function () {
            var isDestroyOnClose = _this.getDestoryOnClose();
            if (!isDestroyOnClose) {
                return;
            }
            if (!_this.props.visible) {
                _this.destoryClose = true;
                _this.forceUpdate();
            }
        };
        _this.getDestoryOnClose = function () {
            return _this.props.destroyOnClose && !_this.props.visible;
        };
        // get drawar push width or height
        _this.getPushTransform = function (placement) {
            if (placement === 'left' || placement === 'right') {
                return 'translateX(' + (placement === 'left' ? 180 : -180) + 'px)';
            }
            if (placement === 'top' || placement === 'bottom') {
                return 'translateY(' + (placement === 'top' ? 180 : -180) + 'px)';
            }
        };
        // render drawer body dom
        _this.renderBody = function () {
            if (_this.destoryClose && !_this.props.visible) {
                return null;
            }
            _this.destoryClose = false;
            var placement = _this.props.placement;

            var containerStyle = placement === 'left' || placement === 'right' ? {
                overflow: 'auto',
                height: '100%'
            } : {};
            var isDestroyOnClose = _this.getDestoryOnClose();
            if (isDestroyOnClose) {
                // Increase the opacity transition, delete children after closing.
                containerStyle.opacity = 0;
                containerStyle.transition = 'opacity .3s';
            }
            var _this$props = _this.props,
                prefixCls = _this$props.prefixCls,
                title = _this$props.title,
                closable = _this$props.closable;
            // is have header dom

            var header = void 0;
            if (title) {
                header = React.createElement(
                    'div',
                    { className: prefixCls + '-header' },
                    React.createElement(
                        'div',
                        { className: prefixCls + '-title' },
                        title
                    )
                );
            }
            // is have closer button
            var closer = void 0;
            if (closable) {
                closer = React.createElement(
                    'button',
                    { onClick: _this.close, 'aria-label': 'Close', className: prefixCls + '-close' },
                    React.createElement(
                        'span',
                        { className: prefixCls + '-close-x' },
                        React.createElement(_icon2['default'], { type: 'close' })
                    )
                );
            }
            return React.createElement(
                'div',
                { className: prefixCls + '-wrapper-body', style: containerStyle, onTransitionEnd: _this.onDestoryTransitionEnd },
                header,
                closer,
                React.createElement(
                    'div',
                    { className: prefixCls + '-body', style: _this.props.style },
                    _this.props.children
                )
            );
        };
        _this.getRcDrawerStyle = function () {
            var _this$props2 = _this.props,
                zIndex = _this$props2.zIndex,
                placement = _this$props2.placement,
                maskStyle = _this$props2.maskStyle;

            return _this.state.push ? (0, _extends3['default'])({}, maskStyle, { zIndex: zIndex, transform: _this.getPushTransform(placement) }) : (0, _extends3['default'])({}, maskStyle, { zIndex: zIndex });
        };
        // render Provider for Multi-level drawe
        _this.renderProvider = function (value) {
            var _a = _this.props,
                zIndex = _a.zIndex,
                style = _a.style,
                placement = _a.placement,
                className = _a.className,
                wrapClassName = _a.wrapClassName,
                width = _a.width,
                height = _a.height,
                rest = __rest(_a, ["zIndex", "style", "placement", "className", "wrapClassName", "width", "height"]);
            (0, _warning2['default'])(wrapClassName === undefined, 'wrapClassName is deprecated, please use className instead.');
            var haveMask = rest.mask ? '' : 'no-mask';
            _this.parentDrawer = value;
            var offsetStyle = {};
            if (placement === 'left' || placement === 'right') {
                offsetStyle.width = width;
            } else {
                offsetStyle.height = height;
            }
            return React.createElement(
                DrawerContext.Provider,
                { value: _this },
                React.createElement(
                    _rcDrawer2['default'],
                    (0, _extends3['default'])({ handler: false }, rest, offsetStyle, { open: _this.props.visible, onMaskClick: _this.onMaskClick, showMask: _this.props.mask, placement: placement, style: _this.getRcDrawerStyle(), className: (0, _classnames2['default'])(wrapClassName, className, haveMask) }),
                    _this.renderBody()
                )
            );
        };
        return _this;
    }

    (0, _createClass3['default'])(Drawer, [{
        key: 'componentDidUpdate',
        value: function componentDidUpdate(preProps) {
            if (preProps.visible !== this.props.visible && this.parentDrawer) {
                if (this.props.visible) {
                    this.parentDrawer.push();
                } else {
                    this.parentDrawer.pull();
                }
            }
        }
    }, {
        key: 'render',
        value: function render() {
            return React.createElement(
                DrawerContext.Consumer,
                null,
                this.renderProvider
            );
        }
    }]);
    return Drawer;
}(React.Component);

exports['default'] = Drawer;

Drawer.propTypes = {
    closable: PropTypes.bool,
    destroyOnClose: PropTypes.bool,
    getContainer: PropTypes.oneOfType([PropTypes.string, PropTypes.object, PropTypes.func, PropTypes.bool]),
    maskClosable: PropTypes.bool,
    mask: PropTypes.bool,
    maskStyle: PropTypes.object,
    style: PropTypes.object,
    title: PropTypes.node,
    visible: PropTypes.bool,
    width: PropTypes.oneOfType([PropTypes.string, PropTypes.number]),
    zIndex: PropTypes.number,
    prefixCls: PropTypes.string,
    placement: PropTypes.string,
    onClose: PropTypes.func,
    className: PropTypes.string
};
Drawer.defaultProps = {
    prefixCls: 'ant-drawer',
    width: 256,
    height: 256,
    closable: true,
    placement: 'right',
    maskClosable: true,
    mask: true,
    level: null
};
module.exports = exports['default'];
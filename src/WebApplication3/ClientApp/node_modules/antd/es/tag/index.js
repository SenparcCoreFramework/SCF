import _extends from 'babel-runtime/helpers/extends';
import _defineProperty from 'babel-runtime/helpers/defineProperty';
import _classCallCheck from 'babel-runtime/helpers/classCallCheck';
import _createClass from 'babel-runtime/helpers/createClass';
import _possibleConstructorReturn from 'babel-runtime/helpers/possibleConstructorReturn';
import _inherits from 'babel-runtime/helpers/inherits';
var __rest = this && this.__rest || function (s, e) {
    var t = {};
    for (var p in s) {
        if (Object.prototype.hasOwnProperty.call(s, p) && e.indexOf(p) < 0) t[p] = s[p];
    }if (s != null && typeof Object.getOwnPropertySymbols === "function") for (var i = 0, p = Object.getOwnPropertySymbols(s); i < p.length; i++) {
        if (e.indexOf(p[i]) < 0) t[p[i]] = s[p[i]];
    }return t;
};
import * as React from 'react';
import * as ReactDOM from 'react-dom';
import Animate from 'rc-animate';
import classNames from 'classnames';
import omit from 'omit.js';
import { polyfill } from 'react-lifecycles-compat';
import Icon from '../icon';
import CheckableTag from './CheckableTag';
import Wave from '../_util/wave';

var Tag = function (_React$Component) {
    _inherits(Tag, _React$Component);

    function Tag() {
        _classCallCheck(this, Tag);

        var _this = _possibleConstructorReturn(this, (Tag.__proto__ || Object.getPrototypeOf(Tag)).apply(this, arguments));

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

    _createClass(Tag, [{
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
            var closeIcon = closable ? React.createElement(Icon, { type: 'close', onClick: this.handleIconClick }) : '';
            var isPresetColor = this.isPresetColor(color);
            var classString = classNames(prefixCls, (_classNames = {}, _defineProperty(_classNames, prefixCls + '-' + color, isPresetColor), _defineProperty(_classNames, prefixCls + '-has-color', color && !isPresetColor), _defineProperty(_classNames, prefixCls + '-close', this.state.closing), _classNames), className);
            // fix https://fb.me/react-unknown-prop
            var divProps = omit(otherProps, ['onClose', 'afterClose', 'visible']);
            var tagStyle = _extends({ backgroundColor: color && !isPresetColor ? color : null }, style);
            var tag = this.state.closed ? React.createElement('span', null) : React.createElement(
                'div',
                _extends({ 'data-show': !this.state.closing }, divProps, { className: classString, style: tagStyle }),
                children,
                closeIcon
            );
            return React.createElement(
                Wave,
                null,
                React.createElement(
                    Animate,
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
                    newState = _extends({}, newState, { closed: !nextProps.visible });
                }
                return newState;
            }
            return null;
        }
    }]);

    return Tag;
}(React.Component);

Tag.CheckableTag = CheckableTag;
Tag.defaultProps = {
    prefixCls: 'ant-tag',
    closable: false
};
polyfill(Tag);
export default Tag;
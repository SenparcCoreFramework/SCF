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
import * as PropTypes from 'prop-types';
import classNames from 'classnames';
import Animate from 'rc-animate';
import omit from 'omit.js';
// Render indicator
var defaultIndicator = null;
function renderIndicator(props) {
    var prefixCls = props.prefixCls,
        indicator = props.indicator;

    var dotClassName = prefixCls + '-dot';
    if (React.isValidElement(indicator)) {
        return React.cloneElement(indicator, {
            className: classNames(indicator.props.className, dotClassName)
        });
    }
    if (React.isValidElement(defaultIndicator)) {
        return React.cloneElement(defaultIndicator, {
            className: classNames(defaultIndicator.props.className, dotClassName)
        });
    }
    return React.createElement(
        'span',
        { className: classNames(dotClassName, prefixCls + '-dot-spin') },
        React.createElement('i', null),
        React.createElement('i', null),
        React.createElement('i', null),
        React.createElement('i', null)
    );
}
function shouldDelay(spinning, delay) {
    return !!spinning && !!delay && !isNaN(Number(delay));
}

var Spin = function (_React$Component) {
    _inherits(Spin, _React$Component);

    function Spin(props) {
        _classCallCheck(this, Spin);

        var _this = _possibleConstructorReturn(this, (Spin.__proto__ || Object.getPrototypeOf(Spin)).call(this, props));

        _this.delayUpdateSpinning = function () {
            var spinning = _this.props.spinning;

            if (_this.state.spinning !== spinning) {
                _this.setState({ spinning: spinning });
            }
        };
        var spinning = props.spinning,
            delay = props.delay;

        _this.state = {
            spinning: spinning && !shouldDelay(spinning, delay)
        };
        return _this;
    }

    _createClass(Spin, [{
        key: 'isNestedPattern',
        value: function isNestedPattern() {
            return !!(this.props && this.props.children);
        }
    }, {
        key: 'componentDidMount',
        value: function componentDidMount() {
            var _props = this.props,
                spinning = _props.spinning,
                delay = _props.delay;

            if (shouldDelay(spinning, delay)) {
                this.delayTimeout = window.setTimeout(this.delayUpdateSpinning, delay);
            }
        }
    }, {
        key: 'componentWillUnmount',
        value: function componentWillUnmount() {
            if (this.debounceTimeout) {
                clearTimeout(this.debounceTimeout);
            }
            if (this.delayTimeout) {
                clearTimeout(this.delayTimeout);
            }
        }
    }, {
        key: 'componentDidUpdate',
        value: function componentDidUpdate() {
            var _this2 = this;

            var currentSpinning = this.state.spinning;
            var spinning = this.props.spinning;
            if (currentSpinning === spinning) {
                return;
            }
            var delay = this.props.delay;

            if (this.debounceTimeout) {
                clearTimeout(this.debounceTimeout);
            }
            if (currentSpinning && !spinning) {
                this.debounceTimeout = window.setTimeout(function () {
                    return _this2.setState({ spinning: spinning });
                }, 200);
                if (this.delayTimeout) {
                    clearTimeout(this.delayTimeout);
                }
            } else {
                if (shouldDelay(spinning, delay)) {
                    if (this.delayTimeout) {
                        clearTimeout(this.delayTimeout);
                    }
                    this.delayTimeout = window.setTimeout(this.delayUpdateSpinning, delay);
                } else {
                    this.setState({ spinning: spinning });
                }
            }
        }
    }, {
        key: 'render',
        value: function render() {
            var _classNames;

            var _a = this.props,
                className = _a.className,
                size = _a.size,
                prefixCls = _a.prefixCls,
                tip = _a.tip,
                wrapperClassName = _a.wrapperClassName,
                restProps = __rest(_a, ["className", "size", "prefixCls", "tip", "wrapperClassName"]);var spinning = this.state.spinning;

            var spinClassName = classNames(prefixCls, (_classNames = {}, _defineProperty(_classNames, prefixCls + '-sm', size === 'small'), _defineProperty(_classNames, prefixCls + '-lg', size === 'large'), _defineProperty(_classNames, prefixCls + '-spinning', spinning), _defineProperty(_classNames, prefixCls + '-show-text', !!tip), _classNames), className);
            // fix https://fb.me/react-unknown-prop
            var divProps = omit(restProps, ['spinning', 'delay', 'indicator']);
            var spinElement = React.createElement(
                'div',
                _extends({}, divProps, { className: spinClassName }),
                renderIndicator(this.props),
                tip ? React.createElement(
                    'div',
                    { className: prefixCls + '-text' },
                    tip
                ) : null
            );
            if (this.isNestedPattern()) {
                var _classNames2;

                var animateClassName = prefixCls + '-nested-loading';
                if (wrapperClassName) {
                    animateClassName += ' ' + wrapperClassName;
                }
                var containerClassName = classNames((_classNames2 = {}, _defineProperty(_classNames2, prefixCls + '-container', true), _defineProperty(_classNames2, prefixCls + '-blur', spinning), _classNames2));
                return React.createElement(
                    Animate,
                    _extends({}, divProps, { component: 'div', className: animateClassName, style: null, transitionName: 'fade' }),
                    spinning && React.createElement(
                        'div',
                        { key: 'loading' },
                        spinElement
                    ),
                    React.createElement(
                        'div',
                        { className: containerClassName, key: 'container' },
                        this.props.children
                    )
                );
            }
            return spinElement;
        }
    }], [{
        key: 'setDefaultIndicator',
        value: function setDefaultIndicator(indicator) {
            defaultIndicator = indicator;
        }
    }]);

    return Spin;
}(React.Component);

Spin.defaultProps = {
    prefixCls: 'ant-spin',
    spinning: true,
    size: 'default',
    wrapperClassName: ''
};
Spin.propTypes = {
    prefixCls: PropTypes.string,
    className: PropTypes.string,
    spinning: PropTypes.bool,
    size: PropTypes.oneOf(['small', 'default', 'large']),
    wrapperClassName: PropTypes.string,
    indicator: PropTypes.node
};
export default Spin;
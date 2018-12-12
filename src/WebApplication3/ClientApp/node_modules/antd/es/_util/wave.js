import _classCallCheck from 'babel-runtime/helpers/classCallCheck';
import _createClass from 'babel-runtime/helpers/createClass';
import _possibleConstructorReturn from 'babel-runtime/helpers/possibleConstructorReturn';
import _inherits from 'babel-runtime/helpers/inherits';
import * as React from 'react';
import { findDOMNode } from 'react-dom';
import TransitionEvents from 'css-animation/es/Event';
import raf from '../_util/raf';
var styleForPesudo = void 0;
// Where el is the DOM element you'd like to test for visibility
function isHidden(element) {
    if (process.env.NODE_ENV === 'test') {
        return false;
    }
    return !element || element.offsetParent === null;
}

var Wave = function (_React$Component) {
    _inherits(Wave, _React$Component);

    function Wave() {
        _classCallCheck(this, Wave);

        var _this = _possibleConstructorReturn(this, (Wave.__proto__ || Object.getPrototypeOf(Wave)).apply(this, arguments));

        _this.animationStart = false;
        _this.destroy = false;
        _this.onClick = function (node, waveColor) {
            if (!node || isHidden(node) || node.className.indexOf('-leave') >= 0) {
                return;
            }
            var insertExtraNode = _this.props.insertExtraNode;

            _this.extraNode = document.createElement('div');
            var extraNode = _this.extraNode;
            extraNode.className = 'ant-click-animating-node';
            var attributeName = _this.getAttributeName();
            node.removeAttribute(attributeName);
            node.setAttribute(attributeName, 'true');
            // Not white or transparnt or grey
            styleForPesudo = styleForPesudo || document.createElement('style');
            if (waveColor && waveColor !== '#ffffff' && waveColor !== 'rgb(255, 255, 255)' && _this.isNotGrey(waveColor) && !/rgba\(\d*, \d*, \d*, 0\)/.test(waveColor) && // any transparent rgba color
            waveColor !== 'transparent') {
                extraNode.style.borderColor = waveColor;
                styleForPesudo.innerHTML = '[ant-click-animating-without-extra-node]:after { border-color: ' + waveColor + '; }';
                if (!document.body.contains(styleForPesudo)) {
                    document.body.appendChild(styleForPesudo);
                }
            }
            if (insertExtraNode) {
                node.appendChild(extraNode);
            }
            TransitionEvents.addStartEventListener(node, _this.onTransitionStart);
            TransitionEvents.addEndEventListener(node, _this.onTransitionEnd);
        };
        _this.bindAnimationEvent = function (node) {
            if (!node || !node.getAttribute || node.getAttribute('disabled') || node.className.indexOf('disabled') >= 0) {
                return;
            }
            var onClick = function onClick(e) {
                // Fix radio button click twice
                if (e.target.tagName === 'INPUT' || isHidden(e.target)) {
                    return;
                }
                _this.resetEffect(node);
                // Get wave color from target
                var waveColor = getComputedStyle(node).getPropertyValue('border-top-color') || // Firefox Compatible
                getComputedStyle(node).getPropertyValue('border-color') || getComputedStyle(node).getPropertyValue('background-color');
                _this.clickWaveTimeoutId = window.setTimeout(function () {
                    return _this.onClick(node, waveColor);
                }, 0);
                raf.cancel(_this.animationStartId);
                _this.animationStart = true;
                // Render to trigger transition event cost 3 frames. Let's delay 10 frames to reset this.
                _this.animationStartId = raf(function () {
                    _this.animationStart = false;
                }, 10);
            };
            node.addEventListener('click', onClick, true);
            return {
                cancel: function cancel() {
                    node.removeEventListener('click', onClick, true);
                }
            };
        };
        _this.onTransitionStart = function (e) {
            if (_this.destroy) return;
            var node = findDOMNode(_this);
            if (!e || e.target !== node) {
                return;
            }
            if (!_this.animationStart) {
                _this.resetEffect(node);
            }
        };
        _this.onTransitionEnd = function (e) {
            if (!e || e.animationName !== 'fadeEffect') {
                return;
            }
            _this.resetEffect(e.target);
        };
        return _this;
    }

    _createClass(Wave, [{
        key: 'isNotGrey',
        value: function isNotGrey(color) {
            var match = (color || '').match(/rgba?\((\d*), (\d*), (\d*)(, [\.\d]*)?\)/);
            if (match && match[1] && match[2] && match[3]) {
                return !(match[1] === match[2] && match[2] === match[3]);
            }
            return true;
        }
    }, {
        key: 'getAttributeName',
        value: function getAttributeName() {
            var insertExtraNode = this.props.insertExtraNode;

            return insertExtraNode ? 'ant-click-animating' : 'ant-click-animating-without-extra-node';
        }
    }, {
        key: 'resetEffect',
        value: function resetEffect(node) {
            if (!node || node === this.extraNode || !(node instanceof Element)) {
                return;
            }
            var insertExtraNode = this.props.insertExtraNode;

            var attributeName = this.getAttributeName();
            node.removeAttribute(attributeName);
            this.removeExtraStyleNode();
            if (insertExtraNode && this.extraNode && node.contains(this.extraNode)) {
                node.removeChild(this.extraNode);
            }
            TransitionEvents.removeStartEventListener(node, this.onTransitionStart);
            TransitionEvents.removeEndEventListener(node, this.onTransitionEnd);
        }
    }, {
        key: 'removeExtraStyleNode',
        value: function removeExtraStyleNode() {
            if (styleForPesudo) {
                styleForPesudo.innerHTML = '';
            }
        }
    }, {
        key: 'componentDidMount',
        value: function componentDidMount() {
            var node = findDOMNode(this);
            if (node.nodeType !== 1) {
                return;
            }
            this.instance = this.bindAnimationEvent(node);
        }
    }, {
        key: 'componentWillUnmount',
        value: function componentWillUnmount() {
            if (this.instance) {
                this.instance.cancel();
            }
            if (this.clickWaveTimeoutId) {
                clearTimeout(this.clickWaveTimeoutId);
            }
            this.destroy = true;
        }
    }, {
        key: 'render',
        value: function render() {
            return this.props.children;
        }
    }]);

    return Wave;
}(React.Component);

export default Wave;
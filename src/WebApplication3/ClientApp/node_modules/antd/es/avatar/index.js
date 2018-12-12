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
import Icon from '../icon';
import classNames from 'classnames';

var Avatar = function (_React$Component) {
    _inherits(Avatar, _React$Component);

    function Avatar() {
        _classCallCheck(this, Avatar);

        var _this = _possibleConstructorReturn(this, (Avatar.__proto__ || Object.getPrototypeOf(Avatar)).apply(this, arguments));

        _this.state = {
            scale: 1,
            isImgExist: true
        };
        _this.setScale = function () {
            var childrenNode = _this.avatarChildren;
            if (childrenNode) {
                var childrenWidth = childrenNode.offsetWidth;
                var avatarNode = ReactDOM.findDOMNode(_this);
                var avatarWidth = avatarNode.getBoundingClientRect().width;
                // add 4px gap for each side to get better performance
                if (avatarWidth - 8 < childrenWidth) {
                    _this.setState({
                        scale: (avatarWidth - 8) / childrenWidth
                    });
                } else {
                    _this.setState({
                        scale: 1
                    });
                }
            }
        };
        _this.handleImgLoadError = function () {
            var onError = _this.props.onError;

            var errorFlag = onError ? onError() : undefined;
            if (errorFlag !== false) {
                _this.setState({ isImgExist: false });
            }
        };
        return _this;
    }

    _createClass(Avatar, [{
        key: 'componentDidMount',
        value: function componentDidMount() {
            this.setScale();
        }
    }, {
        key: 'componentDidUpdate',
        value: function componentDidUpdate(prevProps, prevState) {
            if (prevProps.children !== this.props.children || prevState.scale !== this.state.scale && this.state.scale === 1 || prevState.isImgExist !== this.state.isImgExist) {
                this.setScale();
            }
        }
    }, {
        key: 'render',
        value: function render() {
            var _classNames,
                _classNames2,
                _this2 = this;

            var _a = this.props,
                prefixCls = _a.prefixCls,
                shape = _a.shape,
                size = _a.size,
                src = _a.src,
                srcSet = _a.srcSet,
                icon = _a.icon,
                className = _a.className,
                alt = _a.alt,
                others = __rest(_a, ["prefixCls", "shape", "size", "src", "srcSet", "icon", "className", "alt"]);var _state = this.state,
                isImgExist = _state.isImgExist,
                scale = _state.scale;

            var sizeCls = classNames((_classNames = {}, _defineProperty(_classNames, prefixCls + '-lg', size === 'large'), _defineProperty(_classNames, prefixCls + '-sm', size === 'small'), _classNames));
            var classString = classNames(prefixCls, className, sizeCls, (_classNames2 = {}, _defineProperty(_classNames2, prefixCls + '-' + shape, shape), _defineProperty(_classNames2, prefixCls + '-image', src && isImgExist), _defineProperty(_classNames2, prefixCls + '-icon', icon), _classNames2));
            var sizeStyle = typeof size === 'number' ? {
                width: size,
                height: size,
                lineHeight: size + 'px',
                fontSize: icon ? size / 2 : 18
            } : {};
            var children = this.props.children;
            if (src && isImgExist) {
                children = React.createElement('img', { src: src, srcSet: srcSet, onError: this.handleImgLoadError, alt: alt });
            } else if (icon) {
                children = React.createElement(Icon, { type: icon });
            } else {
                var childrenNode = this.avatarChildren;
                if (childrenNode || scale !== 1) {
                    var transformString = 'scale(' + scale + ') translateX(-50%)';
                    var childrenStyle = {
                        msTransform: transformString,
                        WebkitTransform: transformString,
                        transform: transformString
                    };
                    var sizeChildrenStyle = typeof size === 'number' ? {
                        lineHeight: size + 'px'
                    } : {};
                    children = React.createElement(
                        'span',
                        { className: prefixCls + '-string', ref: function ref(span) {
                                return _this2.avatarChildren = span;
                            }, style: _extends({}, sizeChildrenStyle, childrenStyle) },
                        children
                    );
                } else {
                    children = React.createElement(
                        'span',
                        { className: prefixCls + '-string', ref: function ref(span) {
                                return _this2.avatarChildren = span;
                            } },
                        children
                    );
                }
            }
            return React.createElement(
                'span',
                _extends({}, others, { style: _extends({}, sizeStyle, others.style), className: classString }),
                children
            );
        }
    }]);

    return Avatar;
}(React.Component);

export default Avatar;

Avatar.defaultProps = {
    prefixCls: 'ant-avatar',
    shape: 'circle',
    size: 'default'
};
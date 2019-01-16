import _extends from 'babel-runtime/helpers/extends';
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
import classNames from 'classnames';

var Comment = function (_React$Component) {
    _inherits(Comment, _React$Component);

    function Comment() {
        _classCallCheck(this, Comment);

        var _this = _possibleConstructorReturn(this, (Comment.__proto__ || Object.getPrototypeOf(Comment)).apply(this, arguments));

        _this.renderNested = function (children) {
            var prefixCls = _this.props.prefixCls;

            return React.createElement(
                'div',
                { className: classNames(prefixCls + '-nested') },
                children
            );
        };
        return _this;
    }

    _createClass(Comment, [{
        key: 'getAction',
        value: function getAction(actions) {
            if (!actions || !actions.length) {
                return null;
            }
            var actionList = actions.map(function (action, index) {
                return React.createElement(
                    'li',
                    { key: 'action-' + index },
                    action
                );
            });
            return actionList;
        }
    }, {
        key: 'render',
        value: function render() {
            var _a = this.props,
                actions = _a.actions,
                author = _a.author,
                avatar = _a.avatar,
                children = _a.children,
                className = _a.className,
                content = _a.content,
                prefixCls = _a.prefixCls,
                style = _a.style,
                datetime = _a.datetime,
                otherProps = __rest(_a, ["actions", "author", "avatar", "children", "className", "content", "prefixCls", "style", "datetime"]);
            var avatarDom = React.createElement(
                'div',
                { className: prefixCls + '-avatar' },
                typeof avatar === 'string' ? React.createElement('img', { src: avatar }) : avatar
            );
            var actionDom = actions && actions.length ? React.createElement(
                'ul',
                { className: prefixCls + '-actions' },
                this.getAction(actions)
            ) : null;
            var authorContent = React.createElement(
                'div',
                { className: prefixCls + '-content-author' },
                author && React.createElement(
                    'span',
                    { className: prefixCls + '-content-author-name' },
                    author
                ),
                datetime && React.createElement(
                    'span',
                    { className: prefixCls + '-content-author-time' },
                    datetime
                )
            );
            var contentDom = React.createElement(
                'div',
                { className: prefixCls + '-content' },
                authorContent,
                React.createElement(
                    'div',
                    { className: prefixCls + '-content-detail' },
                    content
                ),
                actionDom
            );
            var comment = React.createElement(
                'div',
                { className: prefixCls + '-inner' },
                avatarDom,
                contentDom
            );
            return React.createElement(
                'div',
                _extends({}, otherProps, { className: classNames(prefixCls, className), style: style }),
                comment,
                children ? this.renderNested(children) : null
            );
        }
    }]);

    return Comment;
}(React.Component);

export default Comment;

Comment.defaultProps = {
    prefixCls: 'ant-comment'
};
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

var _classnames = require('classnames');

var _classnames2 = _interopRequireDefault(_classnames);

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

var Comment = function (_React$Component) {
    (0, _inherits3['default'])(Comment, _React$Component);

    function Comment() {
        (0, _classCallCheck3['default'])(this, Comment);

        var _this = (0, _possibleConstructorReturn3['default'])(this, (Comment.__proto__ || Object.getPrototypeOf(Comment)).apply(this, arguments));

        _this.renderNested = function (children) {
            var prefixCls = _this.props.prefixCls;

            return React.createElement(
                'div',
                { className: (0, _classnames2['default'])(prefixCls + '-nested') },
                children
            );
        };
        return _this;
    }

    (0, _createClass3['default'])(Comment, [{
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
                (0, _extends3['default'])({}, otherProps, { className: (0, _classnames2['default'])(prefixCls, className), style: style }),
                comment,
                children ? this.renderNested(children) : null
            );
        }
    }]);
    return Comment;
}(React.Component);

exports['default'] = Comment;

Comment.defaultProps = {
    prefixCls: 'ant-comment'
};
module.exports = exports['default'];
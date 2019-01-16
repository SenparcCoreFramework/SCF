'use strict';

Object.defineProperty(exports, "__esModule", {
    value: true
});

var _defineProperty2 = require('babel-runtime/helpers/defineProperty');

var _defineProperty3 = _interopRequireDefault(_defineProperty2);

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

var _typeof2 = require('babel-runtime/helpers/typeof');

var _typeof3 = _interopRequireDefault(_typeof2);

var _react = require('react');

var React = _interopRequireWildcard(_react);

var _classnames = require('classnames');

var _classnames2 = _interopRequireDefault(_classnames);

var _Avatar = require('./Avatar');

var _Avatar2 = _interopRequireDefault(_Avatar);

var _Title = require('./Title');

var _Title2 = _interopRequireDefault(_Title);

var _Paragraph = require('./Paragraph');

var _Paragraph2 = _interopRequireDefault(_Paragraph);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj['default'] = obj; return newObj; } }

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { 'default': obj }; }

function getComponentProps(prop) {
    if (prop && (typeof prop === 'undefined' ? 'undefined' : (0, _typeof3['default'])(prop)) === 'object') {
        return prop;
    }
    return {};
}
function getAvatarBasicProps(hasTitle, hasParagraph) {
    if (hasTitle && !hasParagraph) {
        return { shape: 'square' };
    }
    return { shape: 'circle' };
}
function getTitleBasicProps(hasAvatar, hasParagraph) {
    if (!hasAvatar && hasParagraph) {
        return { width: '38%' };
    }
    if (hasAvatar && hasParagraph) {
        return { width: '50%' };
    }
    return {};
}
function getParagraphBasicProps(hasAvatar, hasTitle) {
    var basicProps = {};
    // Width
    if (!hasAvatar || !hasTitle) {
        basicProps.width = '61%';
    }
    // Rows
    if (!hasAvatar && hasTitle) {
        basicProps.rows = 3;
    } else {
        basicProps.rows = 2;
    }
    return basicProps;
}

var Skeleton = function (_React$Component) {
    (0, _inherits3['default'])(Skeleton, _React$Component);

    function Skeleton() {
        (0, _classCallCheck3['default'])(this, Skeleton);
        return (0, _possibleConstructorReturn3['default'])(this, (Skeleton.__proto__ || Object.getPrototypeOf(Skeleton)).apply(this, arguments));
    }

    (0, _createClass3['default'])(Skeleton, [{
        key: 'render',
        value: function render() {
            var _props = this.props,
                loading = _props.loading,
                prefixCls = _props.prefixCls,
                className = _props.className,
                children = _props.children,
                avatar = _props.avatar,
                title = _props.title,
                paragraph = _props.paragraph,
                active = _props.active;

            if (loading || !('loading' in this.props)) {
                var _classNames;

                var hasAvatar = !!avatar;
                var hasTitle = !!title;
                var hasParagraph = !!paragraph;
                // Avatar
                var avatarNode = void 0;
                if (hasAvatar) {
                    var avatarProps = (0, _extends3['default'])({}, getAvatarBasicProps(hasTitle, hasParagraph), getComponentProps(avatar));
                    avatarNode = React.createElement(
                        'div',
                        { className: prefixCls + '-header' },
                        React.createElement(_Avatar2['default'], avatarProps)
                    );
                }
                var contentNode = void 0;
                if (hasTitle || hasParagraph) {
                    // Title
                    var $title = void 0;
                    if (hasTitle) {
                        var titleProps = (0, _extends3['default'])({}, getTitleBasicProps(hasAvatar, hasParagraph), getComponentProps(title));
                        $title = React.createElement(_Title2['default'], titleProps);
                    }
                    // Paragraph
                    var paragraphNode = void 0;
                    if (hasParagraph) {
                        var paragraphProps = (0, _extends3['default'])({}, getParagraphBasicProps(hasAvatar, hasTitle), getComponentProps(paragraph));
                        paragraphNode = React.createElement(_Paragraph2['default'], paragraphProps);
                    }
                    contentNode = React.createElement(
                        'div',
                        { className: prefixCls + '-content' },
                        $title,
                        paragraphNode
                    );
                }
                var cls = (0, _classnames2['default'])(prefixCls, className, (_classNames = {}, (0, _defineProperty3['default'])(_classNames, prefixCls + '-with-avatar', hasAvatar), (0, _defineProperty3['default'])(_classNames, prefixCls + '-active', active), _classNames));
                return React.createElement(
                    'div',
                    { className: cls },
                    avatarNode,
                    contentNode
                );
            }
            return children;
        }
    }]);
    return Skeleton;
}(React.Component);

Skeleton.defaultProps = {
    prefixCls: 'ant-skeleton',
    avatar: false,
    title: true,
    paragraph: true
};
exports['default'] = Skeleton;
module.exports = exports['default'];
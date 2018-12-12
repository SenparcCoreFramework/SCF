import _defineProperty from 'babel-runtime/helpers/defineProperty';
import _extends from 'babel-runtime/helpers/extends';
import _classCallCheck from 'babel-runtime/helpers/classCallCheck';
import _createClass from 'babel-runtime/helpers/createClass';
import _possibleConstructorReturn from 'babel-runtime/helpers/possibleConstructorReturn';
import _inherits from 'babel-runtime/helpers/inherits';
import _typeof from 'babel-runtime/helpers/typeof';
import * as React from 'react';
import classNames from 'classnames';
import Avatar from './Avatar';
import Title from './Title';
import Paragraph from './Paragraph';
function getComponentProps(prop) {
    if (prop && (typeof prop === 'undefined' ? 'undefined' : _typeof(prop)) === 'object') {
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
    _inherits(Skeleton, _React$Component);

    function Skeleton() {
        _classCallCheck(this, Skeleton);

        return _possibleConstructorReturn(this, (Skeleton.__proto__ || Object.getPrototypeOf(Skeleton)).apply(this, arguments));
    }

    _createClass(Skeleton, [{
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
                    var avatarProps = _extends({}, getAvatarBasicProps(hasTitle, hasParagraph), getComponentProps(avatar));
                    avatarNode = React.createElement(
                        'div',
                        { className: prefixCls + '-header' },
                        React.createElement(Avatar, avatarProps)
                    );
                }
                var contentNode = void 0;
                if (hasTitle || hasParagraph) {
                    // Title
                    var $title = void 0;
                    if (hasTitle) {
                        var titleProps = _extends({}, getTitleBasicProps(hasAvatar, hasParagraph), getComponentProps(title));
                        $title = React.createElement(Title, titleProps);
                    }
                    // Paragraph
                    var paragraphNode = void 0;
                    if (hasParagraph) {
                        var paragraphProps = _extends({}, getParagraphBasicProps(hasAvatar, hasTitle), getComponentProps(paragraph));
                        paragraphNode = React.createElement(Paragraph, paragraphProps);
                    }
                    contentNode = React.createElement(
                        'div',
                        { className: prefixCls + '-content' },
                        $title,
                        paragraphNode
                    );
                }
                var cls = classNames(prefixCls, className, (_classNames = {}, _defineProperty(_classNames, prefixCls + '-with-avatar', hasAvatar), _defineProperty(_classNames, prefixCls + '-active', active), _classNames));
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
export default Skeleton;
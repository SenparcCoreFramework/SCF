import _defineProperty from 'babel-runtime/helpers/defineProperty';
import _classCallCheck from 'babel-runtime/helpers/classCallCheck';
import _createClass from 'babel-runtime/helpers/createClass';
import _possibleConstructorReturn from 'babel-runtime/helpers/possibleConstructorReturn';
import _inherits from 'babel-runtime/helpers/inherits';
import * as React from 'react';
import * as PropTypes from 'prop-types';
import classNames from 'classnames';

var AnchorLink = function (_React$Component) {
    _inherits(AnchorLink, _React$Component);

    function AnchorLink() {
        _classCallCheck(this, AnchorLink);

        var _this = _possibleConstructorReturn(this, (AnchorLink.__proto__ || Object.getPrototypeOf(AnchorLink)).apply(this, arguments));

        _this.handleClick = function (e) {
            var _this$context$antAnch = _this.context.antAnchor,
                scrollTo = _this$context$antAnch.scrollTo,
                onClick = _this$context$antAnch.onClick;
            var _this$props = _this.props,
                href = _this$props.href,
                title = _this$props.title;

            if (onClick) {
                onClick(e, { title: title, href: href });
            }
            scrollTo(href);
        };
        return _this;
    }

    _createClass(AnchorLink, [{
        key: 'componentDidMount',
        value: function componentDidMount() {
            this.context.antAnchor.registerLink(this.props.href);
        }
    }, {
        key: 'componentWillReceiveProps',
        value: function componentWillReceiveProps(nextProps) {
            var href = nextProps.href;

            if (this.props.href !== href) {
                this.context.antAnchor.unregisterLink(this.props.href);
                this.context.antAnchor.registerLink(href);
            }
        }
    }, {
        key: 'componentWillUnmount',
        value: function componentWillUnmount() {
            this.context.antAnchor.unregisterLink(this.props.href);
        }
    }, {
        key: 'render',
        value: function render() {
            var _props = this.props,
                prefixCls = _props.prefixCls,
                href = _props.href,
                title = _props.title,
                children = _props.children;

            var active = this.context.antAnchor.activeLink === href;
            var wrapperClassName = classNames(prefixCls + '-link', _defineProperty({}, prefixCls + '-link-active', active));
            var titleClassName = classNames(prefixCls + '-link-title', _defineProperty({}, prefixCls + '-link-title-active', active));
            return React.createElement(
                'div',
                { className: wrapperClassName },
                React.createElement(
                    'a',
                    { className: titleClassName, href: href, title: typeof title === 'string' ? title : '', onClick: this.handleClick },
                    title
                ),
                children
            );
        }
    }]);

    return AnchorLink;
}(React.Component);

export default AnchorLink;

AnchorLink.defaultProps = {
    prefixCls: 'ant-anchor',
    href: '#'
};
AnchorLink.contextTypes = {
    antAnchor: PropTypes.object
};
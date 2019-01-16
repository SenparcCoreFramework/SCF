import _defineProperty from 'babel-runtime/helpers/defineProperty';
import _classCallCheck from 'babel-runtime/helpers/classCallCheck';
import _createClass from 'babel-runtime/helpers/createClass';
import _possibleConstructorReturn from 'babel-runtime/helpers/possibleConstructorReturn';
import _inherits from 'babel-runtime/helpers/inherits';
import * as React from 'react';
import classNames from 'classnames';

var Title = function (_React$Component) {
    _inherits(Title, _React$Component);

    function Title() {
        _classCallCheck(this, Title);

        return _possibleConstructorReturn(this, (Title.__proto__ || Object.getPrototypeOf(Title)).apply(this, arguments));
    }

    _createClass(Title, [{
        key: 'render',
        value: function render() {
            var _classNames, _classNames2;

            var _props = this.props,
                prefixCls = _props.prefixCls,
                className = _props.className,
                style = _props.style,
                size = _props.size,
                shape = _props.shape;

            var sizeCls = classNames((_classNames = {}, _defineProperty(_classNames, prefixCls + '-lg', size === 'large'), _defineProperty(_classNames, prefixCls + '-sm', size === 'small'), _classNames));
            var shapeCls = classNames((_classNames2 = {}, _defineProperty(_classNames2, prefixCls + '-circle', shape === 'circle'), _defineProperty(_classNames2, prefixCls + '-square', shape === 'square'), _classNames2));
            return React.createElement('span', { className: classNames(prefixCls, className, sizeCls, shapeCls), style: style });
        }
    }]);

    return Title;
}(React.Component);

Title.defaultProps = {
    prefixCls: 'ant-skeleton-avatar',
    size: 'large'
};
export default Title;
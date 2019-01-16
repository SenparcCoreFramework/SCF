import _extends from 'babel-runtime/helpers/extends';
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
            var _props = this.props,
                prefixCls = _props.prefixCls,
                className = _props.className,
                width = _props.width,
                style = _props.style;

            return React.createElement('h3', { className: classNames(prefixCls, className), style: _extends({ width: width }, style) });
        }
    }]);

    return Title;
}(React.Component);

Title.defaultProps = {
    prefixCls: 'ant-skeleton-title'
};
export default Title;
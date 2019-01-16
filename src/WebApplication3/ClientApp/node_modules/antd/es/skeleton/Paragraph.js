import _toConsumableArray from 'babel-runtime/helpers/toConsumableArray';
import _classCallCheck from 'babel-runtime/helpers/classCallCheck';
import _createClass from 'babel-runtime/helpers/createClass';
import _possibleConstructorReturn from 'babel-runtime/helpers/possibleConstructorReturn';
import _inherits from 'babel-runtime/helpers/inherits';
import * as React from 'react';
import classNames from 'classnames';

var Paragraph = function (_React$Component) {
    _inherits(Paragraph, _React$Component);

    function Paragraph() {
        _classCallCheck(this, Paragraph);

        return _possibleConstructorReturn(this, (Paragraph.__proto__ || Object.getPrototypeOf(Paragraph)).apply(this, arguments));
    }

    _createClass(Paragraph, [{
        key: 'getWidth',
        value: function getWidth(index) {
            var _props = this.props,
                width = _props.width,
                _props$rows = _props.rows,
                rows = _props$rows === undefined ? 2 : _props$rows;

            if (Array.isArray(width)) {
                return width[index];
            }
            // last paragraph
            if (rows - 1 === index) {
                return width;
            }
            return undefined;
        }
    }, {
        key: 'render',
        value: function render() {
            var _this2 = this;

            var _props2 = this.props,
                prefixCls = _props2.prefixCls,
                className = _props2.className,
                style = _props2.style,
                rows = _props2.rows;

            var rowList = [].concat(_toConsumableArray(Array(rows))).map(function (_, index) {
                return React.createElement('li', { key: index, style: { width: _this2.getWidth(index) } });
            });
            return React.createElement(
                'ul',
                { className: classNames(prefixCls, className), style: style },
                rowList
            );
        }
    }]);

    return Paragraph;
}(React.Component);

Paragraph.defaultProps = {
    prefixCls: 'ant-skeleton-paragraph'
};
export default Paragraph;
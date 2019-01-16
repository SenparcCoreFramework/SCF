'use strict';

Object.defineProperty(exports, "__esModule", {
    value: true
});

var _toConsumableArray2 = require('babel-runtime/helpers/toConsumableArray');

var _toConsumableArray3 = _interopRequireDefault(_toConsumableArray2);

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

var Paragraph = function (_React$Component) {
    (0, _inherits3['default'])(Paragraph, _React$Component);

    function Paragraph() {
        (0, _classCallCheck3['default'])(this, Paragraph);
        return (0, _possibleConstructorReturn3['default'])(this, (Paragraph.__proto__ || Object.getPrototypeOf(Paragraph)).apply(this, arguments));
    }

    (0, _createClass3['default'])(Paragraph, [{
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

            var rowList = [].concat((0, _toConsumableArray3['default'])(Array(rows))).map(function (_, index) {
                return React.createElement('li', { key: index, style: { width: _this2.getWidth(index) } });
            });
            return React.createElement(
                'ul',
                { className: (0, _classnames2['default'])(prefixCls, className), style: style },
                rowList
            );
        }
    }]);
    return Paragraph;
}(React.Component);

Paragraph.defaultProps = {
    prefixCls: 'ant-skeleton-paragraph'
};
exports['default'] = Paragraph;
module.exports = exports['default'];
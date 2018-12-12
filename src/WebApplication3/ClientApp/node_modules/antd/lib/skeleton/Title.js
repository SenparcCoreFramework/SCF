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

var Title = function (_React$Component) {
    (0, _inherits3['default'])(Title, _React$Component);

    function Title() {
        (0, _classCallCheck3['default'])(this, Title);
        return (0, _possibleConstructorReturn3['default'])(this, (Title.__proto__ || Object.getPrototypeOf(Title)).apply(this, arguments));
    }

    (0, _createClass3['default'])(Title, [{
        key: 'render',
        value: function render() {
            var _props = this.props,
                prefixCls = _props.prefixCls,
                className = _props.className,
                width = _props.width,
                style = _props.style;

            return React.createElement('h3', { className: (0, _classnames2['default'])(prefixCls, className), style: (0, _extends3['default'])({ width: width }, style) });
        }
    }]);
    return Title;
}(React.Component);

Title.defaultProps = {
    prefixCls: 'ant-skeleton-title'
};
exports['default'] = Title;
module.exports = exports['default'];
'use strict';

Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.ConfigConsumer = undefined;

var _react = require('react');

var React = _interopRequireWildcard(_react);

var _createReactContext = require('create-react-context');

var _createReactContext2 = _interopRequireDefault(_createReactContext);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { 'default': obj }; }

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj['default'] = obj; return newObj; } }

var ConfigContext = (0, _createReactContext2['default'])({});
var ConfigProvider = function ConfigProvider(props) {
    var getPopupContainer = props.getPopupContainer,
        children = props.children;

    var config = {
        getPopupContainer: getPopupContainer
    };
    return React.createElement(
        ConfigContext.Provider,
        { value: config },
        children
    );
};
var ConfigConsumer = exports.ConfigConsumer = ConfigContext.Consumer;
exports['default'] = ConfigProvider;
'use strict';

Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.svgBaseProps = undefined;

var _defineProperty2 = require('babel-runtime/helpers/defineProperty');

var _defineProperty3 = _interopRequireDefault(_defineProperty2);

var _svgBaseProps;

exports.getThemeFromTypeName = getThemeFromTypeName;
exports.removeTypeTheme = removeTypeTheme;
exports.withThemeSuffix = withThemeSuffix;
exports.alias = alias;

var _warning = require('../_util/warning');

var _warning2 = _interopRequireDefault(_warning);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { 'default': obj }; }

// These props make sure that the SVG behaviours like general text.
// Reference: https://blog.prototypr.io/align-svg-icons-to-text-and-say-goodbye-to-font-icons-d44b3d7b26b4
var svgBaseProps = exports.svgBaseProps = (_svgBaseProps = {
    width: '1em',
    height: '1em',
    fill: 'currentColor'
}, (0, _defineProperty3['default'])(_svgBaseProps, 'aria-hidden', 'true'), (0, _defineProperty3['default'])(_svgBaseProps, 'focusable', 'false'), _svgBaseProps);
var fillTester = /-fill$/;
var outlineTester = /-o$/;
var twoToneTester = /-twotone$/;
function getThemeFromTypeName(type) {
    var result = null;
    if (fillTester.test(type)) {
        result = 'filled';
    } else if (outlineTester.test(type)) {
        result = 'outlined';
    } else if (twoToneTester.test(type)) {
        result = 'twoTone';
    }
    return result;
}
function removeTypeTheme(type) {
    return type.replace(fillTester, '').replace(outlineTester, '').replace(twoToneTester, '');
}
function withThemeSuffix(type, theme) {
    var result = type;
    if (theme === 'filled') {
        result += '-fill';
    } else if (theme === 'outlined') {
        result += '-o';
    } else if (theme === 'twoTone') {
        result += '-twotone';
    } else {
        (0, _warning2['default'])(false, 'This icon \'' + type + '\' has unknown theme \'' + theme + '\'');
    }
    return result;
}
// For alias or compatibility
function alias(type) {
    switch (type) {
        case 'cross':
            return 'close';
        default:
    }
    return type;
}
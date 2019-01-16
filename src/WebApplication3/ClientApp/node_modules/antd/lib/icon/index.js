'use strict';

Object.defineProperty(exports, "__esModule", {
    value: true
});

var _extends2 = require('babel-runtime/helpers/extends');

var _extends3 = _interopRequireDefault(_extends2);

var _defineProperty2 = require('babel-runtime/helpers/defineProperty');

var _defineProperty3 = _interopRequireDefault(_defineProperty2);

var _toConsumableArray2 = require('babel-runtime/helpers/toConsumableArray');

var _toConsumableArray3 = _interopRequireDefault(_toConsumableArray2);

var _react = require('react');

var React = _interopRequireWildcard(_react);

var _classnames = require('classnames');

var _classnames2 = _interopRequireDefault(_classnames);

var _dist = require('@ant-design/icons/lib/dist');

var allIcons = _interopRequireWildcard(_dist);

var _iconsReact = require('@ant-design/icons-react');

var _iconsReact2 = _interopRequireDefault(_iconsReact);

var _IconFont = require('./IconFont');

var _IconFont2 = _interopRequireDefault(_IconFont);

var _utils = require('./utils');

var _warning = require('../_util/warning');

var _warning2 = _interopRequireDefault(_warning);

var _twoTonePrimaryColor = require('./twoTonePrimaryColor');

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj['default'] = obj; return newObj; } }

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { 'default': obj }; }

var __rest = undefined && undefined.__rest || function (s, e) {
    var t = {};
    for (var p in s) {
        if (Object.prototype.hasOwnProperty.call(s, p) && e.indexOf(p) < 0) t[p] = s[p];
    }if (s != null && typeof Object.getOwnPropertySymbols === "function") for (var i = 0, p = Object.getOwnPropertySymbols(s); i < p.length; i++) {
        if (e.indexOf(p[i]) < 0) t[p[i]] = s[p[i]];
    }return t;
};

// Initial setting
_iconsReact2['default'].add.apply(_iconsReact2['default'], (0, _toConsumableArray3['default'])(Object.keys(allIcons).map(function (key) {
    return allIcons[key];
})));
(0, _twoTonePrimaryColor.setTwoToneColor)('#1890ff');
var defaultTheme = 'outlined';
var dangerousTheme = undefined;
var Icon = function Icon(props) {
    var _classNames;

    var className = props.className,
        type = props.type,
        Component = props.component,
        viewBox = props.viewBox,
        spin = props.spin,
        children = props.children,
        theme = props.theme,
        twoToneColor = props.twoToneColor,
        restProps = __rest(props, ["className", "type", "component", "viewBox", "spin", "children", "theme", "twoToneColor"]);

    (0, _warning2['default'])(Boolean(type || Component || children), 'Icon should have `type` prop or `component` prop or `children`.');
    var classString = (0, _classnames2['default'])((_classNames = {}, (0, _defineProperty3['default'])(_classNames, 'anticon', true), (0, _defineProperty3['default'])(_classNames, 'anticon-' + type, Boolean(type)), _classNames), className);
    var svgClassString = (0, _classnames2['default'])((0, _defineProperty3['default'])({}, 'anticon-spin', !!spin || type === 'loading'));
    var innerNode = void 0;
    // component > children > type
    if (Component) {
        var innerSvgProps = (0, _extends3['default'])({}, _utils.svgBaseProps, { className: svgClassString, viewBox: viewBox });
        if (!viewBox) {
            delete innerSvgProps.viewBox;
        }
        innerNode = React.createElement(
            Component,
            innerSvgProps,
            children
        );
    }
    if (children) {
        (0, _warning2['default'])(Boolean(viewBox) || React.Children.count(children) === 1 && React.isValidElement(children) && React.Children.only(children).type === 'use', 'Make sure that you provide correct `viewBox`' + ' prop (default `0 0 1024 1024`) to the icon.');
        var _innerSvgProps = (0, _extends3['default'])({}, _utils.svgBaseProps, { className: svgClassString });
        innerNode = React.createElement(
            'svg',
            (0, _extends3['default'])({}, _innerSvgProps, { viewBox: viewBox }),
            children
        );
    }
    if (typeof type === 'string') {
        var computedType = type;
        if (theme) {
            var themeInName = (0, _utils.getThemeFromTypeName)(type);
            (0, _warning2['default'])(!themeInName || theme === themeInName, 'The icon name \'' + type + '\' already specify a theme \'' + themeInName + '\',' + (' the \'theme\' prop \'' + theme + '\' will be ignored.'));
        }
        computedType = (0, _utils.withThemeSuffix)((0, _utils.removeTypeTheme)((0, _utils.alias)(type)), dangerousTheme || theme || defaultTheme);
        innerNode = React.createElement(_iconsReact2['default'], { className: svgClassString, type: computedType, primaryColor: twoToneColor });
    }
    return React.createElement(
        'i',
        (0, _extends3['default'])({}, restProps, { className: classString }),
        innerNode
    );
};
function unstable_ChangeThemeOfIconsDangerously(theme) {
    (0, _warning2['default'])(false, 'You are using the unstable method \'Icon.unstable_ChangeThemeOfAllIconsDangerously\', ' + ('make sure that all the icons with theme \'' + theme + '\' display correctly.'));
    dangerousTheme = theme;
}
function unstable_ChangeDefaultThemeOfIcons(theme) {
    (0, _warning2['default'])(false, 'You are using the unstable method \'Icon.unstable_ChangeDefaultThemeOfIcons\', ' + ('make sure that all the icons with theme \'' + theme + '\' display correctly.'));
    defaultTheme = theme;
}
Icon.createFromIconfontCN = _IconFont2['default'];
Icon.getTwoToneColor = _twoTonePrimaryColor.getTwoToneColor;
Icon.setTwoToneColor = _twoTonePrimaryColor.setTwoToneColor;
exports['default'] = Icon;
module.exports = exports['default'];
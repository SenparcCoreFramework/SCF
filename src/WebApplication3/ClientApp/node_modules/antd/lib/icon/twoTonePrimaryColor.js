'use strict';

Object.defineProperty(exports, "__esModule", {
    value: true
});
exports.setTwoToneColor = setTwoToneColor;
exports.getTwoToneColor = getTwoToneColor;

var _iconsReact = require('@ant-design/icons-react');

var _iconsReact2 = _interopRequireDefault(_iconsReact);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { 'default': obj }; }

function setTwoToneColor(primaryColor) {
    return _iconsReact2['default'].setTwoToneColors({
        primaryColor: primaryColor
    });
}
function getTwoToneColor() {
    var colors = _iconsReact2['default'].getTwoToneColors();
    return colors.primaryColor;
}
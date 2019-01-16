'use strict';

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.genArrProps = genArrProps;
exports.valueProp = valueProp;

var _propTypes = require('prop-types');

var _propTypes2 = _interopRequireDefault(_propTypes);

var _util = require('./util');

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { 'default': obj }; }

var internalValProp = _propTypes2['default'].oneOfType([_propTypes2['default'].string, _propTypes2['default'].number]);

function genArrProps(propType) {
  return _propTypes2['default'].oneOfType([propType, _propTypes2['default'].arrayOf(propType)]);
}

/**
 * Origin code check `multiple` is true when `treeCheckStrictly` & `labelInValue`.
 * But in process logic is already cover to array.
 * Check array is not necessary. Let's simplify this check logic.
 */
function valueProp() {
  for (var _len = arguments.length, args = Array(_len), _key = 0; _key < _len; _key++) {
    args[_key] = arguments[_key];
  }

  var props = args[0],
      propName = args[1],
      Component = args[2];


  if ((0, _util.isLabelInValue)(props)) {
    var _err = genArrProps(_propTypes2['default'].shape({
      label: _propTypes2['default'].node,
      value: internalValProp
    })).apply(undefined, args);
    if (_err) {
      return new Error('Invalid prop `' + propName + '` supplied to `' + Component + '`. ' + 'You should use { label: string, value: string | number } or [{ label: string, value: string | number }] instead.');
    }
    return null;
  }

  var err = genArrProps(internalValProp).apply(undefined, args);
  if (err) {
    return new Error('Invalid prop `' + propName + '` supplied to `' + Component + '`. ' + 'You should use string or [string] instead.');
  }
  return null;
}
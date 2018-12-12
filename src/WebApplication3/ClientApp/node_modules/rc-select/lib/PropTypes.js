'use strict';

Object.defineProperty(exports, "__esModule", {
  value: true
});

var _propTypes = require('prop-types');

var _propTypes2 = _interopRequireDefault(_propTypes);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { 'default': obj }; }

function valueType(props, propName, componentName) {
  var basicType = _propTypes2['default'].oneOfType([_propTypes2['default'].string, _propTypes2['default'].number]);

  var labelInValueShape = _propTypes2['default'].shape({
    key: basicType.isRequired,
    label: _propTypes2['default'].node
  });

  for (var _len = arguments.length, rest = Array(_len > 3 ? _len - 3 : 0), _key = 3; _key < _len; _key++) {
    rest[_key - 3] = arguments[_key];
  }

  if (props.labelInValue) {
    var validate = _propTypes2['default'].oneOfType([_propTypes2['default'].arrayOf(labelInValueShape), labelInValueShape]);
    var error = validate.apply(undefined, [props, propName, componentName].concat(rest));
    if (error) {
      return new Error('Invalid prop `' + propName + '` supplied to `' + componentName + '`, ' + ('when you set `labelInValue` to `true`, `' + propName + '` should in ') + 'shape of `{ key: string | number, label?: ReactNode }`.');
    }
  } else if ((props.mode === 'multiple' || props.mode === 'tags' || props.multiple || props.tags) && props[propName] === '') {
    return new Error('Invalid prop `' + propName + '` of type `string` supplied to `' + componentName + '`, ' + 'expected `array` when `multiple` or `tags` is `true`.');
  } else {
    var _validate = _propTypes2['default'].oneOfType([_propTypes2['default'].arrayOf(basicType), basicType]);
    return _validate.apply(undefined, [props, propName, componentName].concat(rest));
  }
  return null;
}

var SelectPropTypes = {
  id: _propTypes2['default'].string,
  defaultActiveFirstOption: _propTypes2['default'].bool,
  multiple: _propTypes2['default'].bool,
  filterOption: _propTypes2['default'].any,
  children: _propTypes2['default'].any,
  showSearch: _propTypes2['default'].bool,
  disabled: _propTypes2['default'].bool,
  allowClear: _propTypes2['default'].bool,
  showArrow: _propTypes2['default'].bool,
  tags: _propTypes2['default'].bool,
  prefixCls: _propTypes2['default'].string,
  className: _propTypes2['default'].string,
  transitionName: _propTypes2['default'].string,
  optionLabelProp: _propTypes2['default'].string,
  optionFilterProp: _propTypes2['default'].string,
  animation: _propTypes2['default'].string,
  choiceTransitionName: _propTypes2['default'].string,
  open: _propTypes2['default'].bool,
  defaultOpen: _propTypes2['default'].bool,
  onChange: _propTypes2['default'].func,
  onBlur: _propTypes2['default'].func,
  onFocus: _propTypes2['default'].func,
  onSelect: _propTypes2['default'].func,
  onSearch: _propTypes2['default'].func,
  onPopupScroll: _propTypes2['default'].func,
  onMouseEnter: _propTypes2['default'].func,
  onMouseLeave: _propTypes2['default'].func,
  onInputKeyDown: _propTypes2['default'].func,
  placeholder: _propTypes2['default'].any,
  onDeselect: _propTypes2['default'].func,
  labelInValue: _propTypes2['default'].bool,
  loading: _propTypes2['default'].bool,
  value: valueType,
  defaultValue: valueType,
  dropdownStyle: _propTypes2['default'].object,
  maxTagTextLength: _propTypes2['default'].number,
  maxTagCount: _propTypes2['default'].number,
  maxTagPlaceholder: _propTypes2['default'].oneOfType([_propTypes2['default'].node, _propTypes2['default'].func]),
  tokenSeparators: _propTypes2['default'].arrayOf(_propTypes2['default'].string),
  getInputElement: _propTypes2['default'].func,
  showAction: _propTypes2['default'].arrayOf(_propTypes2['default'].string),
  clearIcon: _propTypes2['default'].node,
  inputIcon: _propTypes2['default'].node,
  removeIcon: _propTypes2['default'].node,
  menuItemSelectedIcon: _propTypes2['default'].oneOfType([_propTypes2['default'].func, _propTypes2['default'].node]),
  dropdownRender: _propTypes2['default'].func
};

exports['default'] = SelectPropTypes;
module.exports = exports['default'];
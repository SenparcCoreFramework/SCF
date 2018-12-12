import _extends from 'babel-runtime/helpers/extends';
import _classCallCheck from 'babel-runtime/helpers/classCallCheck';
import _createClass from 'babel-runtime/helpers/createClass';
import _possibleConstructorReturn from 'babel-runtime/helpers/possibleConstructorReturn';
import _inherits from 'babel-runtime/helpers/inherits';
import React, { cloneElement } from 'react';
import { findDOMNode } from 'react-dom';
import PropTypes from 'prop-types';
import toArray from 'rc-util/es/Children/toArray';
import Menu from 'rc-menu';
import scrollIntoView from 'dom-scroll-into-view';
import raf from 'raf';
import { getSelectKeys, preventDefaultEvent, saveRef } from './util';

var DropdownMenu = function (_React$Component) {
  _inherits(DropdownMenu, _React$Component);

  function DropdownMenu(props) {
    _classCallCheck(this, DropdownMenu);

    var _this = _possibleConstructorReturn(this, (DropdownMenu.__proto__ || Object.getPrototypeOf(DropdownMenu)).call(this, props));

    _this.scrollActiveItemToView = function () {
      // scroll into view
      var itemComponent = findDOMNode(_this.firstActiveItem);
      var _this$props = _this.props,
          value = _this$props.value,
          visible = _this$props.visible,
          firstActiveValue = _this$props.firstActiveValue;


      if (!itemComponent || !visible) {
        return;
      }
      var scrollIntoViewOpts = {
        onlyScrollIfNeeded: true
      };
      if ((!value || value.length === 0) && firstActiveValue) {
        scrollIntoViewOpts.alignWithTop = true;
      }

      // Delay to scroll since current frame item position is not ready when pre view is by filter
      // https://github.com/ant-design/ant-design/issues/11268#issuecomment-406634462
      _this.rafInstance = raf(function () {
        scrollIntoView(itemComponent, findDOMNode(_this.menuRef), scrollIntoViewOpts);
      });
    };

    _this.lastInputValue = props.inputValue;
    _this.saveMenuRef = saveRef(_this, 'menuRef');
    return _this;
  }

  _createClass(DropdownMenu, [{
    key: 'componentDidMount',
    value: function componentDidMount() {
      this.scrollActiveItemToView();
      this.lastVisible = this.props.visible;
    }
  }, {
    key: 'shouldComponentUpdate',
    value: function shouldComponentUpdate(nextProps) {
      if (!nextProps.visible) {
        this.lastVisible = false;
      }
      // freeze when hide
      return this.props.visible && !nextProps.visible || nextProps.visible || nextProps.inputValue !== this.props.inputValue;
    }
  }, {
    key: 'componentDidUpdate',
    value: function componentDidUpdate(prevProps) {
      var props = this.props;
      if (!prevProps.visible && props.visible) {
        this.scrollActiveItemToView();
      }
      this.lastVisible = props.visible;
      this.lastInputValue = props.inputValue;
    }
  }, {
    key: 'componentWillUnmount',
    value: function componentWillUnmount() {
      if (this.rafInstance && this.rafInstance.cancel) {
        this.rafInstance.cancel();
      }
    }
  }, {
    key: 'renderMenu',
    value: function renderMenu() {
      var _this2 = this;

      var props = this.props;
      var menuItems = props.menuItems,
          menuItemSelectedIcon = props.menuItemSelectedIcon,
          defaultActiveFirstOption = props.defaultActiveFirstOption,
          value = props.value,
          prefixCls = props.prefixCls,
          multiple = props.multiple,
          onMenuSelect = props.onMenuSelect,
          inputValue = props.inputValue,
          firstActiveValue = props.firstActiveValue,
          backfillValue = props.backfillValue;

      if (menuItems && menuItems.length) {
        var menuProps = {};
        if (multiple) {
          menuProps.onDeselect = props.onMenuDeselect;
          menuProps.onSelect = onMenuSelect;
        } else {
          menuProps.onClick = onMenuSelect;
        }

        var selectedKeys = getSelectKeys(menuItems, value);
        var activeKeyProps = {};

        var clonedMenuItems = menuItems;
        if (selectedKeys.length || firstActiveValue) {
          if (props.visible && !this.lastVisible) {
            activeKeyProps.activeKey = selectedKeys[0] || firstActiveValue;
          } else if (!props.visible) {
            activeKeyProps.activeKey = null;
          }
          var foundFirst = false;
          // set firstActiveItem via cloning menus
          // for scroll into view
          var clone = function clone(item) {
            if (!foundFirst && selectedKeys.indexOf(item.key) !== -1 || !foundFirst && !selectedKeys.length && firstActiveValue.indexOf(item.key) !== -1) {
              foundFirst = true;
              return cloneElement(item, {
                ref: function ref(_ref) {
                  _this2.firstActiveItem = _ref;
                }
              });
            }
            return item;
          };

          clonedMenuItems = menuItems.map(function (item) {
            if (item.type.isMenuItemGroup) {
              var children = toArray(item.props.children).map(clone);
              return cloneElement(item, {}, children);
            }
            return clone(item);
          });
        } else {
          // Clear firstActiveItem when dropdown menu items was empty
          // Avoid `Unable to find node on an unmounted component`
          // https://github.com/ant-design/ant-design/issues/10774
          this.firstActiveItem = null;
        }

        // clear activeKey when inputValue change
        var lastValue = value && value[value.length - 1];
        if (inputValue !== this.lastInputValue && (!lastValue || lastValue !== backfillValue)) {
          activeKeyProps.activeKey = '';
        }
        return React.createElement(
          Menu,
          _extends({
            ref: this.saveMenuRef,
            style: this.props.dropdownMenuStyle,
            defaultActiveFirst: defaultActiveFirstOption,
            role: 'listbox',
            itemIcon: multiple ? menuItemSelectedIcon : null
          }, activeKeyProps, {
            multiple: multiple
          }, menuProps, {
            selectedKeys: selectedKeys,
            prefixCls: prefixCls + '-menu'
          }),
          clonedMenuItems
        );
      }
      return null;
    }
  }, {
    key: 'render',
    value: function render() {
      var renderMenu = this.renderMenu();
      return renderMenu ? React.createElement(
        'div',
        {
          style: {
            overflow: 'auto',
            transform: 'translateZ(0)'
          },
          id: this.props.ariaId,
          onFocus: this.props.onPopupFocus,
          onMouseDown: preventDefaultEvent,
          onScroll: this.props.onPopupScroll
        },
        renderMenu
      ) : null;
    }
  }]);

  return DropdownMenu;
}(React.Component);

DropdownMenu.displayName = 'DropdownMenu';
DropdownMenu.propTypes = {
  ariaId: PropTypes.string,
  defaultActiveFirstOption: PropTypes.bool,
  value: PropTypes.any,
  dropdownMenuStyle: PropTypes.object,
  multiple: PropTypes.bool,
  onPopupFocus: PropTypes.func,
  onPopupScroll: PropTypes.func,
  onMenuDeSelect: PropTypes.func,
  onMenuSelect: PropTypes.func,
  prefixCls: PropTypes.string,
  menuItems: PropTypes.any,
  inputValue: PropTypes.string,
  visible: PropTypes.bool,
  firstActiveValue: PropTypes.string,
  menuItemSelectedIcon: PropTypes.oneOfType([PropTypes.func, PropTypes.node])
};
export default DropdownMenu;
import _defineProperty from 'babel-runtime/helpers/defineProperty';
import _classCallCheck from 'babel-runtime/helpers/classCallCheck';
import _createClass from 'babel-runtime/helpers/createClass';
import _possibleConstructorReturn from 'babel-runtime/helpers/possibleConstructorReturn';
import _inherits from 'babel-runtime/helpers/inherits';
import * as React from 'react';
import * as ReactDOM from 'react-dom';
import Menu, { SubMenu, Item as MenuItem } from 'rc-menu';
import closest from 'dom-closest';
import classNames from 'classnames';
import shallowequal from 'shallowequal';
import Dropdown from '../dropdown';
import Icon from '../icon';
import Checkbox from '../checkbox';
import Radio from '../radio';
import FilterDropdownMenuWrapper from './FilterDropdownMenuWrapper';
function stopPropagation(e) {
    e.stopPropagation();
    if (e.nativeEvent.stopImmediatePropagation) {
        e.nativeEvent.stopImmediatePropagation();
    }
}

var FilterMenu = function (_React$Component) {
    _inherits(FilterMenu, _React$Component);

    function FilterMenu(props) {
        _classCallCheck(this, FilterMenu);

        var _this = _possibleConstructorReturn(this, (FilterMenu.__proto__ || Object.getPrototypeOf(FilterMenu)).call(this, props));

        _this.setNeverShown = function (column) {
            var rootNode = ReactDOM.findDOMNode(_this);
            var filterBelongToScrollBody = !!closest(rootNode, '.ant-table-scroll');
            if (filterBelongToScrollBody) {
                // When fixed column have filters, there will be two dropdown menus
                // Filter dropdown menu inside scroll body should never be shown
                // To fix https://github.com/ant-design/ant-design/issues/5010 and
                // https://github.com/ant-design/ant-design/issues/7909
                _this.neverShown = !!column.fixed;
            }
        };
        _this.setSelectedKeys = function (_ref) {
            var selectedKeys = _ref.selectedKeys;

            _this.setState({ selectedKeys: selectedKeys });
        };
        _this.handleClearFilters = function () {
            _this.setState({
                selectedKeys: []
            }, _this.handleConfirm);
        };
        _this.handleConfirm = function () {
            _this.setVisible(false);
            // Call `setSelectedKeys` & `confirm` in the same time will make filter data not up to date
            // https://github.com/ant-design/ant-design/issues/12284
            _this.setState({}, _this.confirmFilter);
        };
        _this.onVisibleChange = function (visible) {
            _this.setVisible(visible);
            if (!visible) {
                _this.confirmFilter();
            }
        };
        _this.handleMenuItemClick = function (info) {
            var selectedKeys = _this.state.selectedKeys;

            if (!info.keyPath || info.keyPath.length <= 1) {
                return;
            }
            var keyPathOfSelectedItem = _this.state.keyPathOfSelectedItem;
            if (selectedKeys && selectedKeys.indexOf(info.key) >= 0) {
                // deselect SubMenu child
                delete keyPathOfSelectedItem[info.key];
            } else {
                // select SubMenu child
                keyPathOfSelectedItem[info.key] = info.keyPath;
            }
            _this.setState({ keyPathOfSelectedItem: keyPathOfSelectedItem });
        };
        _this.renderFilterIcon = function () {
            var _classNames;

            var _this$props = _this.props,
                column = _this$props.column,
                locale = _this$props.locale,
                prefixCls = _this$props.prefixCls,
                selectedKeys = _this$props.selectedKeys;

            var filtered = selectedKeys && selectedKeys.length > 0;
            var filterIcon = column.filterIcon;
            if (typeof filterIcon === 'function') {
                filterIcon = filterIcon(filtered);
            }
            var dropdownIconClass = classNames((_classNames = {}, _defineProperty(_classNames, prefixCls + '-selected', filtered), _defineProperty(_classNames, prefixCls + '-open', _this.getDropdownVisible()), _classNames));
            return filterIcon ? React.cloneElement(filterIcon, {
                title: locale.filterTitle,
                className: classNames(prefixCls + '-icon', dropdownIconClass, filterIcon.props.className),
                onClick: stopPropagation
            }) : React.createElement(Icon, { title: locale.filterTitle, type: 'filter', theme: 'filled', className: dropdownIconClass, onClick: stopPropagation });
        };
        var visible = 'filterDropdownVisible' in props.column ? props.column.filterDropdownVisible : false;
        _this.state = {
            selectedKeys: props.selectedKeys,
            keyPathOfSelectedItem: {},
            visible: visible
        };
        return _this;
    }

    _createClass(FilterMenu, [{
        key: 'componentDidMount',
        value: function componentDidMount() {
            var column = this.props.column;

            this.setNeverShown(column);
        }
    }, {
        key: 'componentWillReceiveProps',
        value: function componentWillReceiveProps(nextProps) {
            var column = nextProps.column;

            this.setNeverShown(column);
            var newState = {};
            /**
             * if the state is visible the component should ignore updates on selectedKeys prop to avoid
             * that the user selection is lost
             * this happens frequently when a table is connected on some sort of realtime data
             * Fixes https://github.com/ant-design/ant-design/issues/10289 and
             * https://github.com/ant-design/ant-design/issues/10209
             */
            if ('selectedKeys' in nextProps && !shallowequal(this.props.selectedKeys, nextProps.selectedKeys)) {
                newState.selectedKeys = nextProps.selectedKeys;
            }
            if ('filterDropdownVisible' in column) {
                newState.visible = column.filterDropdownVisible;
            }
            if (Object.keys(newState).length > 0) {
                this.setState(newState);
            }
        }
    }, {
        key: 'getDropdownVisible',
        value: function getDropdownVisible() {
            return this.neverShown ? false : this.state.visible;
        }
    }, {
        key: 'setVisible',
        value: function setVisible(visible) {
            var column = this.props.column;

            if (!('filterDropdownVisible' in column)) {
                this.setState({ visible: visible });
            }
            if (column.onFilterDropdownVisibleChange) {
                column.onFilterDropdownVisibleChange(visible);
            }
        }
    }, {
        key: 'confirmFilter',
        value: function confirmFilter() {
            var selectedKeys = this.state.selectedKeys;

            if (!shallowequal(selectedKeys, this.props.selectedKeys)) {
                this.props.confirmFilter(this.props.column, selectedKeys);
            }
        }
    }, {
        key: 'renderMenuItem',
        value: function renderMenuItem(item) {
            var column = this.props.column;
            var selectedKeys = this.state.selectedKeys;

            var multiple = 'filterMultiple' in column ? column.filterMultiple : true;
            var input = multiple ? React.createElement(Checkbox, { checked: selectedKeys && selectedKeys.indexOf(item.value.toString()) >= 0 }) : React.createElement(Radio, { checked: selectedKeys && selectedKeys.indexOf(item.value.toString()) >= 0 });
            return React.createElement(
                MenuItem,
                { key: item.value },
                input,
                React.createElement(
                    'span',
                    null,
                    item.text
                )
            );
        }
    }, {
        key: 'hasSubMenu',
        value: function hasSubMenu() {
            var _props$column$filters = this.props.column.filters,
                filters = _props$column$filters === undefined ? [] : _props$column$filters;

            return filters.some(function (item) {
                return !!(item.children && item.children.length > 0);
            });
        }
    }, {
        key: 'renderMenus',
        value: function renderMenus(items) {
            var _this2 = this;

            return items.map(function (item) {
                if (item.children && item.children.length > 0) {
                    var keyPathOfSelectedItem = _this2.state.keyPathOfSelectedItem;

                    var containSelected = Object.keys(keyPathOfSelectedItem).some(function (key) {
                        return keyPathOfSelectedItem[key].indexOf(item.value) >= 0;
                    });
                    var subMenuCls = containSelected ? _this2.props.dropdownPrefixCls + '-submenu-contain-selected' : '';
                    return React.createElement(
                        SubMenu,
                        { title: item.text, className: subMenuCls, key: item.value.toString() },
                        _this2.renderMenus(item.children)
                    );
                }
                return _this2.renderMenuItem(item);
            });
        }
    }, {
        key: 'render',
        value: function render() {
            var _this3 = this;

            var _props = this.props,
                column = _props.column,
                locale = _props.locale,
                prefixCls = _props.prefixCls,
                dropdownPrefixCls = _props.dropdownPrefixCls,
                getPopupContainer = _props.getPopupContainer;
            // default multiple selection in filter dropdown

            var multiple = 'filterMultiple' in column ? column.filterMultiple : true;
            var dropdownMenuClass = classNames(_defineProperty({}, dropdownPrefixCls + '-menu-without-submenu', !this.hasSubMenu()));
            var filterDropdown = column.filterDropdown;

            if (filterDropdown instanceof Function) {
                filterDropdown = filterDropdown({
                    prefixCls: dropdownPrefixCls + '-custom',
                    setSelectedKeys: function setSelectedKeys(selectedKeys) {
                        return _this3.setSelectedKeys({ selectedKeys: selectedKeys });
                    },
                    selectedKeys: this.state.selectedKeys,
                    confirm: this.handleConfirm,
                    clearFilters: this.handleClearFilters,
                    filters: column.filters,
                    getPopupContainer: function getPopupContainer(triggerNode) {
                        return triggerNode.parentNode;
                    }
                });
            }
            var menus = filterDropdown ? React.createElement(
                FilterDropdownMenuWrapper,
                null,
                filterDropdown
            ) : React.createElement(
                FilterDropdownMenuWrapper,
                { className: prefixCls + '-dropdown' },
                React.createElement(
                    Menu,
                    { multiple: multiple, onClick: this.handleMenuItemClick, prefixCls: dropdownPrefixCls + '-menu', className: dropdownMenuClass, onSelect: this.setSelectedKeys, onDeselect: this.setSelectedKeys, selectedKeys: this.state.selectedKeys, getPopupContainer: function getPopupContainer(triggerNode) {
                            return triggerNode.parentNode;
                        } },
                    this.renderMenus(column.filters)
                ),
                React.createElement(
                    'div',
                    { className: prefixCls + '-dropdown-btns' },
                    React.createElement(
                        'a',
                        { className: prefixCls + '-dropdown-link confirm', onClick: this.handleConfirm },
                        locale.filterConfirm
                    ),
                    React.createElement(
                        'a',
                        { className: prefixCls + '-dropdown-link clear', onClick: this.handleClearFilters },
                        locale.filterReset
                    )
                )
            );
            return React.createElement(
                Dropdown,
                { trigger: ['click'], placement: 'bottomRight', overlay: menus, visible: this.getDropdownVisible(), onVisibleChange: this.onVisibleChange, getPopupContainer: getPopupContainer, forceRender: true },
                this.renderFilterIcon()
            );
        }
    }]);

    return FilterMenu;
}(React.Component);

export default FilterMenu;

FilterMenu.defaultProps = {
    handleFilter: function handleFilter() {},

    column: {}
};
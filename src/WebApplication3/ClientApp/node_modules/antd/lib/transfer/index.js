'use strict';

Object.defineProperty(exports, "__esModule", {
    value: true
});

var _extends2 = require('babel-runtime/helpers/extends');

var _extends3 = _interopRequireDefault(_extends2);

var _toConsumableArray2 = require('babel-runtime/helpers/toConsumableArray');

var _toConsumableArray3 = _interopRequireDefault(_toConsumableArray2);

var _defineProperty2 = require('babel-runtime/helpers/defineProperty');

var _defineProperty3 = _interopRequireDefault(_defineProperty2);

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

var _propTypes = require('prop-types');

var PropTypes = _interopRequireWildcard(_propTypes);

var _classnames = require('classnames');

var _classnames2 = _interopRequireDefault(_classnames);

var _list = require('./list');

var _list2 = _interopRequireDefault(_list);

var _operation = require('./operation');

var _operation2 = _interopRequireDefault(_operation);

var _search = require('./search');

var _search2 = _interopRequireDefault(_search);

var _warning = require('../_util/warning');

var _warning2 = _interopRequireDefault(_warning);

var _LocaleReceiver = require('../locale-provider/LocaleReceiver');

var _LocaleReceiver2 = _interopRequireDefault(_LocaleReceiver);

var _default = require('../locale-provider/default');

var _default2 = _interopRequireDefault(_default);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj['default'] = obj; return newObj; } }

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { 'default': obj }; }

function noop() {}

var Transfer = function (_React$Component) {
    (0, _inherits3['default'])(Transfer, _React$Component);

    function Transfer(props) {
        (0, _classCallCheck3['default'])(this, Transfer);

        var _this = (0, _possibleConstructorReturn3['default'])(this, (Transfer.__proto__ || Object.getPrototypeOf(Transfer)).call(this, props));

        _this.separatedDataSource = null;
        _this.moveTo = function (direction) {
            var _this$props = _this.props,
                _this$props$targetKey = _this$props.targetKeys,
                targetKeys = _this$props$targetKey === undefined ? [] : _this$props$targetKey,
                _this$props$dataSourc = _this$props.dataSource,
                dataSource = _this$props$dataSourc === undefined ? [] : _this$props$dataSourc,
                onChange = _this$props.onChange;
            var _this$state = _this.state,
                sourceSelectedKeys = _this$state.sourceSelectedKeys,
                targetSelectedKeys = _this$state.targetSelectedKeys;

            var moveKeys = direction === 'right' ? sourceSelectedKeys : targetSelectedKeys;
            // filter the disabled options
            var newMoveKeys = moveKeys.filter(function (key) {
                return !dataSource.some(function (data) {
                    return !!(key === data.key && data.disabled);
                });
            });
            // move items to target box
            var newTargetKeys = direction === 'right' ? newMoveKeys.concat(targetKeys) : targetKeys.filter(function (targetKey) {
                return newMoveKeys.indexOf(targetKey) === -1;
            });
            // empty checked keys
            var oppositeDirection = direction === 'right' ? 'left' : 'right';
            _this.setState((0, _defineProperty3['default'])({}, _this.getSelectedKeysName(oppositeDirection), []));
            _this.handleSelectChange(oppositeDirection, []);
            if (onChange) {
                onChange(newTargetKeys, direction, newMoveKeys);
            }
        };
        _this.moveToLeft = function () {
            return _this.moveTo('left');
        };
        _this.moveToRight = function () {
            return _this.moveTo('right');
        };
        _this.handleSelectAll = function (direction, filteredDataSource, checkAll) {
            var originalSelectedKeys = _this.state[_this.getSelectedKeysName(direction)] || [];
            var currentKeys = filteredDataSource.map(function (item) {
                return item.key;
            });
            // Only operate current keys from original selected keys
            var newKeys1 = originalSelectedKeys.filter(function (key) {
                return currentKeys.indexOf(key) === -1;
            });
            var newKeys2 = [].concat((0, _toConsumableArray3['default'])(originalSelectedKeys));
            currentKeys.forEach(function (key) {
                if (newKeys2.indexOf(key) === -1) {
                    newKeys2.push(key);
                }
            });
            var holder = checkAll ? newKeys1 : newKeys2;
            _this.handleSelectChange(direction, holder);
            if (!_this.props.selectedKeys) {
                _this.setState((0, _defineProperty3['default'])({}, _this.getSelectedKeysName(direction), holder));
            }
        };
        _this.handleLeftSelectAll = function (filteredDataSource, checkAll) {
            return _this.handleSelectAll('left', filteredDataSource, checkAll);
        };
        _this.handleRightSelectAll = function (filteredDataSource, checkAll) {
            return _this.handleSelectAll('right', filteredDataSource, checkAll);
        };
        _this.handleFilter = function (direction, e) {
            var _this$props2 = _this.props,
                onSearchChange = _this$props2.onSearchChange,
                onSearch = _this$props2.onSearch;

            var value = e.target.value;
            _this.setState((0, _defineProperty3['default'])({}, direction + 'Filter', value));
            if (onSearchChange) {
                (0, _warning2['default'])(false, '`onSearchChange` in Transfer is deprecated. Please use `onSearch` instead.');
                onSearchChange(direction, e);
            }
            if (onSearch) {
                onSearch(direction, value);
            }
        };
        _this.handleLeftFilter = function (e) {
            return _this.handleFilter('left', e);
        };
        _this.handleRightFilter = function (e) {
            return _this.handleFilter('right', e);
        };
        _this.handleClear = function (direction) {
            var onSearch = _this.props.onSearch;

            _this.setState((0, _defineProperty3['default'])({}, direction + 'Filter', ''));
            if (onSearch) {
                onSearch(direction, '');
            }
        };
        _this.handleLeftClear = function () {
            return _this.handleClear('left');
        };
        _this.handleRightClear = function () {
            return _this.handleClear('right');
        };
        _this.handleSelect = function (direction, selectedItem, checked) {
            var _this$state2 = _this.state,
                sourceSelectedKeys = _this$state2.sourceSelectedKeys,
                targetSelectedKeys = _this$state2.targetSelectedKeys;

            var holder = direction === 'left' ? [].concat((0, _toConsumableArray3['default'])(sourceSelectedKeys)) : [].concat((0, _toConsumableArray3['default'])(targetSelectedKeys));
            var index = holder.indexOf(selectedItem.key);
            if (index > -1) {
                holder.splice(index, 1);
            }
            if (checked) {
                holder.push(selectedItem.key);
            }
            _this.handleSelectChange(direction, holder);
            if (!_this.props.selectedKeys) {
                _this.setState((0, _defineProperty3['default'])({}, _this.getSelectedKeysName(direction), holder));
            }
        };
        _this.handleLeftSelect = function (selectedItem, checked) {
            return _this.handleSelect('left', selectedItem, checked);
        };
        _this.handleRightSelect = function (selectedItem, checked) {
            return _this.handleSelect('right', selectedItem, checked);
        };
        _this.handleScroll = function (direction, e) {
            var onScroll = _this.props.onScroll;

            if (onScroll) {
                onScroll(direction, e);
            }
        };
        _this.handleLeftScroll = function (e) {
            return _this.handleScroll('left', e);
        };
        _this.handleRightScroll = function (e) {
            return _this.handleScroll('right', e);
        };
        _this.getLocale = function (transferLocale) {
            // Keep old locale props still working.
            var oldLocale = {};
            if ('notFoundContent' in _this.props) {
                oldLocale.notFoundContent = _this.props.notFoundContent;
            }
            if ('searchPlaceholder' in _this.props) {
                oldLocale.searchPlaceholder = _this.props.searchPlaceholder;
            }
            return (0, _extends3['default'])({}, transferLocale, oldLocale, _this.props.locale);
        };
        _this.renderTransfer = function (transferLocale) {
            var _this$props3 = _this.props,
                _this$props3$prefixCl = _this$props3.prefixCls,
                prefixCls = _this$props3$prefixCl === undefined ? 'ant-transfer' : _this$props3$prefixCl,
                className = _this$props3.className,
                disabled = _this$props3.disabled,
                _this$props3$operatio = _this$props3.operations,
                operations = _this$props3$operatio === undefined ? [] : _this$props3$operatio,
                showSearch = _this$props3.showSearch,
                body = _this$props3.body,
                footer = _this$props3.footer,
                style = _this$props3.style,
                listStyle = _this$props3.listStyle,
                operationStyle = _this$props3.operationStyle,
                filterOption = _this$props3.filterOption,
                render = _this$props3.render,
                lazy = _this$props3.lazy;

            var locale = _this.getLocale(transferLocale);
            var _this$state3 = _this.state,
                leftFilter = _this$state3.leftFilter,
                rightFilter = _this$state3.rightFilter,
                sourceSelectedKeys = _this$state3.sourceSelectedKeys,
                targetSelectedKeys = _this$state3.targetSelectedKeys;

            var _this$separateDataSou = _this.separateDataSource(_this.props),
                leftDataSource = _this$separateDataSou.leftDataSource,
                rightDataSource = _this$separateDataSou.rightDataSource;

            var leftActive = targetSelectedKeys.length > 0;
            var rightActive = sourceSelectedKeys.length > 0;
            var cls = (0, _classnames2['default'])(className, prefixCls, disabled && prefixCls + '-disabled');
            var titles = _this.getTitles(locale);
            return React.createElement(
                'div',
                { className: cls, style: style },
                React.createElement(_list2['default'], (0, _extends3['default'])({ prefixCls: prefixCls + '-list', titleText: titles[0], dataSource: leftDataSource, filter: leftFilter, filterOption: filterOption, style: listStyle, checkedKeys: sourceSelectedKeys, handleFilter: _this.handleLeftFilter, handleClear: _this.handleLeftClear, handleSelect: _this.handleLeftSelect, handleSelectAll: _this.handleLeftSelectAll, render: render, showSearch: showSearch, body: body, footer: footer, lazy: lazy, onScroll: _this.handleLeftScroll, disabled: disabled }, locale)),
                React.createElement(_operation2['default'], { className: prefixCls + '-operation', rightActive: rightActive, rightArrowText: operations[0], moveToRight: _this.moveToRight, leftActive: leftActive, leftArrowText: operations[1], moveToLeft: _this.moveToLeft, style: operationStyle, disabled: disabled }),
                React.createElement(_list2['default'], (0, _extends3['default'])({ prefixCls: prefixCls + '-list', titleText: titles[1], dataSource: rightDataSource, filter: rightFilter, filterOption: filterOption, style: listStyle, checkedKeys: targetSelectedKeys, handleFilter: _this.handleRightFilter, handleClear: _this.handleRightClear, handleSelect: _this.handleRightSelect, handleSelectAll: _this.handleRightSelectAll, render: render, showSearch: showSearch, body: body, footer: footer, lazy: lazy, onScroll: _this.handleRightScroll, disabled: disabled }, locale))
            );
        };
        (0, _warning2['default'])(!('notFoundContent' in props || 'searchPlaceholder' in props), 'Transfer[notFoundContent] and Transfer[searchPlaceholder] will be removed, ' + 'please use Transfer[locale] instead.');
        var _props$selectedKeys = props.selectedKeys,
            selectedKeys = _props$selectedKeys === undefined ? [] : _props$selectedKeys,
            _props$targetKeys = props.targetKeys,
            targetKeys = _props$targetKeys === undefined ? [] : _props$targetKeys;

        _this.state = {
            leftFilter: '',
            rightFilter: '',
            sourceSelectedKeys: selectedKeys.filter(function (key) {
                return targetKeys.indexOf(key) === -1;
            }),
            targetSelectedKeys: selectedKeys.filter(function (key) {
                return targetKeys.indexOf(key) > -1;
            })
        };
        return _this;
    }

    (0, _createClass3['default'])(Transfer, [{
        key: 'componentWillReceiveProps',
        value: function componentWillReceiveProps(nextProps) {
            var _state = this.state,
                sourceSelectedKeys = _state.sourceSelectedKeys,
                targetSelectedKeys = _state.targetSelectedKeys;

            if (nextProps.targetKeys !== this.props.targetKeys || nextProps.dataSource !== this.props.dataSource) {
                // clear cached separated dataSource
                this.separatedDataSource = null;
                if (!nextProps.selectedKeys) {
                    // clear key no longer existed
                    // clear checkedKeys according to targetKeys
                    var dataSource = nextProps.dataSource,
                        _nextProps$targetKeys = nextProps.targetKeys,
                        targetKeys = _nextProps$targetKeys === undefined ? [] : _nextProps$targetKeys;

                    var newSourceSelectedKeys = [];
                    var newTargetSelectedKeys = [];
                    dataSource.forEach(function (_ref) {
                        var key = _ref.key;

                        if (sourceSelectedKeys.includes(key) && !targetKeys.includes(key)) {
                            newSourceSelectedKeys.push(key);
                        }
                        if (targetSelectedKeys.includes(key) && targetKeys.includes(key)) {
                            newTargetSelectedKeys.push(key);
                        }
                    });
                    this.setState({
                        sourceSelectedKeys: newSourceSelectedKeys,
                        targetSelectedKeys: newTargetSelectedKeys
                    });
                }
            }
            if (nextProps.selectedKeys) {
                var _targetKeys = nextProps.targetKeys || [];
                this.setState({
                    sourceSelectedKeys: nextProps.selectedKeys.filter(function (key) {
                        return !_targetKeys.includes(key);
                    }),
                    targetSelectedKeys: nextProps.selectedKeys.filter(function (key) {
                        return _targetKeys.includes(key);
                    })
                });
            }
        }
    }, {
        key: 'separateDataSource',
        value: function separateDataSource(props) {
            if (this.separatedDataSource) {
                return this.separatedDataSource;
            }
            var dataSource = props.dataSource,
                rowKey = props.rowKey,
                _props$targetKeys2 = props.targetKeys,
                targetKeys = _props$targetKeys2 === undefined ? [] : _props$targetKeys2;

            var leftDataSource = [];
            var rightDataSource = new Array(targetKeys.length);
            dataSource.forEach(function (record) {
                if (rowKey) {
                    record.key = rowKey(record);
                }
                // rightDataSource should be ordered by targetKeys
                // leftDataSource should be ordered by dataSource
                var indexOfKey = targetKeys.indexOf(record.key);
                if (indexOfKey !== -1) {
                    rightDataSource[indexOfKey] = record;
                } else {
                    leftDataSource.push(record);
                }
            });
            this.separatedDataSource = {
                leftDataSource: leftDataSource,
                rightDataSource: rightDataSource
            };
            return this.separatedDataSource;
        }
    }, {
        key: 'handleSelectChange',
        value: function handleSelectChange(direction, holder) {
            var _state2 = this.state,
                sourceSelectedKeys = _state2.sourceSelectedKeys,
                targetSelectedKeys = _state2.targetSelectedKeys;

            var onSelectChange = this.props.onSelectChange;
            if (!onSelectChange) {
                return;
            }
            if (direction === 'left') {
                onSelectChange(holder, targetSelectedKeys);
            } else {
                onSelectChange(sourceSelectedKeys, holder);
            }
        }
    }, {
        key: 'getTitles',
        value: function getTitles(transferLocale) {
            var props = this.props;

            if (props.titles) {
                return props.titles;
            }
            return transferLocale.titles;
        }
    }, {
        key: 'getSelectedKeysName',
        value: function getSelectedKeysName(direction) {
            return direction === 'left' ? 'sourceSelectedKeys' : 'targetSelectedKeys';
        }
    }, {
        key: 'render',
        value: function render() {
            return React.createElement(
                _LocaleReceiver2['default'],
                { componentName: 'Transfer', defaultLocale: _default2['default'].Transfer },
                this.renderTransfer
            );
        }
    }]);
    return Transfer;
}(React.Component);
// For high-level customized Transfer @dqaria


exports['default'] = Transfer;
Transfer.List = _list2['default'];
Transfer.Operation = _operation2['default'];
Transfer.Search = _search2['default'];
Transfer.defaultProps = {
    dataSource: [],
    render: noop,
    locale: {},
    showSearch: false
};
Transfer.propTypes = {
    prefixCls: PropTypes.string,
    disabled: PropTypes.bool,
    dataSource: PropTypes.array,
    render: PropTypes.func,
    targetKeys: PropTypes.array,
    onChange: PropTypes.func,
    height: PropTypes.number,
    style: PropTypes.object,
    listStyle: PropTypes.object,
    operationStyle: PropTypes.object,
    className: PropTypes.string,
    titles: PropTypes.array,
    operations: PropTypes.array,
    showSearch: PropTypes.bool,
    filterOption: PropTypes.func,
    searchPlaceholder: PropTypes.string,
    notFoundContent: PropTypes.node,
    locale: PropTypes.object,
    body: PropTypes.func,
    footer: PropTypes.func,
    rowKey: PropTypes.func,
    lazy: PropTypes.oneOfType([PropTypes.object, PropTypes.bool])
};
module.exports = exports['default'];
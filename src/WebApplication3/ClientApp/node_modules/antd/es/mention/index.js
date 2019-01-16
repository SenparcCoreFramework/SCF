import _extends from 'babel-runtime/helpers/extends';
import _defineProperty from 'babel-runtime/helpers/defineProperty';
import _classCallCheck from 'babel-runtime/helpers/classCallCheck';
import _createClass from 'babel-runtime/helpers/createClass';
import _possibleConstructorReturn from 'babel-runtime/helpers/possibleConstructorReturn';
import _inherits from 'babel-runtime/helpers/inherits';
import * as React from 'react';
import RcMention, { Nav, toString, toEditorState, getMentions } from 'rc-editor-mention';
import { polyfill } from 'react-lifecycles-compat';
import classNames from 'classnames';
import shallowequal from 'shallowequal';
import Icon from '../icon';

var Mention = function (_React$Component) {
    _inherits(Mention, _React$Component);

    function Mention(props) {
        _classCallCheck(this, Mention);

        var _this = _possibleConstructorReturn(this, (Mention.__proto__ || Object.getPrototypeOf(Mention)).call(this, props));

        _this.onSearchChange = function (value, prefix) {
            if (_this.props.onSearchChange) {
                return _this.props.onSearchChange(value, prefix);
            }
            return _this.defaultSearchChange(value);
        };
        _this.onChange = function (editorState) {
            if (_this.props.onChange) {
                _this.props.onChange(editorState);
            }
        };
        _this.onFocus = function (ev) {
            _this.setState({
                focus: true
            });
            if (_this.props.onFocus) {
                _this.props.onFocus(ev);
            }
        };
        _this.onBlur = function (ev) {
            _this.setState({
                focus: false
            });
            if (_this.props.onBlur) {
                _this.props.onBlur(ev);
            }
        };
        _this.focus = function () {
            _this.mentionEle._editor.focusEditor();
        };
        _this.mentionRef = function (ele) {
            _this.mentionEle = ele;
        };
        _this.state = {
            suggestions: props.suggestions,
            focus: false
        };
        return _this;
    }

    _createClass(Mention, [{
        key: 'defaultSearchChange',
        value: function defaultSearchChange(value) {
            var searchValue = value.toLowerCase();
            var filteredSuggestions = (this.props.suggestions || []).filter(function (suggestion) {
                if (suggestion.type && suggestion.type === Nav) {
                    return suggestion.props.value ? suggestion.props.value.toLowerCase().indexOf(searchValue) !== -1 : true;
                }
                return suggestion.toLowerCase().indexOf(searchValue) !== -1;
            });
            this.setState({
                suggestions: filteredSuggestions
            });
        }
    }, {
        key: 'render',
        value: function render() {
            var _classNames;

            var _props = this.props,
                _props$className = _props.className,
                className = _props$className === undefined ? '' : _props$className,
                prefixCls = _props.prefixCls,
                loading = _props.loading,
                placement = _props.placement;
            var _state = this.state,
                suggestions = _state.suggestions,
                focus = _state.focus;

            var cls = classNames(className, (_classNames = {}, _defineProperty(_classNames, prefixCls + '-active', focus), _defineProperty(_classNames, prefixCls + '-placement-top', placement === 'top'), _classNames));
            var notFoundContent = loading ? React.createElement(Icon, { type: 'loading' }) : this.props.notFoundContent;
            return React.createElement(RcMention, _extends({}, this.props, { className: cls, ref: this.mentionRef, onSearchChange: this.onSearchChange, onChange: this.onChange, onFocus: this.onFocus, onBlur: this.onBlur, suggestions: suggestions, notFoundContent: notFoundContent }));
        }
    }], [{
        key: 'getDerivedStateFromProps',
        value: function getDerivedStateFromProps(nextProps, state) {
            var suggestions = nextProps.suggestions;

            if (!shallowequal(suggestions, state.suggestions)) {
                return {
                    suggestions: suggestions
                };
            }
            return null;
        }
    }]);

    return Mention;
}(React.Component);

Mention.getMentions = getMentions;
Mention.defaultProps = {
    prefixCls: 'ant-mention',
    notFoundContent: '无匹配结果，轻敲空格完成输入',
    loading: false,
    multiLines: false,
    placement: 'bottom'
};
Mention.Nav = Nav;
Mention.toString = toString;
Mention.toContentState = toEditorState;
polyfill(Mention);
export default Mention;
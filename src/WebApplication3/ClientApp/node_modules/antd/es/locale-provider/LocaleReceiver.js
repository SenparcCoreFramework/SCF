import _extends from 'babel-runtime/helpers/extends';
import _classCallCheck from 'babel-runtime/helpers/classCallCheck';
import _createClass from 'babel-runtime/helpers/createClass';
import _possibleConstructorReturn from 'babel-runtime/helpers/possibleConstructorReturn';
import _inherits from 'babel-runtime/helpers/inherits';
import * as React from 'react';
import * as PropTypes from 'prop-types';
import defaultLocaleData from './default';

var LocaleReceiver = function (_React$Component) {
    _inherits(LocaleReceiver, _React$Component);

    function LocaleReceiver() {
        _classCallCheck(this, LocaleReceiver);

        return _possibleConstructorReturn(this, (LocaleReceiver.__proto__ || Object.getPrototypeOf(LocaleReceiver)).apply(this, arguments));
    }

    _createClass(LocaleReceiver, [{
        key: 'getLocale',
        value: function getLocale() {
            var _props = this.props,
                componentName = _props.componentName,
                defaultLocale = _props.defaultLocale;

            var locale = defaultLocale || defaultLocaleData[componentName || 'global'];
            var antLocale = this.context.antLocale;

            var localeFromContext = componentName && antLocale ? antLocale[componentName] : {};
            return _extends({}, typeof locale === 'function' ? locale() : locale, localeFromContext || {});
        }
    }, {
        key: 'getLocaleCode',
        value: function getLocaleCode() {
            var antLocale = this.context.antLocale;

            var localeCode = antLocale && antLocale.locale;
            // Had use LocaleProvide but didn't set locale
            if (antLocale && antLocale.exist && !localeCode) {
                return defaultLocaleData.locale;
            }
            return localeCode;
        }
    }, {
        key: 'render',
        value: function render() {
            return this.props.children(this.getLocale(), this.getLocaleCode());
        }
    }]);

    return LocaleReceiver;
}(React.Component);

export default LocaleReceiver;

LocaleReceiver.defaultProps = {
    componentName: 'global'
};
LocaleReceiver.contextTypes = {
    antLocale: PropTypes.object
};
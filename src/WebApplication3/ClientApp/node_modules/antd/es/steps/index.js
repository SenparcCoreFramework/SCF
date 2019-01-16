import _extends from 'babel-runtime/helpers/extends';
import _classCallCheck from 'babel-runtime/helpers/classCallCheck';
import _createClass from 'babel-runtime/helpers/createClass';
import _possibleConstructorReturn from 'babel-runtime/helpers/possibleConstructorReturn';
import _inherits from 'babel-runtime/helpers/inherits';
import * as React from 'react';
import * as PropTypes from 'prop-types';
import RcSteps from 'rc-steps';
import Icon from '../icon';

var Steps = function (_React$Component) {
    _inherits(Steps, _React$Component);

    function Steps() {
        _classCallCheck(this, Steps);

        return _possibleConstructorReturn(this, (Steps.__proto__ || Object.getPrototypeOf(Steps)).apply(this, arguments));
    }

    _createClass(Steps, [{
        key: 'render',
        value: function render() {
            var prefixCls = this.props.prefixCls;

            var icons = {
                finish: React.createElement(Icon, { type: 'check', className: prefixCls + '-finish-icon' }),
                error: React.createElement(Icon, { type: 'close', className: prefixCls + '-error-icon' })
            };
            return React.createElement(RcSteps, _extends({ icons: icons }, this.props));
        }
    }]);

    return Steps;
}(React.Component);

export default Steps;

Steps.Step = RcSteps.Step;
Steps.defaultProps = {
    prefixCls: 'ant-steps',
    iconPrefix: 'ant',
    current: 0
};
Steps.propTypes = {
    prefixCls: PropTypes.string,
    iconPrefix: PropTypes.string,
    current: PropTypes.number
};
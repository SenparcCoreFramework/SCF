import * as React from 'react';
import * as PropTypes from 'prop-types';
import { RadioGroupProps, RadioGroupState, RadioChangeEvent, RadioGroupButtonStyle } from './interface';
export default class RadioGroup extends React.Component<RadioGroupProps, RadioGroupState> {
    static defaultProps: {
        disabled: boolean;
        prefixCls: string;
        buttonStyle: RadioGroupButtonStyle;
    };
    static childContextTypes: {
        radioGroup: PropTypes.Requireable<any>;
    };
    constructor(props: RadioGroupProps);
    getChildContext(): {
        radioGroup: {
            onChange: (ev: RadioChangeEvent) => void;
            value: any;
            disabled: boolean | undefined;
            name: string | undefined;
        };
    };
    componentWillReceiveProps(nextProps: RadioGroupProps): void;
    shouldComponentUpdate(nextProps: RadioGroupProps, nextState: RadioGroupState): boolean;
    onRadioChange: (ev: RadioChangeEvent) => void;
    render(): JSX.Element;
}

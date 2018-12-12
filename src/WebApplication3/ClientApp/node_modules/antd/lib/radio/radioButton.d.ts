import * as React from 'react';
import * as PropTypes from 'prop-types';
import { AbstractCheckboxProps } from '../checkbox/Checkbox';
import { RadioChangeEvent } from './interface';
export declare type RadioButtonProps = AbstractCheckboxProps<RadioChangeEvent>;
export default class RadioButton extends React.Component<RadioButtonProps, any> {
    static defaultProps: {
        prefixCls: string;
    };
    static contextTypes: {
        radioGroup: PropTypes.Requireable<any>;
    };
    context: any;
    render(): JSX.Element;
}

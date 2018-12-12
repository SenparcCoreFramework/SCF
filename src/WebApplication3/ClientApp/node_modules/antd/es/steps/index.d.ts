import * as React from 'react';
import * as PropTypes from 'prop-types';
export interface StepsProps {
    prefixCls?: string;
    iconPrefix?: string;
    current?: number;
    initial?: number;
    labelPlacement?: 'horizontal' | 'vertical';
    status?: 'wait' | 'process' | 'finish' | 'error';
    size?: 'default' | 'small';
    direction?: 'horizontal' | 'vertical';
    progressDot?: boolean | Function;
    style?: React.CSSProperties;
}
export default class Steps extends React.Component<StepsProps, any> {
    static Step: any;
    static defaultProps: {
        prefixCls: string;
        iconPrefix: string;
        current: number;
    };
    static propTypes: {
        prefixCls: PropTypes.Requireable<string>;
        iconPrefix: PropTypes.Requireable<string>;
        current: PropTypes.Requireable<number>;
    };
    render(): JSX.Element;
}

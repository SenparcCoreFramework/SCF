import * as PropTypes from 'prop-types';
import * as React from 'react';
export declare type ProgressType = 'line' | 'circle' | 'dashboard';
export declare type ProgressSize = 'default' | 'small';
export interface ProgressProps {
    prefixCls?: string;
    className?: string;
    type?: ProgressType;
    percent?: number;
    successPercent?: number;
    format?: (percent?: number, successPercent?: number) => React.ReactNode;
    status?: 'success' | 'active' | 'exception';
    showInfo?: boolean;
    strokeWidth?: number;
    strokeLinecap?: string;
    strokeColor?: string;
    trailColor?: string;
    width?: number;
    style?: React.CSSProperties;
    gapDegree?: number;
    gapPosition?: 'top' | 'bottom' | 'left' | 'right';
    size?: ProgressSize;
}
export default class Progress extends React.Component<ProgressProps, {}> {
    static defaultProps: {
        type: string;
        percent: number;
        showInfo: boolean;
        trailColor: string;
        prefixCls: string;
        size: string;
    };
    static propTypes: {
        status: PropTypes.Requireable<string>;
        type: PropTypes.Requireable<string>;
        showInfo: PropTypes.Requireable<boolean>;
        percent: PropTypes.Requireable<number>;
        width: PropTypes.Requireable<number>;
        strokeWidth: PropTypes.Requireable<number>;
        strokeLinecap: PropTypes.Requireable<string>;
        strokeColor: PropTypes.Requireable<string>;
        trailColor: PropTypes.Requireable<string>;
        format: PropTypes.Requireable<(...args: any[]) => any>;
        gapDegree: PropTypes.Requireable<number>;
        default: PropTypes.Requireable<string>;
    };
    render(): JSX.Element;
}

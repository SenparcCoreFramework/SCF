import * as React from 'react';
import * as PropTypes from 'prop-types';
export declare type SpinSize = 'small' | 'default' | 'large';
export declare type SpinIndicator = React.ReactElement<any>;
export interface SpinProps {
    prefixCls?: string;
    className?: string;
    spinning?: boolean;
    style?: React.CSSProperties;
    size?: SpinSize;
    tip?: string;
    delay?: number;
    wrapperClassName?: string;
    indicator?: SpinIndicator;
}
export interface SpinState {
    spinning?: boolean;
    notCssAnimationSupported?: boolean;
}
declare class Spin extends React.Component<SpinProps, SpinState> {
    static defaultProps: {
        prefixCls: string;
        spinning: boolean;
        size: "default" | "small" | "large";
        wrapperClassName: string;
    };
    static propTypes: {
        prefixCls: PropTypes.Requireable<string>;
        className: PropTypes.Requireable<string>;
        spinning: PropTypes.Requireable<boolean>;
        size: PropTypes.Requireable<string>;
        wrapperClassName: PropTypes.Requireable<string>;
        indicator: PropTypes.Requireable<PropTypes.ReactNodeLike>;
    };
    static setDefaultIndicator(indicator: React.ReactNode): void;
    debounceTimeout: number;
    delayTimeout: number;
    constructor(props: SpinProps);
    isNestedPattern(): boolean;
    componentDidMount(): void;
    componentWillUnmount(): void;
    componentDidUpdate(): void;
    delayUpdateSpinning: () => void;
    render(): JSX.Element;
}
export default Spin;

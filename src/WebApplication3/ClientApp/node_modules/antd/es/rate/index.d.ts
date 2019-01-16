import * as React from 'react';
import * as PropTypes from 'prop-types';
export interface RateProps {
    prefixCls?: string;
    count?: number;
    value?: number;
    defaultValue?: number;
    allowHalf?: boolean;
    allowClear?: boolean;
    disabled?: boolean;
    onChange?: (value: number) => any;
    onHoverChange?: (value: number) => any;
    character?: React.ReactNode;
    className?: string;
    style?: React.CSSProperties;
}
export default class Rate extends React.Component<RateProps, any> {
    static propTypes: {
        prefixCls: PropTypes.Requireable<string>;
        character: PropTypes.Requireable<PropTypes.ReactNodeLike>;
    };
    static defaultProps: {
        prefixCls: string;
        character: JSX.Element;
    };
    private rcRate;
    focus(): void;
    blur(): void;
    saveRate: (node: any) => void;
    render(): JSX.Element;
}

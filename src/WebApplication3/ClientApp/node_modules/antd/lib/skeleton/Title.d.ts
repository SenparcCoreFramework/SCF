import * as React from 'react';
export interface SkeletonTitleProps {
    prefixCls?: string;
    className?: string;
    style?: object;
    width?: number | string;
}
declare class Title extends React.Component<SkeletonTitleProps, any> {
    static defaultProps: Partial<SkeletonTitleProps>;
    render(): JSX.Element;
}
export default Title;

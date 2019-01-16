import * as React from 'react';
import * as PropTypes from 'prop-types';
export interface ColSize {
    span?: number;
    order?: number;
    offset?: number;
    push?: number;
    pull?: number;
}
export interface ColProps extends React.HTMLAttributes<HTMLDivElement> {
    span?: number;
    order?: number;
    offset?: number;
    push?: number;
    pull?: number;
    xs?: number | ColSize;
    sm?: number | ColSize;
    md?: number | ColSize;
    lg?: number | ColSize;
    xl?: number | ColSize;
    xxl?: number | ColSize;
    prefixCls?: string;
}
export default class Col extends React.Component<ColProps, {}> {
    static propTypes: {
        span: PropTypes.Requireable<string | number>;
        order: PropTypes.Requireable<string | number>;
        offset: PropTypes.Requireable<string | number>;
        push: PropTypes.Requireable<string | number>;
        pull: PropTypes.Requireable<string | number>;
        className: PropTypes.Requireable<string>;
        children: PropTypes.Requireable<PropTypes.ReactNodeLike>;
        xs: PropTypes.Requireable<number | object>;
        sm: PropTypes.Requireable<number | object>;
        md: PropTypes.Requireable<number | object>;
        lg: PropTypes.Requireable<number | object>;
        xl: PropTypes.Requireable<number | object>;
        xxl: PropTypes.Requireable<number | object>;
    };
    render(): JSX.Element;
}

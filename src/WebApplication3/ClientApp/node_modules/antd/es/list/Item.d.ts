import * as React from 'react';
import * as PropTypes from 'prop-types';
import { ListGridType } from './index';
export interface ListItemProps {
    className?: string;
    children?: React.ReactNode;
    prefixCls?: string;
    style?: React.CSSProperties;
    extra?: React.ReactNode;
    actions?: React.ReactNode[];
    grid?: ListGridType;
}
export interface ListItemMetaProps {
    avatar?: React.ReactNode;
    className?: string;
    children?: React.ReactNode;
    description?: React.ReactNode;
    prefixCls?: string;
    style?: React.CSSProperties;
    title?: React.ReactNode;
}
export declare const Meta: (props: ListItemMetaProps) => JSX.Element;
export default class Item extends React.Component<ListItemProps, any> {
    static Meta: typeof Meta;
    static propTypes: {
        column: PropTypes.Requireable<string | number>;
        xs: PropTypes.Requireable<string | number>;
        sm: PropTypes.Requireable<string | number>;
        md: PropTypes.Requireable<string | number>;
        lg: PropTypes.Requireable<string | number>;
        xl: PropTypes.Requireable<string | number>;
        xxl: PropTypes.Requireable<string | number>;
    };
    static contextTypes: {
        grid: PropTypes.Requireable<any>;
    };
    context: any;
    render(): JSX.Element;
}

import * as React from 'react';
import * as PropTypes from 'prop-types';
import BreadcrumbItem from './BreadcrumbItem';
export interface Route {
    path: string;
    breadcrumbName: string;
}
export interface BreadcrumbProps {
    prefixCls?: string;
    routes?: Route[];
    params?: any;
    separator?: React.ReactNode;
    itemRender?: (route: any, params: any, routes: Array<any>, paths: Array<string>) => React.ReactNode;
    style?: React.CSSProperties;
    className?: string;
}
export default class Breadcrumb extends React.Component<BreadcrumbProps, any> {
    static Item: typeof BreadcrumbItem;
    static defaultProps: {
        prefixCls: string;
        separator: string;
    };
    static propTypes: {
        prefixCls: PropTypes.Requireable<string>;
        separator: PropTypes.Requireable<PropTypes.ReactNodeLike>;
        routes: PropTypes.Requireable<any[]>;
        params: PropTypes.Requireable<object>;
        linkRender: PropTypes.Requireable<(...args: any[]) => any>;
        nameRender: PropTypes.Requireable<(...args: any[]) => any>;
    };
    componentDidMount(): void;
    render(): JSX.Element;
}

import * as React from 'react';
import * as PropTypes from 'prop-types';
export interface BreadcrumbItemProps {
    prefixCls?: string;
    separator?: React.ReactNode;
    href?: string;
}
export default class BreadcrumbItem extends React.Component<BreadcrumbItemProps, any> {
    static __ANT_BREADCRUMB_ITEM: boolean;
    static defaultProps: {
        prefixCls: string;
        separator: string;
    };
    static propTypes: {
        prefixCls: PropTypes.Requireable<string>;
        separator: PropTypes.Requireable<string | PropTypes.ReactElementLike>;
        href: PropTypes.Requireable<string>;
    };
    render(): JSX.Element | null;
}

import * as React from 'react';
import * as PropTypes from 'prop-types';
import { AntAnchor } from './Anchor';
export interface AnchorLinkProps {
    prefixCls?: string;
    href: string;
    title: React.ReactNode;
    children?: any;
}
export default class AnchorLink extends React.Component<AnchorLinkProps, any> {
    static defaultProps: {
        prefixCls: string;
        href: string;
    };
    static contextTypes: {
        antAnchor: PropTypes.Requireable<object>;
    };
    context: {
        antAnchor: AntAnchor;
    };
    componentDidMount(): void;
    componentWillReceiveProps(nextProps: AnchorLinkProps): void;
    componentWillUnmount(): void;
    handleClick: (e: React.MouseEvent<HTMLElement>) => void;
    render(): JSX.Element;
}

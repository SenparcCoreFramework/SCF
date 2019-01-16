import * as React from 'react';
import * as PropTypes from 'prop-types';
import { ClickParam } from './index';
interface MenuItemProps {
    rootPrefixCls?: string;
    disabled?: boolean;
    level?: number;
    title?: React.ReactNode;
    children?: React.ReactNode;
    className?: string;
    onClick?: (param: ClickParam) => void;
}
declare class MenuItem extends React.Component<MenuItemProps, any> {
    static contextTypes: {
        inlineCollapsed: PropTypes.Requireable<boolean>;
    };
    static isMenuItem: number;
    context: any;
    private menuItem;
    onKeyDown: (e: React.MouseEvent<HTMLElement>) => void;
    saveMenuItem: (menuItem: any) => void;
    render(): JSX.Element;
}
export default MenuItem;

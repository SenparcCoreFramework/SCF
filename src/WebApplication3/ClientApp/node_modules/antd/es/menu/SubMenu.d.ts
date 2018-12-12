import * as React from 'react';
import * as PropTypes from 'prop-types';
declare class SubMenu extends React.Component<any, any> {
    static contextTypes: {
        antdMenuTheme: PropTypes.Requireable<string>;
    };
    static isSubMenu: number;
    context: any;
    private subMenu;
    onKeyDown: (e: React.MouseEvent<HTMLElement>) => void;
    saveSubMenu: (subMenu: any) => void;
    render(): JSX.Element;
}
export default SubMenu;

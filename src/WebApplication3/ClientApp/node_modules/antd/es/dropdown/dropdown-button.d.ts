import * as React from 'react';
import { ButtonHTMLType } from '../button/button';
import { ButtonGroupProps } from '../button/button-group';
import { ConfigProviderProps } from '../config-provider';
import { DropDownProps } from './dropdown';
export interface DropdownButtonProps extends ButtonGroupProps, DropDownProps {
    type?: 'primary' | 'ghost' | 'dashed';
    htmlType?: ButtonHTMLType;
    disabled?: boolean;
    onClick?: React.MouseEventHandler<HTMLButtonElement>;
    children?: any;
}
export default class DropdownButton extends React.Component<DropdownButtonProps, any> {
    static defaultProps: {
        placement: string;
        type: string;
        prefixCls: string;
    };
    renderButton: ({ getPopupContainer: getContextPopupContainer }: ConfigProviderProps) => JSX.Element;
    render(): JSX.Element;
}

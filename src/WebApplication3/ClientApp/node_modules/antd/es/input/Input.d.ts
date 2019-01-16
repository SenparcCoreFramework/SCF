import * as React from 'react';
import * as PropTypes from 'prop-types';
import Group from './Group';
import Search from './Search';
import TextArea from './TextArea';
import { Omit } from '../_util/type';
export interface InputProps extends Omit<React.InputHTMLAttributes<HTMLInputElement>, 'size' | 'prefix'> {
    prefixCls?: string;
    size?: 'large' | 'default' | 'small';
    onPressEnter?: React.KeyboardEventHandler<HTMLInputElement>;
    addonBefore?: React.ReactNode;
    addonAfter?: React.ReactNode;
    prefix?: React.ReactNode;
    suffix?: React.ReactNode;
}
export default class Input extends React.Component<InputProps, any> {
    static Group: typeof Group;
    static Search: typeof Search;
    static TextArea: typeof TextArea;
    static defaultProps: {
        prefixCls: string;
        type: string;
        disabled: boolean;
    };
    static propTypes: {
        type: PropTypes.Requireable<string>;
        id: PropTypes.Requireable<string | number>;
        size: PropTypes.Requireable<string>;
        maxLength: PropTypes.Requireable<string | number>;
        disabled: PropTypes.Requireable<boolean>;
        value: PropTypes.Requireable<any>;
        defaultValue: PropTypes.Requireable<any>;
        className: PropTypes.Requireable<string>;
        addonBefore: PropTypes.Requireable<PropTypes.ReactNodeLike>;
        addonAfter: PropTypes.Requireable<PropTypes.ReactNodeLike>;
        prefixCls: PropTypes.Requireable<string>;
        onPressEnter: PropTypes.Requireable<(...args: any[]) => any>;
        onKeyDown: PropTypes.Requireable<(...args: any[]) => any>;
        onKeyUp: PropTypes.Requireable<(...args: any[]) => any>;
        onFocus: PropTypes.Requireable<(...args: any[]) => any>;
        onBlur: PropTypes.Requireable<(...args: any[]) => any>;
        prefix: PropTypes.Requireable<PropTypes.ReactNodeLike>;
        suffix: PropTypes.Requireable<PropTypes.ReactNodeLike>;
    };
    input: HTMLInputElement;
    handleKeyDown: (e: React.KeyboardEvent<HTMLInputElement>) => void;
    focus(): void;
    blur(): void;
    select(): void;
    getInputClassName(): any;
    saveInput: (node: HTMLInputElement) => void;
    renderLabeledInput(children: React.ReactElement<any>): React.ReactElement<any>;
    renderLabeledIcon(children: React.ReactElement<any>): React.ReactElement<any>;
    renderInput(): React.ReactElement<any>;
    render(): React.ReactElement<any>;
}

import * as React from 'react';
import Input, { InputProps } from './Input';
export interface SearchProps extends InputProps {
    inputPrefixCls?: string;
    onSearch?: (value: string, event?: React.MouseEvent<HTMLElement> | React.KeyboardEvent<HTMLInputElement>) => any;
    enterButton?: boolean | React.ReactNode;
}
export default class Search extends React.Component<SearchProps, any> {
    static defaultProps: {
        inputPrefixCls: string;
        prefixCls: string;
        enterButton: boolean;
    };
    private input;
    onSearch: (e: React.KeyboardEvent<HTMLInputElement> | React.MouseEvent<HTMLElement>) => void;
    focus(): void;
    blur(): void;
    saveInput: (node: Input) => void;
    getButtonOrIcon(): React.ReactElement<any>;
    render(): JSX.Element;
}

import * as React from 'react';
import * as PropTypes from 'prop-types';
export interface SwitchProps {
    prefixCls?: string;
    size?: 'small' | 'default';
    className?: string;
    checked?: boolean;
    defaultChecked?: boolean;
    onChange?: (checked: boolean) => any;
    checkedChildren?: React.ReactNode;
    unCheckedChildren?: React.ReactNode;
    disabled?: boolean;
    loading?: boolean;
    autoFocus?: boolean;
    style?: React.CSSProperties;
}
export default class Switch extends React.Component<SwitchProps, {}> {
    static defaultProps: {
        prefixCls: string;
    };
    static propTypes: {
        prefixCls: PropTypes.Requireable<string>;
        size: PropTypes.Requireable<string>;
        className: PropTypes.Requireable<string>;
    };
    private rcSwitch;
    focus(): void;
    blur(): void;
    saveSwitch: (node: any) => void;
    render(): JSX.Element;
}

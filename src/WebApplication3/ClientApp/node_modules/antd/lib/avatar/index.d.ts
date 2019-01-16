import * as React from 'react';
export interface AvatarProps {
    /** Shape of avatar, options:`circle`, `square` */
    shape?: 'circle' | 'square';
    size?: 'large' | 'small' | 'default' | number;
    /** Src of image avatar */
    src?: string;
    /** Srcset of image avatar */
    srcSet?: string;
    /** Type of the Icon to be used in avatar */
    icon?: string;
    style?: React.CSSProperties;
    prefixCls?: string;
    className?: string;
    children?: any;
    alt?: string;
    onError?: () => boolean;
}
export interface AvatarState {
    scale: number;
    isImgExist: boolean;
}
export default class Avatar extends React.Component<AvatarProps, AvatarState> {
    static defaultProps: {
        prefixCls: string;
        shape: string;
        size: string;
    };
    state: {
        scale: number;
        isImgExist: boolean;
    };
    private avatarChildren;
    componentDidMount(): void;
    componentDidUpdate(prevProps: AvatarProps, prevState: AvatarState): void;
    setScale: () => void;
    handleImgLoadError: () => void;
    render(): JSX.Element;
}

import * as React from 'react';
export interface CommentProps {
    /** List of action items rendered below the comment content */
    actions?: Array<React.ReactNode>;
    /** The element to display as the comment author. */
    author?: string;
    /** The element to display as the comment avatar - generally an antd Avatar */
    avatar?: React.ReactNode;
    /** className of comment */
    className?: string;
    /** The main content of the comment */
    content: React.ReactNode;
    /** Nested comments should be provided as children of the Comment */
    children?: any;
    /** Comment prefix defaults to '.ant-comment' */
    prefixCls?: string;
    /** Additional style for the comment */
    style?: React.CSSProperties;
    /** A datetime element containing the time to be displayed */
    datetime?: React.ReactNode;
}
export default class Comment extends React.Component<CommentProps, {}> {
    static defaultProps: {
        prefixCls: string;
    };
    getAction(actions: React.ReactNode[]): JSX.Element[] | null;
    renderNested: (children: any) => JSX.Element;
    render(): JSX.Element;
}

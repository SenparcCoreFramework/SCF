import * as React from 'react';
import * as moment from 'moment';
interface WeekPickerState {
    open: boolean;
    value: moment.Moment | null;
}
declare class WeekPicker extends React.Component<any, WeekPickerState> {
    static defaultProps: {
        format: string;
        allowClear: boolean;
    };
    static getDerivedStateFromProps(nextProps: any): WeekPickerState | null;
    private input;
    constructor(props: any);
    weekDateRender: (current: any) => JSX.Element;
    handleChange: (value: moment.Moment | null) => void;
    handleOpenChange: (open: boolean) => void;
    clearSelection: (e: React.MouseEvent<HTMLElement>) => void;
    focus(): void;
    blur(): void;
    saveInput: (node: any) => void;
    render(): JSX.Element;
}
export default WeekPicker;

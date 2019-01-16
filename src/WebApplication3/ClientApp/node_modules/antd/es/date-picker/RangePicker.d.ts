import * as React from 'react';
import { RangePickerValue, RangePickerPresetRange } from './interface';
export interface RangePickerState {
    value?: RangePickerValue;
    showDate?: RangePickerValue;
    open?: boolean;
    hoverValue?: RangePickerValue;
}
declare class RangePicker extends React.Component<any, RangePickerState> {
    static defaultProps: {
        prefixCls: string;
        tagPrefixCls: string;
        allowClear: boolean;
        showToday: boolean;
    };
    static getDerivedStateFromProps(nextProps: any, prevState: any): {
        open: any;
        value?: undefined;
    } | {
        open: any;
        showDate: any;
        value: any;
    } | {
        open: any;
        value: any;
    } | {
        showDate: any;
        value: any;
    } | {
        value: any;
    } | null;
    private picker;
    constructor(props: any);
    clearSelection: (e: React.MouseEvent<HTMLElement>) => void;
    clearHoverValue: () => void;
    handleChange: (value: RangePickerValue) => void;
    handleOpenChange: (open: boolean) => void;
    handleShowDateChange: (showDate: RangePickerValue) => void;
    handleHoverChange: (hoverValue: any) => void;
    handleRangeMouseLeave: () => void;
    handleCalendarInputSelect: (value: RangePickerValue) => void;
    handleRangeClick: (value: RangePickerPresetRange) => void;
    setValue(value: RangePickerValue, hidePanel?: boolean): void;
    focus(): void;
    blur(): void;
    savePicker: (node: HTMLSpanElement) => void;
    renderFooter: (...args: any[]) => (JSX.Element | null)[] | null;
    render(): JSX.Element;
}
export default RangePicker;

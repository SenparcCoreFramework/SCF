import * as React from 'react';
export interface ConfigProviderProps {
    getPopupContainer?: (triggerNode?: HTMLElement) => HTMLElement;
}
declare const ConfigProvider: React.SFC<ConfigProviderProps>;
export declare const ConfigConsumer: React.ComponentClass<import("create-react-context").ConsumerProps<ConfigProviderProps | null>, any>;
export default ConfigProvider;

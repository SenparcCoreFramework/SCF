import * as React from 'react';
import createReactContext from 'create-react-context';
var ConfigContext = createReactContext({});
var ConfigProvider = function ConfigProvider(props) {
    var getPopupContainer = props.getPopupContainer,
        children = props.children;

    var config = {
        getPopupContainer: getPopupContainer
    };
    return React.createElement(
        ConfigContext.Provider,
        { value: config },
        children
    );
};
export var ConfigConsumer = ConfigContext.Consumer;
export default ConfigProvider;
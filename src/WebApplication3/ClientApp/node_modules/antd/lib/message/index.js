'use strict';

Object.defineProperty(exports, "__esModule", {
    value: true
});

var _react = require('react');

var React = _interopRequireWildcard(_react);

var _rcNotification = require('rc-notification');

var _rcNotification2 = _interopRequireDefault(_rcNotification);

var _icon = require('../icon');

var _icon2 = _interopRequireDefault(_icon);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { 'default': obj }; }

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) newObj[key] = obj[key]; } } newObj['default'] = obj; return newObj; } }

var defaultDuration = 3; /* global Promise */

var defaultTop = void 0;
var messageInstance = void 0;
var key = 1;
var prefixCls = 'ant-message';
var transitionName = 'move-up';
var getContainer = void 0;
var maxCount = void 0;
function getMessageInstance(callback) {
    if (messageInstance) {
        callback(messageInstance);
        return;
    }
    _rcNotification2['default'].newInstance({
        prefixCls: prefixCls,
        transitionName: transitionName,
        style: { top: defaultTop },
        getContainer: getContainer,
        maxCount: maxCount
    }, function (instance) {
        if (messageInstance) {
            callback(messageInstance);
            return;
        }
        messageInstance = instance;
        callback(instance);
    });
}
function notice(args) {
    var duration = args.duration !== undefined ? args.duration : defaultDuration;
    var iconType = {
        info: 'info-circle',
        success: 'check-circle',
        error: 'close-circle',
        warning: 'exclamation-circle',
        loading: 'loading'
    }[args.type];
    var target = key++;
    var closePromise = new Promise(function (resolve) {
        var callback = function callback() {
            if (typeof args.onClose === 'function') {
                args.onClose();
            }
            return resolve(true);
        };
        getMessageInstance(function (instance) {
            var iconNode = React.createElement(_icon2['default'], { type: iconType, theme: iconType === 'loading' ? 'outlined' : 'filled' });
            instance.notice({
                key: target,
                duration: duration,
                style: {},
                content: React.createElement(
                    'div',
                    { className: prefixCls + '-custom-content' + (args.type ? ' ' + prefixCls + '-' + args.type : '') },
                    args.icon ? args.icon : iconType ? iconNode : '',
                    React.createElement(
                        'span',
                        null,
                        args.content
                    )
                ),
                onClose: callback
            });
        });
    });
    var result = function result() {
        if (messageInstance) {
            messageInstance.removeNotice(target);
        }
    };
    result.then = function (filled, rejected) {
        return closePromise.then(filled, rejected);
    };
    result.promise = closePromise;
    return result;
}
var api = {
    open: notice,
    config: function config(options) {
        if (options.top !== undefined) {
            defaultTop = options.top;
            messageInstance = null; // delete messageInstance for new defaultTop
        }
        if (options.duration !== undefined) {
            defaultDuration = options.duration;
        }
        if (options.prefixCls !== undefined) {
            prefixCls = options.prefixCls;
        }
        if (options.getContainer !== undefined) {
            getContainer = options.getContainer;
        }
        if (options.transitionName !== undefined) {
            transitionName = options.transitionName;
            messageInstance = null; // delete messageInstance for new transitionName
        }
        if (options.maxCount !== undefined) {
            maxCount = options.maxCount;
            messageInstance = null;
        }
    },
    destroy: function destroy() {
        if (messageInstance) {
            messageInstance.destroy();
            messageInstance = null;
        }
    }
};
['success', 'info', 'warning', 'error', 'loading'].forEach(function (type) {
    api[type] = function (content, duration, onClose) {
        if (typeof duration === 'function') {
            onClose = duration;
            duration = undefined;
        }
        return api.open({ content: content, duration: duration, type: type, onClose: onClose });
    };
});
api.warn = api.warning;
exports['default'] = api;
module.exports = exports['default'];
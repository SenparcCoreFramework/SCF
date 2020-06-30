function r_cookie(key) {
    var array = document.cookie.split(';');
    var result = array.filter(_ => _.split('=')[0] === key);
    if (result.length > 0) {
        return result[0].split('=')[1];
    } else {
        return null;
    }
}
/**
 * axios封装
 * 请求拦截、响应拦截、错误统一处理
 */
// 创建一个axios实例
var service = axios.create({
    timeout: 100000 // request timeout
});
// 设置post请求头
service.defaults.headers.post['Content-Type'] = 'application/x-www-form-urlencoded';
// 请求拦截
service.interceptors.request.use(
    config => {
        if (config.method.toUpperCase() === 'POST') {
            config.headers['RequestVerificationToken'] = window.document.getElementsByName('__RequestVerificationToken')[0].value;
        }
        config.headers['x-requested-with'] = 'XMLHttpRequest';
        return config;
    },
    error => {
        console.log(error); // for debug
        return Promise.reject(error);
    }
);
// 响应拦截器
service.interceptors.response.use(
    response => {
        if (response.status === 200) {
            if (response.data.success) {
                return Promise.resolve(response);
            } else {
                //请求已发出，其他状态
                app.$message({
                    message: response.data.msg || 'Error',
                    type: 'error',
                    duration: 5 * 1000
                });
                return Promise.resolve(response);
            }
        } else {
            app.$message({
                message: response.msg || 'Error',
                type: 'error',
                duration: 5 * 1000
            });
            return Promise.reject(response);
        }
    },
    error => {
        console.log('err' + error);
        if (error.message.includes('401')) {
            app.$message({
                message: '登陆过期，即将跳转到登录页面',
                type: 'error',
                duration: 3 * 1000,
                onClose: function () {
                    window.sessionStorage.removeItem('activeMenu');
                    window.location.href = '/Admin/Login?handler=Logout';
                }
            });
            return;
        }
        app.$message({
            message: error.message,
            type: 'error',
            duration: 5 * 1000
        });
        return Promise.reject(error);
    }
);

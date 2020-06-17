var vm = new Vue();
//3.在新的实例上使用组件

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
        //if (store.getters.token) {

        //    config.headers['Authorization'] = 'Bearer ' + getToken();
        //}
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
                vm.$message({
                    message: response.data.msg || 'Error',
                    type: 'error',
                    duration: 5 * 1000
                });
                return Promise.resolve(response);
            }
        } else {
            vm.$message({
                message: response.msg || 'Error',
                type: 'error',
                duration: 5 * 1000
            });
            return Promise.reject(response);
        }
    },
    error => {
        console.log('err' + error);
        vm.$message({
            message: error.message,
            type: 'error',
            duration: 5 * 1000
        });
        return Promise.reject(error);
    }
);

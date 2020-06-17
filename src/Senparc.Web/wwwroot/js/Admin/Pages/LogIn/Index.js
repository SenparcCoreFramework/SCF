var validatePass = (rule, value, callback) => {
    if (value === '') {
        callback(new Error('请输入密码'));
    } else {
        callback();
    }
};
var validateUser = (rule, value, callback) => {
    if (value === '') {
        callback(new Error('请输入用户名'));
    } else {
        callback();
    }
};
var app = new Vue({
    el: '#app',
    data: {
        ruleForm: {
            user: '',
            pass: ''
        },
        rules: {
            user: [
                { validator: validateUser, trigger: 'blur' }
            ],
            pass: [
                { validator: validatePass, trigger: 'blur' }
            ]
        },loading:false
    },
    mounted() {
    },
    methods: {
        submitForm(formName) {
            this.$refs[formName].validate((valid) => {
                this.loading = true;
                var url = "/Admin/Login?handler=Login";
                let data = {
                    Name: this.ruleForm.user,
                    Password: this.ruleForm.pass
                };
                if (valid) {
                    service.post(url, data).then(res => {
                        if (res.data.success) {
                            window.location.href = '/Admin/index';
                        } else {
                            console.log(res.data);
                        }
                    });
                    this.loading = false;
                } else {
                    console.log('error submit!!');
                    return false;
                }
            });
        }
    }
});
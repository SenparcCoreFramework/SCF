var helloworld = {
    template: '#HelloWorld-template',
    props: {
        level: {
            type: Number,
            required: true
        }
    }
};
new Vue({
    el: '#app',
    components: {
        helloworld
    }
});
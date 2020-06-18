const Store = new Vuex.Store({
    state: {
        pageSrc: ''
    },
    mutations: {
        changePageSrc(state, data) {
            state.pageSrc = data;
        }
    }
});

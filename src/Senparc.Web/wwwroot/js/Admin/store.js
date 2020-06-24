var Store = new Vuex.Store({
    state: {
        pageSrc: '',
        resourceCodes:[]
    },
    mutations: {
        changePageSrc(state, data) {
            state.pageSrc = data;
        },
        saveResourceCodes(state, data) {
            state.resourceCodes = data;
        }
    }
});

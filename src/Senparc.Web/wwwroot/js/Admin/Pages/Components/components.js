Vue.component('sidebar-item', {
    name: 'sidebar-item',
    template: `         <!--最里面一层-->
                      <template v-if="item.children.length<1">
                            <el-menu-item :index="item.index" @click="link(item)">
                                <span slot='title'>  <i class="el-icon-location"></i> {{item.menuName}}</span>
                            </el-menu-item>
                        </template>
                        <!--递归-->
                        <el-submenu v-else  :index="item.id">
                                <span slot='title'>  <i class="el-icon-location"></i> {{item.menuName}}</span>
                                <sidebar-item
                                    v-for="child in item.children"
                                    :key="child.id"
                                    :is-nest="true"
                                    :item="child"
                                    class="nest-menu"
                                  />
                        </el-submenu>
`,
    data() {
        return {
        };
    },
    props: {
        item: {
            type: Object,
            required: true
        }
    },
    methods: {
        link(item) {
            console.log(item);
            //window.location.href = item.url;
        }
    }
});
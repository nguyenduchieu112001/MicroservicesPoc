import Vue from "vue";
import VueResource from "vue-resource";
import App from "./App.vue";
import BootstrapVue, { BootstrapVueIcons, BAvatar, BForm } from "bootstrap-vue";
import router from "./router";
import "bootstrap/dist/css/bootstrap.css";
import "bootstrap-vue/dist/bootstrap-vue.css";
import "font-awesome/css/font-awesome.css";

Vue.config.productionTip = false;
Vue.use(BootstrapVue);
Vue.use(BootstrapVueIcons);
Vue.use(VueResource);

Vue.component("b-avatar", BAvatar);
Vue.component("b-form", BForm);

new Vue({
  router,
  render: (h) => h(App),
}).$mount("#app");

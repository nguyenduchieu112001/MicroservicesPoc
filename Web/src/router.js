import Vue from "vue";
import Router from "vue-router";
import Home from "./views/HomeView.vue";
import auth, { DETAILS_USER } from "./components/http/Auth";

Vue.use(Router);

function loadView(view) {
  // route level code-splitting
  // this generates a separate chunk (about.[hash].js) for this route
  // which is lazy-loaded when the route is visited.
  return () =>
    import(/* webpackChunkName: "view-[request]" */ `./views/${view}View.vue`);
}

function checkAdminUser() {
  if (auth && auth.isAuthenticated) {
    const user = localStorage.getItem(DETAILS_USER);
    var userTypes = JSON.parse(user).userTypes;
    return userTypes.some((element) => element === "ADMIN");
  }
}

export default new Router({
  routes: [
    {
      path: "/",
      name: "home",
      component: Home,
    },
    {
      path: "/chat",
      name: "chat",
      component: loadView("Chat"),
    },
    {
      path: "/chatbot",
      name: "chatbot",
      component: loadView("Chatbot"),
    },
    // {
    //   path: "/account",
    //   name: "account",
    //   component: loadView("Account"),
    // },
    {
      path: "/dashboard",
      name: "dashboard",
      component: loadView("Dashboard"),
      beforeEnter: (to, from, next) => {
        const isAdmin = checkAdminUser();

        if (isAdmin) {
          next();
        } else {
          next("/");
        }
      },
    },
    {
      path: "/products",
      name: "products",
      component: loadView("Products"),
    },
    {
      path: "/products/:productCode",
      name: "product",
      props: true,
      component: loadView("ProductDetails"),
    },
    {
      path: "/pricing",
      name: "createPricing",
      component: loadView("PricingCreate"),
      beforeEnter: (to, from, next) => {
        const isAdmin = checkAdminUser();

        if (isAdmin) {
          next();
        } else {
          next("/");
        }
      },
    },
    {
      path: "/policy/fromOffer/:offerNumber",
      name: "createPolicy",
      props: true,
      component: loadView("PolicyCreate"),
    },
    {
      path: "/policies",
      name: "policies",
      component: loadView("Policies"),
    },
    {
      path: "/policies/:policyNumber",
      name: "policyDetails",
      props: true,
      component: loadView("PolicyDetails"),
    },
    {
      path: "/payment",
      name: "payment",
      component: loadView("Payment"),
      beforeEnter: (to, from, next) => {
        const isAdmin = checkAdminUser();

        if (isAdmin) {
          next();
        } else {
          next("/");
        }
      },
    },
  ],
});

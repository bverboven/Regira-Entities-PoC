import type { RouteRecordRaw } from "vue-router"
import HomeView from "@/views/HomeView.vue"
import NotFound from "@/views/NotFound.vue"

export const routes: Array<RouteRecordRaw> = [
  {
    path: "/",
    name: "home",
    component: HomeView,
  },
  {
    path: "/about",
    name: "about",
    // route level code-splitting
    // this generates a separate chunk (About.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import("../views/AboutView.vue"),
  },
  {
    path: "/icons",
    name: "icons",
    component: () => import("../views/Icons.vue"),
  },
  // default routes
  {
    path: "/404",
    name: "notFound",
    component: NotFound,
    props: (to) => ({ url: to.query.url }),
    meta: {
      allowAnonymous: true,
    },
  },
  {
    name: "catchAll",
    path: "/:pathMatch(.*)*",
    redirect: (from) => ({ name: "notFound", query: { url: from.fullPath } }),
    meta: {
      allowAnonymous: true,
    },
  },
]

export default routes

import { createRouter, createWebHistory, type RouteRecordRaw } from "vue-router"
import defaultRoutes from "./routes"

function routerFactory(routes: Array<RouteRecordRaw>) {
  const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [...defaultRoutes, ...routes],
    linkActiveClass: "active",
  })

  return router
}

export default routerFactory

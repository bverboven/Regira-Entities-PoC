import type { App } from "vue"
import type { RouteRecordRaw } from "vue-router"

import { plugin as coursePlugin } from "./courses"
import { plugin as departmentPlugin } from "./departments"
import { plugin as instructorPlugin } from "./instructors"
import { plugin as studentPlugin } from "./students"

// order is important -> cf HomeView
export const plugins = [coursePlugin, departmentPlugin, instructorPlugin, studentPlugin]

export default {
  install(app: App<Element>, { routes }: { routes: Array<RouteRecordRaw> }) {
    app.config.globalProperties.$configs = {}

    plugins.forEach((plugin) => app.use(plugin, { routes }))
  },
}

export { coursePlugin }
export { departmentPlugin }
export { instructorPlugin }
export { studentPlugin }

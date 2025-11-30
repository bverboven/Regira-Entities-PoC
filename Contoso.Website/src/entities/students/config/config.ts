import type { IConfig } from "@/regira_modules/vue/entities"
import Entity from "../data/Entity"

const api = "/students"

const config: IConfig = {
  id: Entity.name,
  key: "Student",

  routePrefix: "students",
  baseQueryParams: {
  },
  initialQuery: {},

  overviewTitle: "Students",
  detailsTitle: "Student",
  description: "Manage Students",
  icon: "bi bi-person-square",

  defaultPageSize: 10,

  api,
  detailsUrl: api,
  listUrl: api,
  searchUrl: api,
  saveUrl: api,
  deleteUrl: api,
}

export default config

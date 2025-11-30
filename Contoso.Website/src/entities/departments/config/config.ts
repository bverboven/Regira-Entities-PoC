import type { IConfig } from "@/regira_modules/vue/entities"
import Entity from "../data/Entity"

const api = "/departments"

const config: IConfig = {
  id: Entity.name,
  key: "Department",

  routePrefix: "departments",
  baseQueryParams: {
    includes: [],
  },
  initialQuery: {},

  overviewTitle: "Departments",
  detailsTitle: "Department",
  description: "Manage departments",
  icon: "bi bi-building-fill-gear",

  defaultPageSize: 10,

  api,
  detailsUrl: api,
  listUrl: api,
  searchUrl: api,
  saveUrl: api,
  deleteUrl: api,
}

export default config

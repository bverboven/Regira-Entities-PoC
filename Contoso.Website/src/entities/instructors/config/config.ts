import type { IConfig } from "@/regira_modules/vue/entities"
import Entity from "../data/Entity"

const api = "/instructors"

const config: IConfig = {
  id: Entity.name,
  key: "Instructor",

  routePrefix: "instructors",
  baseQueryParams: {
  },
  initialQuery: {},

  overviewTitle: "instructors",
  detailsTitle: "instructor",
  description: "instructorsDescription",
  icon: "bi bi-person-gear",

  defaultPageSize: 10,

  api,
  detailsUrl: api,
  listUrl: api,
  searchUrl: api,
  saveUrl: api,
  deleteUrl: api,
}

export default config

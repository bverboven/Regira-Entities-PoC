import type { IConfig } from "@/regira_modules/vue/entities"
import Entity from "../data/Entity"

const api = "/courses"

const config: IConfig = {
  id: Entity.name,
  key: "Course",

  routePrefix: "courses",
  baseQueryParams: {
    includes: ["Department"],
  },
  initialQuery: {},

  overviewTitle: "courses",
  detailsTitle: "course",
  description: "coursesDescription",
  icon: "bi bi-wrench-adjustable",

  defaultPageSize: 10,

  api,
  detailsUrl: api,
  listUrl: api,
  searchUrl: api + "/search",
  saveUrl: api,
  deleteUrl: api,
}

export default config

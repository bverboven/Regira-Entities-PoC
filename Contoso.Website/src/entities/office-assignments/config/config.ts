import type { IConfig } from "@/regira_modules/vue/entities"
import Entity from "../data/Entity"

const api = "/office-assignments"

const config: IConfig = {
    id: Entity.name,
    key: "OfficeAssignment",

    routePrefix: "office-assignments",
    baseQueryParams: {
        includes: [],
    },
    initialQuery: {},

    overviewTitle: "Office Assignments",
    detailsTitle: "Office Assignment",
    description: "Manage Office Assignments",
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

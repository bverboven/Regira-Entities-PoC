import { SearchObjectBase } from "@/regira_modules/vue/entities"

export class EntitySearchObject extends SearchObjectBase {
  title?: string

  departmentId?: number | Array<number>
  instructorId?: number | Array<number>

  minCreated?: Date
  maxCreated?: Date
  minLastModified?: Date
  maxLastModified?: Date

  isArchived?: boolean
}

export default EntitySearchObject

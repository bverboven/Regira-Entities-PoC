import { SearchObjectBase } from "@/regira_modules/vue/entities"

export class EntitySearchObject extends SearchObjectBase {
  id?: string[]
  title?: string
  instructorId?: number
  budgetMin?: number
  budgetMax?: number
  dateMin?: Date
  dateMax?: Date

  isArchived?: boolean
}

export default EntitySearchObject

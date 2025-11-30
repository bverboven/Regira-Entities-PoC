import { EntityBase } from "@/regira_modules/vue/entities"

export class Department extends EntityBase {
  id: string
  title: string
  budget?: number
  instructorId?: number
  startDate?: Date
  created?: Date
  lastModified?: Date
  isArchived?: boolean

  override get $id(): string | number {
    return this.id || "new"
  }
  override get $title(): string | undefined {
    return this.title
  }
}

export const Entity = Department

export default Department

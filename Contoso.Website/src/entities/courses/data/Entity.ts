import { EntityBase } from "@/regira_modules/vue/entities"
import type { Entity as Department } from "../../departments"
import type { Entity as Instructor } from "../../instructors"
import type { Entity as EntityLabel } from "../../entity-labels"
import type { Entity as EntityAttachment } from "../../entity-attachments"

export class Course extends EntityBase {
  id: number = 0
  title: string
  departmentId: number
  description?: string

  created?: Date
  lastModified?: Date

  isArchived?: boolean

  department?: Department
  instructors?: Array<Instructor>
  labels?: Array<EntityLabel>
  attachments?: Array<EntityAttachment>

  override get $id(): string | number {
    return this.id || "new"
  }
  override get $title(): string | undefined {
    return `${this.title || "New course"}`
  }
}

export const Entity = Course

export default Course

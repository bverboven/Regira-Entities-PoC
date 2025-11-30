import { EntityBase } from "@/regira_modules/vue/entities"
import type { Entity as EntityAttachment } from "@/entities/entity-attachments"

export class Instructor extends EntityBase {
  id: number = 0
  givenName: string
  lastName: string

  created?: Date
  lastModified?: Date
  isArchived?: boolean

  attachments?: Array<EntityAttachment>

  override get $id(): string | number {
    return this.id || "new"
  }
  override get $title(): string | undefined {
    return `${this.givenName || ""} ${this.lastName || ""}`.trim() || ""
  }
}

export const Entity = Instructor

export default Instructor

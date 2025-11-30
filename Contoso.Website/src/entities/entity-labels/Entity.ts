import { EntityBase, type IEntity } from "@/regira_modules/vue/entities"

export class EntityLabel extends EntityBase {
    id: number = 0
    objectId?: number
    title?: string
    value: string
    labelType?: string
    created: Date
    lastModified?: Date

    _owningEntity?: IEntity

    _deleted: boolean

    override get $id(): string | number {
        return this.id || "new"
    }
    override get $title(): string | undefined {
        return this.value
    }

    static create(values?: object): EntityLabel {
        return Object.assign(new EntityLabel(), values || {})
    }
}

export const Entity = EntityLabel

export default EntityLabel

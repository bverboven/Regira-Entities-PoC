import { EntityBase } from "@/regira_modules/vue/entities"

export class Attachment extends EntityBase {
    id: number = 0
    fileName?: string
    contentType?: string
    length?: number
    created?: Date
    lastModified?: Date

    _file?: Blob & { name?: string }

    override get $id(): string | number {
        return this.id || "new"
    }
    override get $title(): string | undefined {
        return this.fileName
    }

    static create(values?: object): Attachment {
        return Object.assign(new Attachment(), values || {})
    }
}

export const Entity = Attachment

export default Attachment

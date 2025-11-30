import { EntityBase } from "@/regira_modules/vue/entities"
import Attachment from "../attachments/Entity"

export class EntityAttachment extends EntityBase {
    id: number = 0
    title?: string
    objectId?: number
    objectType?: string
    attachmentId?: number
    sortOrder?: number
    uri?: string

    newFileName?: string
    newContentType?: string
    attachment?: Attachment

    _deleted: boolean = false

    override get $id(): string | number {
        return this.id || "new"
    }
    override get $title(): string | undefined {
        return this.title
    }
    get fileName() {
        return this.attachment?.fileName
    }
    set fileName(value) {
        this.attachment ??= new Attachment()
        this.attachment.fileName = value
    }

    static create(values?: object): EntityAttachment {
        if ((values as EntityAttachment)?.attachment && !((values as EntityAttachment)?.attachment instanceof Attachment)) {
            ;(values as EntityAttachment).attachment = Attachment.create((values as EntityAttachment).attachment)
        }

        const item = Object.assign(new EntityAttachment(), values || {})
        if (item.id > 0 && item.fileName && !item.newFileName) {
            item.newFileName = item.fileName
        }
        return item
    }
}

export const Entity = EntityAttachment

export default EntityAttachment

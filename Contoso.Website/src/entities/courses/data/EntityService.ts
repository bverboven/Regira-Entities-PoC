import { type AxiosWithFilesInstance, createQueryString } from "@/regira_modules/vue/http"
import { EntityServiceBase, type ListResult, type IConfig } from "@/regira_modules/vue/entities"
import { Entity as EntityAttachment, insertWithAttachments, updateWithAttachments, createEntity, save as saveAttachments } from "../../entity-attachments"
import Entity from "./Entity"

export class EntityService extends EntityServiceBase<Entity> {
    constructor(axios: AxiosWithFilesInstance, config: IConfig) {
        super(axios, config)
    console.debug("CourseService", this, { config })
    }

    async getAttachments(so?: object): Promise<Array<EntityAttachment>> {
        const url = `${this.config.api}/attachments`
        const queryString = createQueryString(so || {})
        const {
            data: { items },
        } = await this.axios.get<ListResult<EntityAttachment>>(`${url}?${queryString}`)

        return items.map((x) => EntityAttachment.create(x))
    }
    async addAttachment(itemId: number, file: Blob): Promise<EntityAttachment> {
        const url = `${this.config.api}/${itemId}/files`
        const attachment = createEntity(file)
        await saveAttachments(url, [attachment])
        return attachment
    }

    override async insert(item: Entity): Promise<Entity | null> {
        return await insertWithAttachments(this.config.api, item, async () => await super.insert(item))
    }
    override async update(item: Entity): Promise<Entity | null> {
        return await updateWithAttachments(this.config.api, item, async () => await super.update(item))
    }

    protected override processItem(item: Entity | null) {
        item = super.processItem(item);

        if (item != null) {
            if (item.courseDate) {
                item.courseDate = new Date(item.courseDate)
            }
            if (item.invoice?.invoiceDate) {
                item.invoice.invoiceDate = new Date(item.invoice.invoiceDate)
            }
        }

        return item
    }
    protected override prepareItem(item: Entity): Entity {
        item.labels = item.labels?.filter((x) => !x._deleted)
        item.attachments = item.attachments?.filter((x) => !x._deleted)
        return super.prepareItem(item)
    }

    override toEntity(item: object): Entity {
        return item instanceof Entity ? item : Object.assign(this.createInstance(Entity as new () => Entity), item || {})
    }
}

export default EntityService

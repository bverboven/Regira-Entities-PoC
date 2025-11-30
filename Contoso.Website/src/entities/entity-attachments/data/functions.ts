import { computed } from "vue"
import { browse as browseFiles, fileToBlob, saveAs } from "@/regira_modules/utilities/file-utility"
import { min } from "@/regira_modules/utilities/array-utility"
import { enqueue } from "@/regira_modules/utilities/promise-utility"
import { useAxios } from "@/regira_modules/vue/http/axios"
import type { IEntity } from "@/regira_modules/vue/entities/abstractions/IEntity"
import { Entity as Attachment } from "../attachments/Entity"
import Entity from "./Entity"

export function createEntity(file: Blob & { name?: string }): Entity {
    const item = new Entity()
    item.attachment = new Attachment()
    item.attachment._file = file
    item.attachment.fileName = file.name
    item.attachment.contentType = file.type
    item.attachment.length = file.size
    item.uri = URL.createObjectURL(file)
    return item
}
export function browse({ multiple = true, accept }: { multiple?: boolean; accept?: string } = {}): Promise<Array<Blob>> {
    return browseFiles({ multiple, accept })
}

export async function insertWithAttachments<T extends IEntity & { id: number; attachments?: Array<Entity> }>(api: string, item: T, insertFunc: () => Promise<T | null>): Promise<T | null> {
    if (!item.attachments?.length) {
        return await insertFunc()
    }
    const attachments = item.attachments
    delete item.attachments
    const saved = await insertFunc()
    saved!.attachments = attachments
    await saveAll(api, saved!)
    attachments!.forEach((x) => delete x.attachment!._file)
    return saved
}
export async function updateWithAttachments<T extends IEntity & { id: number; attachments?: Array<Entity> }>(api: string, item: T, updateFunc: () => Promise<T | null>): Promise<T | null> {
    await saveAll(api, item)
    return await updateFunc()
}
export async function saveAll<T extends IEntity & { id: number; attachments?: Array<Entity> }>(api: string, item: T) {
    if (item.attachments?.length) {
        await save(`${api}/${item!.id}/files`, item.attachments!)
    }
}
export async function save(api: string, items: Array<Entity>) {
    const promises = items.map((item) => {
        if (item.attachment?._file != null) {
            const axios: any = useAxios()
            return async () => {
                if (item.fileName != null && item.fileName != item.attachment!._file!.name) {
                    item.attachment!._file = await fileToBlob(item.attachment!._file, item.fileName)
                }
                const {
                    data: { item: saved },
                } = await axios.upload(api, [item.attachment?._file], { ...item })
                item.objectId = saved.objectId
                item.id = saved.id
                item.attachmentId = saved.attachmentId
                if (item.attachment != null && saved?.attachment != null) {
                    item.attachment.id = saved.attachment.id
                }
            }
        }
        return () => null
    })
    await enqueue(promises)
}
export async function download(item: Entity) {
    const axios: any = useAxios()
    const file = item.attachment?._file || (await axios.getFile(item.uri))
    await saveAs(file, item.fileName)
}

// export function toEntity(item: object = {}) {
//     return item instanceof Entity ? item : Object.assign(new Entity(), item || {})
// }

type Input<T> = {
    props: { modelValue?: Array<T> }
    emit: any
}
export function useEntityAttachments<T extends Entity>({ props, emit }: Input<T>) {
    const items = computed({
        get() {
            return props.modelValue?.map((x) => Entity.create(x)) || ([] as Array<T>)
        },
        set(value) {
            emit("update:modelValue", value)
        },
    })

    async function triggerBrowse({ multiple, accept }: { multiple?: boolean; accept?: string } = {}) {
        const files = await browse({ multiple, accept })
        handleBrowse(files)
    }
    function handleBrowse(files: Array<Blob>) {
        const entityAttachments = files.map((file) => createEntity(file))
        entityAttachments.forEach((item) => {
            const minId = Math.min(min(items.value, (x) => x.id) || 0, 0) - 1
            item.id = minId
        })
        emit("update:modelValue", [...items.value, ...entityAttachments])
    }

    return {
        items,
        triggerBrowse,
        handleBrowse,
    }
}

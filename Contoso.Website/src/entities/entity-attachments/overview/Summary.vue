<template>
    <FormSection title="Attachments">
        <div class="row" v-for="item in items">
            <div class="col-md mb-2">
                <div class="input-group">
                    <span class="input-group-text drag-handle cursor move">
                        <Icon name="move" />
                    </span>
                    <a :href="item.uri" target="_blank" class="btn btn-outline-info" @click.prevent="handleDownload(item)"><Icon name="download" /></a>
                    <input v-model="item.fileName" class="form-control" />
                    <span class="input-group-text" style="width: 5rem; overflow: hidden">
                        <span class="w-100 text-end">
                            {{ formatFileSize(item.attachment?.length) }}
                        </span>
                    </span>
                </div>
            </div>
        </div>
    </FormSection>
</template>

<script setup lang="ts">
import { formatFileSize } from "@/regira_modules/utilities/file-utility"
import type Entity from "../data/Entity"
import { download } from "../data/functions"

defineProps<{
    items: Array<Entity>
}>()

async function handleDownload(item: Entity) {
    await download(item)
}
</script>

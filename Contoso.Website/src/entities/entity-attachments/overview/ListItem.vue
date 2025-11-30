<template>
    <div class="row">
        <div class="col-md mb-2">
            <div class="input-group">
                <span v-if="!readonly" class="input-group-text drag-handle cursor move">
                    <Icon name="move" />
                </span>
                <a :href="item.uri" target="_blank" class="btn btn-outline-info" @click.prevent="handleDownload"><Icon name="download" /></a>
                <input
                    v-if="item.id > 0"
                    v-model.lazy="item.newFileName"
                    @update:model-value="onChangeFilename"
                    maxlength="256"
                    class="form-control"
                    :readonly="readonly"
                    :placeholder="item.fileName"
                    autocomplete="__away"
                />
                <input v-else v-model.lazy="item.fileName" :readonly="readonly" @update:model-value="onChangeFilename" maxlength="256" class="form-control" autocomplete="__away" />
                <span class="input-group-text" style="width: 5rem; overflow: hidden">
                    <span class="w-100 text-end">
                        {{ formatFileSize(item.attachment?.length) }}
                    </span>
                </span>
                <FormModalButton v-if="!readonly" v-model="item" @save="$emit('update:modelValue', item)" class="btn btn-outline-secondary"><Icon name="edit" /></FormModalButton>
                <button v-if="!readonly && item.id != 0" type="button" class="btn btn-outline-danger" @click="handleRemove"><Icon name="delete" /></button>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { useVModelField } from "@/regira_modules/vue/vue-helper"
import { formatFileSize } from "@/regira_modules/utilities/file-utility"
import type Entity from "../data/Entity"
import { download } from "../data/functions"
import FormModalButton from "../details/FormModalButton.vue"

const emit = defineEmits<{
    (e: "update:modelValue", item: Entity): void
    (e: "remove", item: Entity): void
}>()
const props = defineProps<{
    modelValue: Entity
    readonly?: boolean
}>()

const item = useVModelField<Entity>(props, emit)

function handleRemove() {
    item.value._deleted = !item.value._deleted
    emit("remove", item.value)
}
async function handleDownload() {
    await download(item.value)
}

function onChangeFilename() {
    emit("update:modelValue", item.value)
}
</script>

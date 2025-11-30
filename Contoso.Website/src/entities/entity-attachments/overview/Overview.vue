<template>
    <FormSection :title="$t('files')">
        <List v-model="items" :readonly="readonly" />
        <FileDropZone v-if="!readonly" @drop-files="handleBrowse" @click="triggerBrowse()">
            <template #default="{ isDropping }">
                <div class="file-drop-zone row align-items-center justify-content-center m-2 mb-3" :class="{ dropping: isDropping }" style="min-height: 20rem">
                    <div class="col-auto">
                        <span class="italic-muted text-info">{{ $t("addNewFile(s)") }}</span>
                    </div>
                </div>
            </template>
        </FileDropZone>
        <Debug :modelValue="items" />
    </FormSection>
</template>

<script setup lang="ts">
import { FileDropZone } from "@/regira_modules/vue/ui"
import { type Entity, List, useEntityAttachments } from "../"

const emit = defineEmits<{
    (e: "update:modelValue", args: Array<Entity>): void
    (e: "sort", args: any): void
}>()
const props = withDefaults(
    defineProps<{
        modelValue?: Array<Entity>
        readonly?: boolean
    }>(),
    {
        modelValue: () => [],
    }
)

const { items, triggerBrowse, handleBrowse } = useEntityAttachments({ props, emit })
</script>

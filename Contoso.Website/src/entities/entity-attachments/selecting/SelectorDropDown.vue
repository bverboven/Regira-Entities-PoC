<template>
    <select v-model="selected" class="form-select">
        <slot name="empty"><option value=""></option></slot>
        <option v-for="item in items" :value="item" :key="item.id">{{ item.fileName }} ({{ formatFileSize(item.attachment?.length) }})</option>
    </select>
</template>

<script setup lang="ts">
import { computed, watchEffect } from "vue"
import { formatFileSize } from "@/regira_modules/utilities/file-utility"
import type Entity from "../data/Entity"

const emit = defineEmits<{
    (e: "update:modelValue", args?: Entity): void
    (e: "update:idValue", args?: number): void
}>()
const props = defineProps<{
    modelValue?: Entity
    items: Array<Entity>
    idValue?: number
}>()

const selected = computed({
    get() {
        return props.modelValue
    },
    set(value) {
        emit("update:idValue", value?.id)
        emit("update:modelValue", value)
    },
})

watchEffect(() => {
    if (props.idValue && props.items.length && (selected.value == null || !props.items.includes(selected.value))) {
        const item = props.items.find((x) => x.id == props.idValue)
        if (item != null) {
            selected.value = item
        }
    }
})
</script>

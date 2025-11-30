<template>
    <Draggable v-model="items" item-key="id" handle=".drag-handle" ghost-class="ghost">
        <template #item="{ element, index }">
            <ListItem v-model="items[index]" :class="{ 'is-deleted': element._deleted }" :readonly="readonly" @update:model-value="emit('update:modelValue', items)" @remove="emit('update:modelValue', items)" />
        </template>
    </Draggable>
</template>

<script setup lang="ts">
import { useVModelField } from "@/regira_modules/vue/vue-helper"
import Draggable from "vuedraggable"
import type Entity from "../data/Entity"
import ListItem from "./ListItem.vue"

const emit = defineEmits<{
    (e: "update:modelValue", args: Array<Entity>): void
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

const items = useVModelField<Array<Entity>>(props, emit)
</script>

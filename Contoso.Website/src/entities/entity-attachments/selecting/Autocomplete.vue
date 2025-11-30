<template>
    <Autocomplete v-model="item" :search="search" :id-selector="idSelector" :display-item-formatter="displayItemFormatter" :placeholder="placeholder" ref="autoEl">
        <template #default="{ item, q }">
            <div class="row">
                <div class="col">{{ item?.attachment?.fileName }}</div>
            </div>
        </template>
    </Autocomplete>
</template>

<script setup lang="ts">
import { ref, computed } from "vue"
import { Autocomplete } from "@/regira_modules/vue/ui"

import Entity from "../data/Entity"

const emit = defineEmits<{
    (e: "update:modelValue", args?: Entity): void
    (e: "update:idValue", args?: number): void
    (e: "select", args?: Entity): void
}>()
const props = defineProps<{
    modelValue?: Entity
    items: Array<Entity>
    placeholder?: string
}>()

const item = computed({
    get: () => props.modelValue as Entity,
    set: (value) => {
        emit("update:modelValue", value)
        emit("update:idValue", value?.id)
        emit("select", value)
    },
})

// expose refs
const autoEl = ref<any>(null)
defineExpose({
    autoEl,
    get inputEl() {
        return autoEl.value.inputEl
    },
    resetQ: () => autoEl.value.resetQ(),
    search: (q?: string) => autoEl.value.search(q),
    reset: () => autoEl.value.reset(),
})

const search = (q: string) => props.items.filter((item) => item.attachment?.fileName?.startsWith(q))
const idSelector = (item?: Entity) => item?.$id?.toString()
const displayItemFormatter = (item?: Entity) => item?.attachment?.fileName as string
</script>

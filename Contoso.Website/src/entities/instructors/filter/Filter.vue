<template>
    <div>
        <slot name="inline" :handleToggle="handleToggle">
            <FilterInline v-model="searchObject" @filter="handleFilter" @toggle-adv="handleToggle" />
        </slot>

        <Teleport to="#modals">
            <MyModal :is-visible="showAdv" :title="props.modalTitle || 'Advanced search'" :show-footer="true" :full-width="true" @close="handleClose" @submit="handleSubmit">
                <slot name="title"></slot>
                <slot name="adv" :handleUpdate="handleUpdate" :handleSubmit="handleSubmit" :handleClose="handleClose"></slot>

                <Debug :modelValue="searchObject" />
            </MyModal>
        </Teleport>
    </div>
</template>

<script setup lang="ts">
import { ref } from "vue"
import { useVModelField } from "@/regira_modules/vue/vue-helper"
import { useFilter, type FilterEmits } from "@/regira_modules/vue/entities"
import type SearchObject from "./SearchObject"
import FilterInline from "./FilterInline.vue"

interface Emits extends /* @vue-ignore */ FilterEmits {}

const emit = defineEmits<Emits>()
const props = defineProps<{
    modelValue: object
    modalTitle?: string
    resultCount?: number
}>()

const searchObject = useVModelField<SearchObject>(props, emit)

const showAdv = ref(false)
const { handleUpdate, handleFilter } = useFilter({ searchObject, emit })

function handleToggle() {
    showAdv.value = !showAdv.value
}
function handleClose() {
    showAdv.value = false
}
function handleSubmit() {
    handleUpdate()
    handleClose()
}
</script>

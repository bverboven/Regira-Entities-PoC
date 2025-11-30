<template>
    <button type="button" class="btn btn-default" @click="open">
        <slot><Icon :name="Entity.name" /></slot>
        <Teleport to="#modals">
            <MyModal :is-visible="isOpen" :title="modalTitle || $tm(config.detailsTitle)" :showFooter="false" :full-width="true" @close="close" @cancel="handleCancel" @submit="handleSave">
                <Form v-model="item" :initial-tab="initialTab" :readonly="readonly" :is-popup="true" @cancel="handleCancel" @save="handleSave" @remove="handleRemove" />
            </MyModal>
        </Teleport>
    </button>
</template>

<script setup lang="ts">
import { computed, type Ref } from "vue"
import { useVModelField } from "@/regira_modules/vue/vue-helper"
import { useModal, type FormModalEmits } from "@/regira_modules/vue/entities"
import config from "../config/config"
import Entity from "../data/Entity"
import useEntityStore from "../data/store"
import Form from "./Form.vue"

interface Emits extends /* @vue-ignore */ FormModalEmits<Entity> {}

const emit = defineEmits<Emits>()
const props = defineProps<{
    modelValue?: Entity
    readonly?: boolean
    itemDefaults?: Ref<Record<string, any>> | Record<string, any>
    initialTab?: string
    label?: string
    closeOnSave?: boolean
}>()

const modelRef = useVModelField<Entity>(props, emit)
const { service: entityService } = useEntityStore()

const modalTitle = computed(() => props.label || entityService.toEntity(modelRef.value).$title)
const {
    item,
    isOpen,

    close,
    open,

    handleSave,
    handleRemove,
    handleCancel,
} = useModal<Entity>({ entityService, model: modelRef, itemDefaults: props.itemDefaults, closeOnSave: props.closeOnSave, closeOnCancel: false, closeOnDelete: true, emit })

defineExpose({
    item,
    isOpen,
    open,
    close,
})
</script>

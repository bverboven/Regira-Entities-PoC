<template>
    <div class="input-selector input-group text-nowrap">
        <slot name="prepend">
            <FormModalButton v-model="item" :item-defaults="itemDefaults" :close-on-save="closeOnSave" class="btn btn-outline-secondary" @save="({ saved }) => handleSelect(saved)">
                <Icon :name="Entity.name" v-if="item?.id" /><Icon v-else name="new" />
            </FormModalButton>
        </slot>
        <slot>
            <Autocomplete class="form-control" v-model="item" :filter-defaults="filterDefaults" :placeholder="placeholder" @select="handleSelect" ref="autoEl" />
        </slot>
        <slot name="append">
            <button v-if="!readonly" type="button" v-show="item != null" class="btn btn-outline-secondary" @click="handleSelect(undefined)"><Icon name="clear" /></button>
            <SelectorModalButton v-model="item" :filter-defaults="filterDefaults" :disabled="readonly" @select="handleSelect" class="btn btn-outline-info" />
        </slot>
    </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, type Ref } from "vue"
import Entity from "../data/Entity"
import useEntityStore from "../data/store"
import FormModalButton from "../details/FormModalButton.vue"
import Autocomplete from "./Autocomplete.vue"
import SelectorModalButton from "./SelectorModalButton.vue"

const emit = defineEmits<{
    (e: "update:modelValue", args?: Entity): void
    (e: "update:idValue", args?: number | string): void
    (e: "select", args?: Entity): void
}>()
const props = defineProps<{
    modelValue?: Entity
    idValue?: number | string
    itemDefaults?: Ref<Record<string, any>> | Record<string, any>
    filterDefaults?: Record<string, any>
    closeOnSave?: boolean
    placeholder?: string
}>()

const { fromPool, list } = useEntityStore()
const item = computed<Entity | undefined>({
    get: () => fromPool(props.modelValue) as Entity,
    set: (value) => {
        emit("update:modelValue", value)
        emit("update:idValue", value?.id)
    },
})
const autoEl = ref<any>(null)
defineExpose({
    resetQ: () => autoEl.value.resetQ(),
})

function handleSelect(selected?: Entity) {
    if (item.value?.id != selected?.id) {
        item.value = selected // emit
        emit("select", selected)
    }

    if (selected == null) {
        // empty q in autocomplete
        const { resetQ } = autoEl.value
        resetQ()
    }
}

onMounted(async () => {
    if (props.idValue && !props.modelValue?.id) {
        const model = await list({ id: props.idValue })
        emit("update:modelValue", model[0])
    }
})
</script>

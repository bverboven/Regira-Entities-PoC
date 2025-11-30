<template>
    <div class="input-group">
        <span class="input-group-text drag-handle cursor move">
            <Icon name="move" />
        </span>
        <span class="input-group-text"><Icon name="tag" /></span>
        <input v-model.trim="item.title" maxlength="256" class="form-control" @keydown.enter.prevent="handleUpdate" :placeholder="$t('labelName')" ref="labelTitleInput" />
        <span class="input-group-text"><LabelIcon :item="item" /></span>
        <input
            v-model.trim="item.value"
            maxlength="256"
            class="form-control"
            @blur="handleUpdate"
            @keydown.enter.prevent="handleUpdate"
            :required="!!item.id"
            :placeholder="$t('labelValue')"
            autocomplete="__away"
            ref="labelValueInput"
        />
        <button v-if="!item.id" type="button" class="btn btn-outline-info" @click.prevent="handleUpdate"><Icon name="new" /></button>
        <button v-else type="button" class="btn btn-outline-danger" @click.prevent="handleRemove"><Icon name="delete" /></button>
    </div>
</template>

<script setup lang="ts">
import { ref } from "vue"
import { useVModelField } from "@/regira_modules/vue"
import Entity from "./Entity"
import { getLabelType } from "./functions"
import LabelIcon from "./LabelIcon.vue"

const emit = defineEmits<{
    (e: "update:modelValue", arg: Entity): void
}>()
const props = withDefaults(
    defineProps<{
        modelValue?: Entity
    }>(),
    {
        modelValue: () => new Entity(),
    }
)

const item = useVModelField<Entity>(props, emit)

function handleUpdate() {
    if (item.value.value) {
        console.debug("handleUpdate", { item: item.value })
        item.value.labelType = getLabelType(item.value.value).toString()
        emit("update:modelValue", item.value)
    }
}
function handleRemove() {
    item.value._deleted = !item.value._deleted
    emit("update:modelValue", item.value)
}

const labelTitleInput = ref<any>(null)
const labelValueInput = ref<any>(null)
defineExpose({
    focusType() {
        labelTitleInput.value?.focus()
    },
    focusValue() {
        labelValueInput.value?.focus()
    },
})
</script>

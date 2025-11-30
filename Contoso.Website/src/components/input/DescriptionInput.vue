<template>
    <div class="col-md mb-2">
        <div class="form-floating">
            <textarea v-model="item" :maxlength="maxLength" :readonly="readonly" :style="style" class="form-control"></textarea>
            <label>{{ label }}</label>
            <!-- <FormLabel :label="label" /> -->
        </div>
    </div>
</template>

<script setup lang="ts">
import { computed } from "vue"
import { useVModelField } from "@/regira_modules/vue/vue-helper"

const emit = defineEmits<{
    (e: "update:modelValue", value: string): void
}>()
const props = withDefaults(
    defineProps<{
        modelValue?: string
        readonly?: boolean
        label?: string
        maxLength?: number
        style?: Record<string, any>
    }>(),
    {
        modelValue: "",
        label: "Description",
        style: () => ({ height: "7.5rem" }),
    }
)

const item = useVModelField<string>(props, emit)
const label = computed(() => {
    let lblValue = props.label
    if (props.maxLength) {
        const charsLeft = props.maxLength - (item.value?.length || 0)
        lblValue = `${lblValue} (${charsLeft} characters left)`
    }
    return lblValue
})
</script>

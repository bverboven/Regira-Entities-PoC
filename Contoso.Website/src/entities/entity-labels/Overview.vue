<template>
    <FormSection :title="$t('labels')" :show-summary="false">
        <Draggable v-model="items" item-key="id" handle=".drag-handle" ghost-class="ghost" style="max-height: 25vh; overflow: auto">
            <template #item="{ element, index }">
                <InlineInput
                    v-model="items[index]"
                    class="mb-2"
                    :class="{ 'is-deleted': element._deleted }"
                    @update:model-value="emit('update:modelValue', items)"
                    @remove="emit('update:modelValue', items)"
                />
            </template>
        </Draggable>

        <div class="row">
            <div class="col-md mb-2">
                <InlineInput v-model="newItem" @update:modelValue="handleAddNew" ref="newLabelEl" />
            </div>
        </div>

        <!-- <template #summary>
            <Summary v-if="items?.length" :items="items" />
            <p v-else class="text-info">{{ $t("noItems") }}</p>
        </template> -->
    </FormSection>
</template>

<script setup lang="ts">
import { ref } from "vue"
import Draggable from "vuedraggable"
import { useVModelField } from "@/regira_modules/vue"
import Entity from "./Entity"
import InlineInput from "./InlineInput.vue"
import Summary from "./Summary.vue"

const emit = defineEmits<{
    (e: "update:modelValue", arg: Array<Entity>): void
}>()
const props = withDefaults(
    defineProps<{
        modelValue?: Array<Entity>
        showSummary?: boolean
    }>(),
    {
        modelValue: () => [],
    }
)

const items = useVModelField<Array<Entity>>(props, emit)

const newLabelEl = ref<any>(null)
const newItem = ref<Entity>(new Entity())
function handleAddNew() {
    emit("update:modelValue", [...items.value, newItem.value])
    newItem.value = new Entity()
    newLabelEl.value?.focusType()
}
</script>

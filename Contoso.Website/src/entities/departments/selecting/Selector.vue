<template>
  <div class="row g-1">
    <div v-for="(item, i) in items" :key="item.id" class="col-auto">
      <div class="text-nowrap p-2 border rounded-1">
        <FormModalButton v-model="items![i]" class="m-0 p-0" />
        {{ item.title }}
        <IconButton icon="delete" class="m-0 py-0 px-1" @click="handleRemove(item)" />
      </div>
    </div>
    <div class="col-auto">
      <InputSelector v-model="newItem" @select="handleSelect" />
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from "vue"
import useEntityStore from "../data/store"
import type Entity from "../data/Entity"
import FormModalButton from "../details/FormModalButton.vue"
import InputSelector from "./InputSelector.vue"

const emit = defineEmits<{
  (e: "update:modelValue", args: Array<Entity>): void
  (e: "update:idsValue", args: Array<string>): void
}>()
const props = defineProps<{
  modelValue?: Array<Entity>
  idsValue?: Array<string>
}>()

const { fromPool, list } = useEntityStore()
const items = computed<Array<Entity>>({
  get: () => fromPool(props.modelValue || []) as Array<Entity>,
  set: (value) => emit("update:modelValue", value as Array<Entity>),
})

const newItem = ref<Entity>()
function handleSelect(selected?: Entity) {
  console.debug("handleSelect", { selected })
  if (selected == null) {
    return
  }

  if (items.value.every((x) => x.$id != selected?.$id)) {
    const newVal = [...items.value, selected]
    emit("update:idsValue", newVal.map((x) => x.id!))
    emit("update:modelValue", newVal)
  }
  newItem.value = undefined
}
function handleRemove(selected: Entity) {
  console.debug("handleRemove", { selected })
  const newVal = items.value.filter((x) => x.$id != selected?.$id)
  emit("update:idsValue", newVal.map((x) => x.id!))
  emit("update:modelValue", newVal)
}

onMounted(async () => {
  console.debug("Selector", { modelValue: props.modelValue, ids: props.idsValue })
  if ((props.idsValue?.length || 0) > 0 && props.modelValue?.length != props.idsValue?.length) {
    const models = await list({ id: props.idsValue })
    emit("update:modelValue", models)
  }
})
</script>

<template>
    <select v-model="selected" class="form-select">
        <option value=""></option>
        <option v-for="item in items" :value="item.id" :key="item.id">
            {{ item.title }}
        </option>
    </select>
</template>

<script setup lang="ts">
    import { computed, onMounted, type Ref } from "vue"
    import type Entity from "../data/Entity"
    import useEntityStore from "../data/store"

    const emit = defineEmits<{
        (e: "update:modelValue", args?: Entity): void
        (e: "update:idValue", args?: number): void
    }>()
    const props = defineProps<{
        modelValue?: Entity
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
    const { fromCache } = useEntityStore()

    const items = computed(() => (fromCache() as Array<Ref<Entity>>)!.map((x) => x.value))
    onMounted(() => {
        if (!selected.value && props.idValue) {
            selected.value = items.value.find((x) => x.id == props.idValue)
        }
    })
</script>

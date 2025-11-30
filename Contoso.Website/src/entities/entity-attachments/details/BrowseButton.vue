<template>
    <button type="button" @click.stop="handleBrowse">
        <slot><Icon name="browse" /></slot>
    </button>
</template>

<script setup lang="ts">
import type Entity from "../data/Entity"
import { browse, createEntity } from "../data/functions"

const emit = defineEmits<{
    (e: "browse", arg: Array<Entity>): void
}>()
const props = withDefaults(
    defineProps<{
        multiple?: boolean
        accept?: string
    }>(),
    {
        multiple: true,
    }
)

async function handleBrowse(): Promise<void> {
    const files = await browse({ multiple: props.multiple, accept: props.accept })
    const items = files.map((file) => createEntity(file))
    emit("browse", items)
}
</script>

<template>
    <Icon :name="icon" />
</template>

<script setup lang="ts">
import { computed } from "vue"
import type Entity from "./Entity"
import { getLabelType } from "./functions"
import LabelTypes from "./LabelTypes"

const props = defineProps<{
    item: Entity
}>()

const icons = new Map<LabelTypes, string>([
    [LabelTypes.Default, "title"],
    [LabelTypes.Date, "date"],
    [LabelTypes.Phone, "phone"],
    [LabelTypes.Email, "mail"],
    [LabelTypes.Url, "website"],
])

const icon = computed(() => icons.get(getLabelType(props.item.value)))
</script>

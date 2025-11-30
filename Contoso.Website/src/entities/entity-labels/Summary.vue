<template>
    <div class="row">
        <div v-for="item in groupedItems" :key="item.title" class="col-auto m-2 border border-info border-opacity-25 rounded">
            <div class="row">
                <div class="col-12 pt-2">
                    <span class="fw-bold">
                        {{ item.title }}
                    </span>
                </div>
                <div v-for="label in item.labels" :key="label.id" class="col-auto p-2"><Icon name="tag" /> {{ label.value }}</div>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { computed } from "vue"
import { groupBy } from "@/regira_modules/utilities/array-utility"
import Entity from "./Entity"

const props = defineProps<{
    items: Array<Entity>
}>()

const groupedItems = computed(() => groupBy(props.items, (x: Entity) => x.title ?? "").map(([title, labels]) => ({ title, labels })))
</script>

<template>
    <div class="row">
        <template v-for="{ name, icon } in icons" :key="name">
            <div class="col-1 mb-2" :title="icon"><Icon :name="name" size="lg" class="p-2" /></div>
            <div class="col-2 mb-2">{{ name }}</div>
        </template>
    </div>
</template>

<script setup lang="ts">
import { computed } from "vue"
import { orderBy } from "@/regira_modules/utilities/array-utility"
import { iconMap } from "@/regira_modules/vue/ui/icons"

const icons = computed(() =>
    orderBy(
        [...iconMap].map(([name, icon]) => ({ name, icon })),
        (x) => x.name.toUpperCase()
    )
)
</script>

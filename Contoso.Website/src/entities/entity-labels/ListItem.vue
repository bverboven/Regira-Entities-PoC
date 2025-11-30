<template>
    <div class="input-group">
        <div class="input-group-text" style="min-width: 9rem">
            <Icon name="tag" />
            <span v-if="item.title" class="ms-2">{{ item.title }}</span>
        </div>
        <div class="form-control text-truncate">
            <template v-if="isAnchor(item.value)">
                <MyAnchor :href="item.value" class="btn btn-link py-0 px-1"><LabelIcon :item="item" /></MyAnchor>
                <span class="ms-2">{{ item.value }}</span>
            </template>
            <template v-else>
                <LabelIcon :item="item" class="mx-1" />
                {{ item.value }}
            </template>
        </div>
    </div>
</template>

<script setup lang="ts">
import { isUrl, isEmail, isPhone, isDate } from "@/regira_modules/utilities/string-utility"
import type Entity from "./Entity"
import LabelIcon from "./LabelIcon.vue"

defineProps<{
    item: Entity
}>()

function isAnchor(value: string) {
    return isUrl(value) || isEmail(value) || (isPhone(value) && !isDate(value))
}
</script>

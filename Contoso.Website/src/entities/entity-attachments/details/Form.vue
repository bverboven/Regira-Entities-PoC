<template>
    <div class="row">
        <div class="col-md mb-2">
            <input v-if="item.id > 0" v-model="item.newFileName" maxlength="256" class="form-control" :placeholder="item.fileName" autocomplete="__away" />
            <input v-else v-model="item.fileName" maxlength="256" class="form-control" autocomplete="__away" />
            <FormLabel label="Filename" />
        </div>

        <div class="col-md mb-2">
            <Debug :modelValue="item" />
        </div>
    </div>
</template>

<script setup lang="ts">
import { useVModelField } from "@/regira_modules/vue/vue-helper"
import type Entity from "../data/Entity"

const emit = defineEmits<{
    (e: "update:modelValue", item: Entity): void
}>()
const props = defineProps<{
    modelValue: Entity
    readonly?: boolean
}>()

const item = useVModelField<Entity>(props, emit)
</script>

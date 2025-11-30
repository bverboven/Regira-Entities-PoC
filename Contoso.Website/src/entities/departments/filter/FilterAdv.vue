<template>
    <div class="adv-filter">
        <div class="row">
            <div class="col mb-2" v-if="resultCount != null">
                <span class="text-info">{{ resultCount }} results</span>
                <small v-if="filterIsActive" class="ms-2 italic-muted">(Filters are applied)</small>
            </div>
            <div class="col mb-2 text-end">
                <IconButton icon="clear" @click="handleReset" :showText="true" />
            </div>
        </div>
        <div class="row">
            <!-- keywords -->
            <div class="col mb-2">
                <div class="input-group">
                    <div class="input-group-text"><Icon name="search" /></div>
                    <input v-model.lazy.trim="searchObject.q" class="form-control" :placeholder="$t('keywords')" />
                </div>
            </div>
        </div>
        <div class="row">
            <!-- title -->
            <div class="col mb-2">
                <div class="input-group">
                    <div class="input-group-text"><Icon name="title" /></div>
                    <input v-model.lazy.trim="searchObject.title" class="form-control" placeholder="title" />
                </div>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { useVModelField } from "@/regira_modules/vue/vue-helper"
import { useFilter, type FilterEmits } from "@/regira_modules/vue/entities"
import SearchObject from "./SearchObject"

interface Emits extends /* @vue-ignore */ FilterEmits {}

const emit = defineEmits<Emits>()
const props = defineProps<{
    modelValue: SearchObject
    resultCount?: number | null
}>()

const searchObject = useVModelField<SearchObject>(props, emit)

const { filterIsActive, handleReset } = useFilter({ searchObject, emit, Constructor: SearchObject })
</script>

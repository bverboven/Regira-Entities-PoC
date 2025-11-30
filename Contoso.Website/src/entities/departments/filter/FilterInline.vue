<template>
    <div class="row">
        <div class="col-auto">
            <div class="input-group">
                <IconButton icon="clear" class="btn-outline-secondary" @click="handleReset" />
                <input v-model.lazy.trim="searchObject.q" class="form-control" :placeholder="$t('keywords')" @change="handleUpdate" />
                <IconButton icon="search" class="btn-outline-primary d-none d-sm-block" @click="handleUpdate" />
                <IconButton v-if="showToggleAdv" icon="filter" :class="filterIsActive ? 'btn-info' : 'btn-outline-info'" @click="handleToggle" />
            </div>
            <small v-if="filterIsActive" class="d-none d-sm-inline italic-muted">{{ $t("filtersAreApplied") }}</small>
        </div>
    </div>
</template>

<script setup lang="ts">
import { useFilter, type FilterEmits } from "@/regira_modules/vue/entities"
import SearchObject from "./SearchObject"
import { useVModelField } from "@/regira_modules/vue/vue-helper"

interface Emits extends /* @vue-ignore */ FilterEmits {}

const emit = defineEmits<Emits>()
const props = withDefaults(
    defineProps<{
        modelValue: SearchObject
        resultCount?: number | null
        showToggleAdv?: boolean
    }>(),
    {
        modelValue: () => new SearchObject(),
        showToggleAdv: false,
    }
)
const searchObject = useVModelField<SearchObject>(props, emit)

const { filterIsActive, handleReset, handleUpdate, handleToggle } = useFilter({ searchObject, emit, Constructor: SearchObject })
</script>

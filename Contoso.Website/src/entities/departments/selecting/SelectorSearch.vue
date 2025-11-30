<template>
  <section class="selector-search">
    <div class="row">
      <div class="col col-md-auto order-1">
        <!-- Filter -->
        <Filter v-model="searchObject" :result-count="itemsCount" @filter="searchHandler(true)"
          @change="searchHandler(true)">
          <template #inline="{ handleToggle }">
            <slot name="inline" :result-count="itemsCount" :search="searchHandler" :search-object="searchObject"
              :paging-info="pagingInfo" :handle-toggle="handleToggle">
              <FilterInline v-model="searchObject" :result-count="itemsCount" @filter="searchHandler(true)"
                @change="searchHandler(true)" @toggle-adv="handleToggle" />
            </slot>
          </template>
          <template #adv="{ handleClose }">
            <slot name="filterAdv" :result-count="itemsCount" :search="searchHandler" :search-object="searchObject"
              :paging-info="pagingInfo" :handle-close="handleClose">
              <component :is="FilterAdv" v-model="searchObject" :result-count="itemsCount" @filter="searchHandler(true)"
                @change="searchHandler(true)" @close="handleClose" />
            </slot>
          </template>
        </Filter>
      </div>
      <div class="col-12 col-md order-3 order-md-2">
        <div class="overflow-hidden">
          <Feedback v-bind="{ feedback }" :hideCloseButton="true" />
          <div v-show="!feedback.status.value" class="row g-0">
            <div class="col-auto">
              <div v-if="selected?.id" class="form-control bg-info py-0">
                <IconButton icon="selected" class="px-1 me-1" @click="handleSelect(null)" />
                <FormModalButton v-model="selected" class="px-1" /> {{ selected.title }}
              </div>
            </div>
          </div>
        </div>
      </div>
      <div class="col-auto order-2 mb-2">
        <FormModalButton :item-defaults="itemDefaults" :close-on-save="true" @save="({ saved }) => handleSelect(saved)"
          class="btn btn-info">
          <Icon name="new" /> <span class="d-none d-sm-inline">{{ $t("new") }}</span>
        </FormModalButton>
      </div>
    </div>

    <!-- Paging - ResultSummary -->
    <div class="row">
      <div class="col order-2">
        <template v-if="pagingInfo != null">
          <Paging class="mt-2" v-show="!isLoading && itemsCount != null && itemsCount > pagingInfo.pageSize!"
            v-model="pagingInfo" :button-type="ButtonType.button" :count="itemsCount || 0"
            @change="searchHandler(false)" />
        </template>
      </div>
      <div class="col-12 col-sm-auto order-1 order-sm-3">
        <ResultSummary v-if="items?.length" :visibleCount="items.length" :totalCount="itemsCount" />
      </div>
    </div>

    <!-- List - Loading -->
    <LoadingContainer :is-loading="isLoading">
      <slot name="list" :items="items" :search-object="searchObject" :paging-info="pagingInfo">
        <component :is="List" v-model="items" :selected="selected" @select="handleSelect" />
      </slot>
    </LoadingContainer>

    <!-- Paging -->
    <template v-if="pagingInfo != null">
      <Paging class="mt-2" v-show="!isLoading && itemsCount != null && itemsCount > pagingInfo.pageSize!"
        v-model="pagingInfo" :button-type="ButtonType.button" :count="itemsCount || 0" @change="searchHandler(false)" />
    </template>

    <Debug :modelValue="items" />
  </section>
</template>

<script setup lang="ts">
import { onMounted, type Ref } from "vue"
import { useVModelField } from "@/regira_modules/vue/vue-helper"
import { useSearchView } from "@/regira_modules/vue/entities"
import { Paging, LoadingContainer, Feedback, ButtonType } from "@/regira_modules/vue/ui"
import ResultSummary from "@/components/ResultSummary.vue"
import config from "../config/config"
import Entity from "../data/Entity"
import useEntityStore from "../data/store"
import SearchObject from "../filter/SearchObject"
import FormModalButton from "../details/FormModalButton.vue"
import FilterInline from "../filter/FilterInline.vue"
import FilterAdv from "../filter/FilterAdv.vue"
import Filter from "../filter/Filter.vue"
import List from "./SelectorList.vue"

const emit = defineEmits<{
  (e: "update:modelValue", selected: Entity | null): void
}>()
const props = defineProps<{
  modelValue?: Entity | null
  filterDefaults?: Record<string, any>
  itemDefaults?: Ref<Record<string, any>> | Record<string, any>
  pageSize?: number
}>()

const selected = useVModelField<Entity | null>(props, emit)

const { service } = useEntityStore()

const {
  searchObject,
  pagingInfo,

  items,
  itemsCount,

  isLoading,
  feedback,

  searchHandler,
} = useSearchView<Entity, SearchObject>({
  service,
  searchObject: Object.assign(new SearchObject(), props.filterDefaults || {}),
  defaultPageSize: props.pageSize || config.defaultPageSize,
})

console.debug("SelectorSearch", { searchObject, pagingInfo, items, itemsCount })

function handleSelect(item: Entity | null) {
  feedback.success(item != null ? `${item.$title} selected` : `selection removed`)
  emit("update:modelValue", item)
  // don't emit 'select' -> wait for submitting (SelectorModalButton)
}

onMounted(searchHandler)
</script>

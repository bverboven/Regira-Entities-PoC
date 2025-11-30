<template>
  <div class="row border-bottom border-bottom-1 py-2">
    <div class="col-auto">
      <router-link :to="{ name: Entity.name + 'Details', params: { id: item.$id } }" class="btn btn-link p-1">
        <Icon :name="Entity.name" />
      </router-link>
    </div>
    <div class="col text-truncate">
      <span class="d-inline">
        {{ item?.$title }}
      </span>
    </div>
    <div class="col text-truncate">
      <DepartmentButton :modelValue="item.department" class="p-1" />
      {{ getDepartment(item.department)?.title }}
    </div>
    <div v-if="!readonly" class="col-auto d-none d-md-block">
      <ConfirmButton icon="delete" class="m-0 p-1" :modal-type="ModalType.danger"
        @confirm="$emit('request-remove', item)">{{
          $t("deleteCourse", { title: item?.$title })
        }}</ConfirmButton>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useVModelField, createFromComputedPool } from "@/regira_modules/vue/vue-helper"
import { ModalType, ConfirmButton } from "@/regira_modules/vue/ui"
import type { SaveResult } from "@/regira_modules/vue/entities"
import { FormModalButton as DepartmentButton, useEntityStore as useDepartmentStore } from "../../departments"
import Entity from "../data/Entity"

const emit = defineEmits<{
  (e: "update:modelValue", args: Entity): void
  (e: "save", args: SaveResult<Entity>): void
  (e: "remove", args: Entity): void
  (e: "request-save", args: Entity): void
  (e: "request-remove", args: Entity): void
}>()
const props = defineProps<{
  modelValue: Entity
  readonly?: boolean
}>()

const item = useVModelField<Entity>(props, emit)

const getDepartment = createFromComputedPool(useDepartmentStore()) as any
</script>

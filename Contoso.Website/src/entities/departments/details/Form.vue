<template>
  <form @submit.prevent="handleSubmit" :modelValue="item">
    <div class="row form-buttons">
      <div class="col col-md-auto order-1">
        <FormButtonsRow :item="item" :readonly="readonly" :feedback="feedback" :show-delete="item?.id != null"
          @cancel="handleCancel" @remove="handleRemove" @restore="handleRestore" />
      </div>
      <div class="col-auto order-2 order-md-3">
        <RouterLink v-if="isPopup" :to="{ name: `${Entity.name}Details`, params: { id: item.$id } }"
          class="btn btn-default py-1" target="_blank" :title="$t('popOut')">
          <Icon name="popOut" />
        </RouterLink>
        <RouterLink v-else-if="overviewUrl" :to="overviewUrl" class="btn btn-info py-1">
          <Icon name="list" /> <span class="d-none d-md-inline ms-1">{{ $t("overview") }}</span>
        </RouterLink>
      </div>
      <div class="col-md order-3 order-md-2">
        <Feedback :feedback="feedback" />
      </div>
    </div>

    <div class="row">
      <div class="col">
        <FormSection :title="$t(config.detailsTitle)" :readonly="readonly">
          <div class="row">
            <div class="col-md mb-2">
              <div class="input-group">
                <div class="input-group-text">
                  <Icon name="title" />
                </div>
                <input v-model="item.title" maxlength="128" :readonly="readonly" class="form-control" />
              </div>
              <FormLabel :label="$t('name')" />
            </div>
            <div class="col-md mb-2">
              <div class="input-group">
                <div class="input-group-text">
                  <Icon name="code" />
                </div>
                <input v-model="item.budget" type="number" min="0" :readonly="readonly" class="form-control" />
              </div>
              <FormLabel :label="$t('code')" />
            </div>
          </div>
        </FormSection>
      </div>
    </div>

    <Debug :modelValue="{
      item,
    }" />
  </form>
</template>

<script setup lang="ts">
import type { RouteRecordRaw } from "vue-router"
import { Feedback } from "@/regira_modules/vue/ui"
import { FormButtonsRow } from "@/components/input"
import { useForm, type FormEmits, formDefaults } from "@/regira_modules/vue/entities"
import config from "../config/config"
import Entity from "../data/Entity"
import useEntityStore from "../data/store"

interface Emits extends /* @vue-ignore */ FormEmits<Entity> { }
const emit = defineEmits<Emits>()
const props = withDefaults(
  defineProps<{
    modelValue: Entity
    readonly?: boolean
    overviewUrl?: string | RouteRecordRaw
    isPopup?: boolean
    initialTab?: string
  }>(),
  { ...formDefaults }
)

const { service: entityService } = useEntityStore()

const { item, feedback, handleCancel, handleSubmit, handleRemove, handleRestore } = useForm<Entity>({ entityService, props, emit })
</script>

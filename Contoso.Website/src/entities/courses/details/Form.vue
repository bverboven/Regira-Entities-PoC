<template>
  <form @submit.prevent="handleSubmit" :modelValue="item">
    <div class="row form-buttons">
      <div class="col col-md-auto order-1">
        <FormButtonsRow :item="item" :readonly="readonly" :feedback="feedback" :show-delete="item?.id > 0"
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
        <TabContainer :tabs="tabs" :active="initialTab" :use-route-nav="!isPopup">
          <template #form>
            <FormSection :title="$t(config.detailsTitle)">
              <div class="row">
                <div class="col-md mb-2">
                  <div class="input-group">
                    <div class="input-group-text">
                      <Icon name="date" />
                    </div>
                    <input v-model="item.title" :disabled="readonly" :culture="$culture" class="form-control" />
                  </div>
                  <FormLabel :label="$t('courseDate')" />
                </div>
                <div class="col-md mb-2">
                  <DepartmentSelector v-model="item.department" v-model:idValue="item.departmentId" :readonly="readonly"
                    :placeholder="$t('selectDepartment')" />
                  <FormLabel :label="$t('department')" />
                </div>
              </div>
            </FormSection>

            <FormSection :title="$t('notes')">
              <div class="row">
                <DescriptionInput v-model="item.description" :readonly="readonly" :label="$t('notes')" />
              </div>
            </FormSection>
          </template>

          <template #labels>
            <Labels v-model="item.labels" :show-summary="item.id > 0" />
          </template>

          <template #files>
            <EntityAttachments v-model="item.attachments" :readonly="readonly" />
          </template>
        </TabContainer>
      </div>
    </div>

    <Debug :modelValue="{
      item: {
        ...item,
        department: item.department && `${item.department?.$title} #${item.department?.id}`,
      }
    }" />
  </form>
</template>

<script setup lang="ts">
import { computed } from "vue"
import type { RouteRecordRaw } from "vue-router"
import { Feedback, TabContainer, Tab } from "@/regira_modules/vue/ui"
import { useForm, type FormEmits, formDefaults } from "@/regira_modules/vue/entities"
import { useLang } from "@/regira_modules/vue/lang"
import { DescriptionInput, FormButtonsRow } from "@/components/input"
import { Overview as Labels } from "../../entity-labels"
import { Overview as EntityAttachments } from "../../entity-attachments"
import { type Entity as Instructor, InputSelector as InstructorSelector, useEntityStore as useInstructorStore } from "../../instructors"
import { type Entity as Department, InputSelector as DepartmentSelector, useEntityStore as useDepartmentStore } from "../../departments"
import config from "../config/config"
import Entity from "../data/Entity"
import useEntityStore from "../data/store"

interface Emits extends /* @vue-ignore */ FormEmits<Entity> { }
const emit = defineEmits<Emits>()
const props = withDefaults(
  defineProps<{
    modelValue: Entity
    initialTab?: string
    readonly?: boolean
    overviewUrl?: string | RouteRecordRaw
    isPopup?: boolean
  }>(),
  { ...formDefaults }
)

const { service: entityService } = useEntityStore()

const { item, feedback, handleCancel, handleSubmit, handleRemove, handleRestore } = useForm<Entity>({ entityService, props, emit })

// Tabs
const { translate } = useLang()
const tabs = computed(() =>
  [
    Tab.create("form", { icon: "form", title: translate("form"), isDefault: true }),
    Tab.create("labels", { icon: "tag", title: translate("labels") }),
    Tab.create("files", { icon: "attachment", title: translate("files") }),
  ].filter((x) => x)
)
</script>

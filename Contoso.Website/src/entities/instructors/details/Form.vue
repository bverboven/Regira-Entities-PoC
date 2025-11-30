<template>
    <form @submit.prevent="handleSubmit" :modelValue="item">
        <div class="row form-buttons">
            <div class="col col-md-auto order-1">
                <FormButtonsRow :item="item" :readonly="readonly" :feedback="feedback" :show-delete="item?.id > 0" @cancel="handleCancel" @remove="handleRemove" @restore="handleRestore" />
            </div>
            <div class="col-auto order-2 order-md-3">
                <RouterLink v-if="isPopup" :to="{ name: `${Entity.name}Details`, params: { id: item.$id } }" class="btn btn-default py-1" target="_blank" :title="$t('popOut')">
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
                        <FormSection :title="$t('instructor')">
                            <div class="row">
                                <div class="col mb-2">
                                    <div class="input-group required-input">
                                        <input class="form-control" v-model.trim="item.givenName" :readonly="readonly" :placeholder="$t('givenName')" required autocomplete="name" maxlength="64" />
                                    </div>
                                    <FormLabel :label="$t('givenName')" />
                                </div>
                                <div class="col mb-2">
                                    <div class="input-group required-input">
                                        <input class="form-control" v-model.trim="item.lastName" :readonly="readonly" :placeholder="$t('lastName')" required autocomplete="name" maxlength="64" />
                                    </div>
                                    <FormLabel :label="$t('lastName')" />
                                </div>
                            </div>
                        </FormSection>
                    </template>

                    <template #files>
                        <EntityAttachments v-model="item.attachments" :readonly="readonly" />
                    </template>
                </TabContainer>
            </div>
        </div>

        <Debug
            :modelValue="{
                ...item,
                attachments: item.attachments?.map(({ id, attachment, newFileName }) => ({ id, fileName: attachment?.fileName, newFileName })),
            }"
        />
    </form>
</template>

<script setup lang="ts">
import { computed } from "vue"
import type { RouteRecordRaw } from "vue-router"
import { Feedback, TabContainer, Tab } from "@/regira_modules/vue/ui"
import { useForm, type FormEmits, formDefaults } from "@/regira_modules/vue/entities"
import { useLang } from "@/regira_modules/vue/lang"
import { FormButtonsRow } from "@/components/input"
import { Overview as EntityAttachments } from "../../entity-attachments"
import Entity from "../data/Entity"
import useEntityStore from "../data/store"

interface Emits extends /* @vue-ignore */ FormEmits<Entity> {}
const emit = defineEmits<Emits>()
const props = withDefaults(
    defineProps<{
        modelValue: Entity
        initialTab?: string
        readonly?: boolean
        isPopup?: boolean
        overviewUrl?: string | RouteRecordRaw
    }>(),
    { ...formDefaults }
)

const { service: entityService } = useEntityStore()

const { item, feedback, handleCancel, handleSubmit, handleRemove, handleRestore } = useForm({ entityService, props, emit })

// Tabs
const { translate } = useLang()
const tabs = computed(() =>
    [
        Tab.create("form", { icon: "form", title: translate("form"), isDefault: true }),
        Tab.create("files", { icon: "attachment", title: translate("files") }),
    ].filter((x) => x)
)
</script>

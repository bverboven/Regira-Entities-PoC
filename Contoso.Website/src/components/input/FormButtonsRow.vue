<template>
    <div class="buttons-row" ref="containerEl">
        <IconButton
            v-if="isArchived"
            type="button"
            icon="restore"
            button-label="Restore"
            class="btn-warning py-1 me-2"
            @click="emit('restore')"
            :disabled="status != '' && status != FeedbackStatus.failed"
        >
            <span v-if="showLabels" class="d-none d-md-inline ms-1">{{ $t("restore") }}</span>
        </IconButton>
        <template v-else>
            <IconButton v-if="!isArchived" type="submit" icon="save" class="btn-primary py-1 me-2" :disabled="readonly || (status != '' && status != FeedbackStatus.failed)">
                <span v-if="showLabels" class="d-none d-md-inline ms-1">{{ $t("save") }}</span>
            </IconButton>
            <IconButton type="button" icon="cancel" class="btn-secondary py-1 mx-2" @click="emit('cancel')" :disabled="readonly || !canCancel">
                <span v-if="showLabels" class="d-none d-md-inline ms-1">{{ $t("reset") }}</span>
            </IconButton>
            <ConfirmButton
                type="button"
                :modal-type="ModalType.danger"
                :modal-title="$t('removeItem?')"
                class="btn-danger py-1 ms-2"
                :disabled="readonly"
                v-show="showDelete"
                @confirm="emit('remove')"
            >
                <template #button-content>
                    <Icon name="delete" class="me-1" />
                    <span v-if="showLabels" class="d-none d-md-inline">{{ $t("delete") }}</span>
                </template>
                <template #default>
                    <slot name="delete">{{ $t("deleteItem", { title: item?.$title }) }}</slot>
                </template>
            </ConfirmButton>
        </template>
    </div>
</template>

<script setup lang="ts">
import { ref, computed } from "vue"
import { FeedbackStatus, type FeedbackOut } from "@/regira_modules/vue/ui/feedback"
import { ConfirmButton, ModalType } from "@/regira_modules/vue/ui"
import type { IEntity } from "@/regira_modules/vue/entities"

const emit = defineEmits<{
    (e: "cancel"): void
    (e: "remove"): void
    (e: "restore"): void
}>()
const props = withDefaults(
    defineProps<{
        item?: IEntity
        readonly?: boolean
        feedback: FeedbackOut
        showDelete?: boolean
        canCancel?: boolean
        hideLabels?: boolean
    }>(),
    {
        showDelete: true,
        canCancel: true,
        hideLabels: false,
    }
)

type archivableItem = { isArchived: number | boolean }

const { status } = props.feedback

const isArchived = computed(() => !!(props.item as unknown as archivableItem).isArchived)

const containerEl = ref<any>(null)
const showLabels = computed(() => {
    if (!containerEl.value) {
        return
    }
    let formEl = containerEl.value
    do {
        formEl = formEl.parentElement
    } while (formEl.nodeName != "FORM" && formEl?.parentElement)
    return !formEl || formEl.clientWidth > 476
})
</script>

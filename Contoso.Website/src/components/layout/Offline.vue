<template>
    <div v-if="!isOnline" class="bg-danger text-white text-center p-2">OFFLINE</div>
</template>

<script setup lang="ts">
import { inject, type Ref } from "vue"

const isOnline = inject<Ref<boolean>>("isOnline")
</script>

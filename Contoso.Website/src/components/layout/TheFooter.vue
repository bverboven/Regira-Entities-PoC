<template>
  <div class="d-flex justify-content-between w-100 py-2">
    <span>&copy; {{ new Date().getFullYear() }} Regira bv</span>
    <small class="text-muted"> version {{ appVersion }} </small>
  </div>
</template>

<script setup lang="ts">
import { useConfig } from '@/app-config'

const { version: appVersion } = useConfig()
</script>

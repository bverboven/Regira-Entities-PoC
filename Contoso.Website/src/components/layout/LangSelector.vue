<template>
    <ul class="list-inline d-inline">
        <li class="list-inline-item cursor pointer" :class="{ 'fw-bold': langCode == 'en' }" @click="setLangCode('en')">EN</li>
        <li class="list-inline-item cursor pointer" :class="{ 'fw-bold': langCode == 'fr' }" @click="setLangCode('fr')">FR</li>
        <li class="list-inline-item cursor pointer" :class="{ 'fw-bold': langCode == 'nl' }" @click="setLangCode('nl')">NL</li>
    </ul>
</template>

<script setup lang="ts">
import { useLang } from "@/regira_modules/vue/lang"

const { langCode, setLangCode } = useLang()
</script>

<template>
  <nav class="navbar navbar-expand-sm bg-body-tertiary" v-click-outside="handleCloseMenu">
    <router-link class="navbar-brand" :to="{ name: 'home' }" :title="`${appTitle} v${version}`">
      <img :src="logo" style="height: 2rem; vertical-align: top" class="me-1" />
      <span class="d-sm-none d-md-inline"> {{ appTitle }} </span>
    </router-link>
    <button class="navbar-toggler" type="button" @click.stop="showNavbarContent = !showNavbarContent">
      <span class="navbar-toggler-icon"></span>
    </button>
    <div class="collapse navbar-collapse bg-light px-2 px-md-0 pt-2 pt-sm-0" :class="{ show: showNavbarContent }">
      <ul class="navbar-nav me-auto mb-2 mb-lg-0">
        <li class="nav-item py-2 py-sm-0 d-none">
          <router-link class="nav-link" aria-current="page" :to="{ name: 'home' }" :title="`${appTitle} v${version}`">{{
            appTitle }}</router-link>
        </li>
        <li v-for="navItem in navbarItems" :key="navItem.id" class="nav-item py-2 py-sm-0">
          <RouterLink :to="{ name: navItem.id + 'Overview', query: navItem.initialQuery || {} }" class="nav-link"
            @click="handleCloseMenu">
            <icon :name="navItem.id" />
            <span class="d-sm-none d-lg-inline ms-2">{{ $t(navItem.overviewTitle) }}</span>
          </RouterLink>
        </li>
      </ul>
    </div>
  </nav>
</template>

<script setup lang="ts">
import { ref, watchEffect, getCurrentInstance } from "vue"
import { useConfig } from "@/app-config"
import logo from "@/assets/images/logo-sm.png"
import type { IConfig } from "@/regira_modules/vue/entities"

const { version } = useConfig()

const showNavbarContent = ref(false)
const showNavbarManagementDropdown = ref(false)

document.addEventListener("click", () => {
  showNavbarContent.value = false
  showNavbarManagementDropdown.value = false
})

const app = getCurrentInstance()!
const configs = Object.entries(app.appContext.config.globalProperties.$configs).map(([_, config]: any) => config as IConfig)

const { navbar: navbarKeys, title: appTitle } = useConfig()
const navbarItems = ref<Array<IConfig>>([])

function handleCloseMenu() {
  showNavbarContent.value = false
}

watchEffect(() => {
  navbarItems.value = []
  navbarKeys.forEach((key: string) => {
    const config = configs.find((d) => d.routePrefix == key)
    if (config == null) {
      console.warn(`Could not find navbar item ${key}`)
    } else {
      navbarItems.value.push(config)
    }
  })
})
</script>

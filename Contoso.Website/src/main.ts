import { createApp } from "vue"
import { createPinia } from "pinia"
import type { RouteRecordRaw } from "vue-router"
import { Anchor, FormSection, DateInput, FormLabel, NullableCheckBox, iconPlugin, loadingPlugin, screenPlugin, modalPlugin, feedbackPlugin, NullableLabel } from "@/regira_modules/vue/ui"
import { focus, grow, clickOutside } from "@/regira_modules/vue/directives"
import { AppStatus, plugin as appPlugin } from "@/regira_modules/vue/app"
import { plugin as langPlugin, useLang } from "@/regira_modules/vue/lang"
import { plugin as isOnlinePlugin } from "@/regira_modules/vue/online"
import { plugin as debugPlugin } from "@/regira_modules/vue/debug"
import { preloaderPlugin, usePreloader, defaultPoolCache, PoolCache } from "@/regira_modules/vue/entities"
import { initAxios } from "@/regira_modules/vue/http"
import { plugin as authPlugin, LocalStorageTokenManager } from "@/regira_modules/vue/auth"
import { plugin as servicesPlugin, type IServiceProvider } from "@/regira_modules/vue/ioc"
import { formatDateTime } from "@/regira_modules/vue/formatters"
import appConfig, { createConfig, useConfig } from "@/app-config"
import { routerFactory } from "@/router"
import { plugin as userPlugin } from "@/infrastructure/user-plugin"
import { default as entityPlugins } from "@/entities"

import { Entity as Department } from "@/entities/departments"

import App from "@/App.vue"
import AppModal from "@/components/layout/AppModal.vue"
import DescriptionInput from "@/components/input/DescriptionInput.vue"

// Assets
import "@/assets/base.scss"
import "@/assets/main.scss"
import loadingImg from "@/assets/images/loading.gif"


// load config
fetch(`${appConfig.baseUrl}/config.json?v=${formatDateTime(new Date(), "yyyyMMdd")}`)
  .then((r) => r.json())
  .then(async (config: Record<string, any>) => {
    // start init app
    console.debug("init", { config })
    // config (processed)
    const processedConfig = createConfig(config)
    const { api } = processedConfig
    const axios = initAxios({ api })

    // load translations
    const translations = await fetch(`${appConfig.baseUrl}/data/translations.json?v=${formatDateTime(new Date(), "yyyyMMdd")}`).then((r) => r.json())

    // app
    const app = createApp(App)
    // store
    app.use(createPinia())

    app.use(appPlugin, { culture: processedConfig.culture })

    // global components (use explicit naming -> functions are renamed when minimized in build)
    app.component("MyAnchor", Anchor)
    app.component("FormSection", FormSection)
    app.component("DateInput", DateInput)
    app.component("NullableCheckBox", NullableCheckBox)
    app.component("NullableLabel", NullableLabel)
    app.component("FormLabel", FormLabel)
    app.component("DescriptionInput", DescriptionInput)

    // services
    app.use(servicesPlugin, {
      configure: (sp: IServiceProvider) => {
        return (
          sp
            // axios
            .add("axios", () => axios)
            // pooling
            .add(PoolCache.name, () => defaultPoolCache)
        )
      },
    })

    app.use(iconPlugin, { source: "bs", clearFirst: false })
    app.use(screenPlugin)
    app.use(isOnlinePlugin)
    app.use(debugPlugin, { isDebug: config.isDebug })
    app.use(loadingPlugin, { img: loadingImg })
    app.use(modalPlugin, { DefaultModal: AppModal })
    app.use(feedbackPlugin, { autoHideDelay: 2500 })

    // lang
    app.use(langPlugin, {
      defaultLang: "en",
      messages: translations, //() => fetch(`${appConfig.baseUrl}/data/translations.json`).then((r) => r.json()),
    })
    const { translate: t, translateMessage, setLangCode } = useLang()
    document.title = translateMessage(processedConfig.title)
    setLangCode(processedConfig.culture)

    // global directives
    app.use(focus)
    app.use(grow)
    app.use(clickOutside)

    // entities + entity routes
    const entityRoutes: Array<RouteRecordRaw> = []
    app.use(entityPlugins, { routes: entityRoutes })

    // routing
    const router = routerFactory([...entityRoutes])
    app.use(router)

    // preloader
    app.use(preloaderPlugin)

    // auth
    const authEnabled = false
    app.use(authPlugin, {
      enabled: authEnabled,
      clientApp: processedConfig.clientApp,
      //loginUrl: processedConfig.loginUrl.replace(/{clientApp}/, processedConfig.clientApp).replace(/{tenantId}/, localStorage.getItem("tenant") || ""),
      //tokenManager: new LocalStorageTokenManager(),
      axios,
      onAuthenticationChange: async (auth) => {
        if (!authEnabled) {
          app.config.globalProperties.$setAppStatus(AppStatus.Ready)

          // preloading
          const preloaderTypes = [Department]
          const { preload } = usePreloader()
          await preload(preloaderTypes as any)
        }
        else if (auth.isAuthenticated) {
          await handleAuthenticate(auth)
          console.debug("ready", {
            app,
            appConfig: useConfig(),
            router,
            routes: router.getRoutes(),
            cache: defaultPoolCache,
            icons: app.config.globalProperties.$icons.map,
          })
        } else {
          app.config.globalProperties.$feedback.fail(`User logged out`)
        }
      },
    })
    async function handleAuthenticate(auth: any) {
      app.config.globalProperties.$setAppStatus(AppStatus.Loading)
      const welcomeMsg = t("welcome", { name: auth.displayName || auth.name })
      app.config.globalProperties.$feedback.success(welcomeMsg)

      // preloading
      const preloaderTypes = [Country]
      const { preload } = usePreloader()
      await preload(preloaderTypes as any)

      // ready
      app.config.globalProperties.$setCulture(auth.culture)
      app.config.globalProperties.$setAppStatus(AppStatus.Ready)

      setLangCode(auth.culture!.split("-")[0])
      document.title = translateMessage(processedConfig.title)
    }
    app.use(userPlugin)

    app.mount("#app")
  })

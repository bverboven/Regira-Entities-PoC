import { type App, watch } from "vue"
import { useAuth, useAuthStore } from "@/regira_modules/vue/auth"
import { useLang } from "@/regira_modules/vue/lang"
import Permissions from "@/infrastructure/permissions"
import { useConfig } from "@/app-config"

export const plugin = {
    install(app: App) {
        const authStore = useAuthStore()
        Object.defineProperties(app.config.globalProperties, {
            $isReadonlyUser: {
                get: () => authStore.authData.hasPermission(Permissions.CAN_READ) && !authStore.authData.hasPermission(Permissions.CAN_WRITE),
                enumerable: true,
                configurable: true,
            },
            $isAdmin: {
                get: () => authStore.authData.hasPermission(Permissions.ADMIN),
                enumerable: true,
                configurable: true,
            },
        })

        // make langCode persistent
        const { langCode, setLangCode } = useLang()
        const lastLangCode = localStorage.getItem("lang")
        if (!authStore.isAuthenticated && lastLangCode && lastLangCode != langCode.value) {
            setLangCode(lastLangCode.substring(0, 2))
        }
        watch(langCode, (newLangCode) => {
            localStorage.setItem("lang", newLangCode)
        })

        const auth = useAuth()
        const appConfig = useConfig()
        // make tenant persistent when logged off
        authStore.$onAction(
            ({ name, after }) =>
                ["login", "refresh"].includes(name) &&
                after(() => {
                    if (authStore.isAuthenticated) {
                        // tenant
                        const tenantId = authStore.getClaimValue("tenant") as string
                        if (tenantId) {
                            localStorage.setItem("tenant", tenantId)
                            // update loginUrl to pass new tenantId
                            auth.service.options.loginUrl = appConfig.loginUrl.replace("{clientApp}", appConfig.clientApp).replace("{tenantId}", tenantId)
                        } else {
                            localStorage.removeItem("tenant")
                        }
                    }
                })
        )
    },
}

export default plugin

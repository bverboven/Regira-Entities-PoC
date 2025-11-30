import { trimRight } from "@/regira_modules/utilities/string-utility";

const { BASE_URL, MODE } = import.meta.env;

type AppConfig = Record<string, any>;// { [key: string]: any };

const appConfig: AppConfig = {
    version: __APP_VERSION__,
    baseUrl: trimRight(BASE_URL, "/"),
    includeCredentials: true,// for Windows authentication
    env: MODE
};

function getEnvValue(config: any, key: string): any {
    return typeof config[key][appConfig.env] !== "undefined" ? config[key][appConfig.env] : config[key];
}

// add extra config to appConfig
export function createConfig(config: any): AppConfig {
    Object.keys(config).forEach((key: string) => {
        appConfig[key] = getEnvValue(config, key);
    });
    return appConfig;
};
// processed config
export function useConfig(): AppConfig { return appConfig; };

export default appConfig;

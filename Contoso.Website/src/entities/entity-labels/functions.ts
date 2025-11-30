import { isUrl, isEmail, isPhone, isDate } from "@/regira_modules/utilities/string-utility"
import LabelTypes from "./LabelTypes"

export function getLabelType(value: string) {
    if (isEmail(value)) {
        return LabelTypes.Email
    }
    if (isDate(value)) {
        return LabelTypes.Date
    } else if (isPhone(value)) {
        return LabelTypes.Phone
    }
    if (isUrl(value)) {
        return LabelTypes.Url
    }

    return LabelTypes.Default
}

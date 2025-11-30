import { EntityBase } from "@/regira_modules/vue/entities"

export class OfficeAssignment extends EntityBase {
    id: number = 0
    startDate?: Date
    endDate?: Date
    location?: string // max 64

    override get $id(): string | number {
        return this.id || "new"
    }
    override get $title(): string | undefined {
        return this.location
    }
}

export const Entity = OfficeAssignment

export default OfficeAssignment

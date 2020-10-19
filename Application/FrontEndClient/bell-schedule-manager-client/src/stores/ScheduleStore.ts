import { computed, observable, makeObservable, action } from "mobx";
import { API_URL } from "../Config";

type ScheduleModel = {
    scheduleId: string,
    scheduleName: string,
    googleCalendarId: string,
    scheduleRules: ScheduleRule[]
}

type ScheduleRule = {
    name: string,
    readableDescription: string
}

class ScheduleStore {
    schedules: ScheduleModel[] = [];
    needsInitialization: boolean = true;

    constructor() {
        makeObservable(this, {
            schedules: observable,
            getSchedules: action,
            createCalendarForSchedule: action,
            clear: action
        });
    }

    async getSchedules() {
        this.needsInitialization = false;
        const response = await fetch(`${API_URL}/api/schedule`, {
            credentials: 'include'
        });
        const json = await response.json();
        this.schedules = json;
    }

    async deleteSchedule(scheduleId: string) {
        const response = await fetch(`${API_URL}/api/schedule/${scheduleId}`, {
            credentials: 'include',
            method: 'DELETE'
        });
        if (response.ok) {
            await this.getSchedules();
        }
    }

    async createCalendarForSchedule(scheduleId: string) {
        const response = await fetch(`${API_URL}/api/schedule/${scheduleId}/google-calendar`, {
            method: 'POST',
            credentials: 'include'
        });
        if (response.ok) {
            await this.getSchedules();
        }
    }

    clear() {
        this.schedules = [];
        this.needsInitialization = true;
    }
}

export default ScheduleStore;
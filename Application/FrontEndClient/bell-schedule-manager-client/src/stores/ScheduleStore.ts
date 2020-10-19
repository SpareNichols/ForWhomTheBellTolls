import { computed, observable, makeObservable, action } from "mobx";
import { API_URL } from "../Config";
import moment from "moment";

export type ScheduleModel = {
    scheduleId: string,
    scheduleName: string,
    googleCalendarId: string,
    scheduleRules: ScheduleRule[],
    todaysEvents: ScheduleEvent[]
}

export type ScheduleRule = {
    name: string,
    readableDescription: string
}

export enum EventType {
    start = "start",
    end = "end"
}

export class ScheduleEvent {
    eventTime!: moment.Moment;
    eventType!: EventType;
    name!: string;
}

class ScheduleStore {
    schedules: ScheduleModel[] = [];
    needsInitialization: boolean = true;

    constructor() {
        makeObservable(this, {
            schedules: observable,
            needsInitialization: observable,
            getSchedules: action,
            isSchedulesLoading: observable,
            createCalendarForSchedule: action,
            isCreatingCalendar: observable,
            clear: action
        });
    }

    async addSchedule(scheduleName: string, file: File) {
        const formData = new FormData();
        formData.append("scheduleName", scheduleName);
        formData.append("file", file)
        const response = await fetch(`${API_URL}/api/schedule/from-file`, {
            credentials: 'include',
            method: 'POST',
            body: formData
        });
        if (response.ok) {
            await this.getSchedules();
        }
    }

    isSchedulesLoading: boolean = false;
    async getSchedules() {
        this.needsInitialization = false;
        this.isSchedulesLoading = true;
        const response = await fetch(`${API_URL}/api/schedule`, {
            credentials: 'include'
        });
        const json = await response.json();
        json.forEach((s: ScheduleModel) => {
            s.todaysEvents.forEach(te => te.eventTime = moment(te.eventTime))
        });
        this.schedules = json;
        this.isSchedulesLoading = false;
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

    isCreatingCalendar: boolean = false;
    async createCalendarForSchedule(scheduleId: string) {
        this.isCreatingCalendar = true;
        const response = await fetch(`${API_URL}/api/schedule/${scheduleId}/google-calendar`, {
            method: 'POST',
            credentials: 'include'
        });
        if (response.ok) {
            await this.getSchedules();
        }
        this.isCreatingCalendar = false;
    }

    clear() {
        this.schedules = [];
        this.needsInitialization = true;
    }
}

export default ScheduleStore;
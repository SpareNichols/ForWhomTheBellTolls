import { computed, observable, makeObservable, action } from "mobx";
import { API_URL } from "../Config";

type ScheduleModel = {
    scheduleId: string,
    scheduleName: string
}

class ScheduleStore {
    schedules: ScheduleModel[] = [];
    needsInitialization: boolean = true;

    constructor() {
        makeObservable(this, {
            schedules: observable,
            getSchedules: action
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
}

export default ScheduleStore;
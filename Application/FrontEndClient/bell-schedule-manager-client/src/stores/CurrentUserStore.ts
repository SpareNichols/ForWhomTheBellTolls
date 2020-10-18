import { computed, observable, makeObservable, action } from "mobx";
import { API_URL } from "../Config";

class UserModel {
    displayName!: string;
    email!: string;
}

class CurrentUserStore {
    user: UserModel | undefined = undefined;
    userAuthAttempted: boolean = false;

    constructor() {
        makeObservable(this, {
            user: observable,
            userAuthAttempted: observable,
            isLoggedIn: computed,
            shouldAttemptAuth: computed,
            getMe: action,
            signMeOut: action
        })
    }

    get isLoggedIn(): boolean {
        return this.user !== undefined;
    }

    get shouldAttemptAuth(): boolean {
        return this.user === undefined && !this.userAuthAttempted;
    }

    async getMe() {
        this.userAuthAttempted = true;

        const response = await fetch(`${API_URL}/api/auth/me`, {
            credentials: 'include'
        });
        if (response.status === 401) {
            this.user = undefined;
            this.userAuthAttempted = true;
            return;
        }
        const json = await response.json();
        this.user = json;
    }

    async signMeOut() {
        const response = await fetch(`${API_URL}/api/auth/logout`, {
            credentials: 'include'
        });
        this.user = undefined;
    }
}

export default CurrentUserStore;
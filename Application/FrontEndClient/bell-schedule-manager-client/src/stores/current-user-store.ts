import { computed, observable, makeObservable, action } from "mobx";

class UserModel {
    name!: string;
}

class CurrentUserStore {
    user: UserModel | undefined = undefined;

    constructor() { 
        makeObservable(this, {
            user: observable,
            isLoggedIn: computed,
            getMe: action
        });
    }

    get isLoggedIn(): boolean {
        return this.user !== undefined;
    }

    getMe() {
        this.user = {
            name: "Michael Nichols"
        }
    }
}

export default CurrentUserStore;
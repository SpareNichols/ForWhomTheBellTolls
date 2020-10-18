import React from 'react';
import CurrentUserStore from '../stores/CurrentUserStore';
import ScheduleStore from '../stores/ScheduleStore';

export const storesContext = React.createContext({
    currentUserStore: new CurrentUserStore(),
    scheduleStore: new ScheduleStore()
});

export const useStores = () => React.useContext(storesContext);

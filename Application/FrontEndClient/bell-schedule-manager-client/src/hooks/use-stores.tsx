import React from 'react';
import CurrentUserStore from '../stores/current-user-store';

export const storesContext = React.createContext({
    currentUserStore: new CurrentUserStore()
});

export const useStores = () => React.useContext(storesContext);

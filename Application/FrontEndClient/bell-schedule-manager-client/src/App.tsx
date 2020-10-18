import React, { ReactElement } from 'react'
import { useStores } from './hooks/use-stores';
import { observer } from 'mobx-react';
import Navigation from './components/Navigation';

const App = observer(() => {
    
    const { currentUserStore } = useStores();

    return (
        <Navigation />
    )
});

export default App;
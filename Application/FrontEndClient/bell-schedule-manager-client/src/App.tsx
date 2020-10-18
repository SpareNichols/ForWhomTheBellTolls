import React, { ReactElement, useEffect } from 'react'
import { useStores } from './hooks/use-stores';
import { observer } from 'mobx-react';
import Navigation from './components/Navigation';
import UploadSchedule from './components/UploadSchedule';
import { Container, Row, Col, Card } from 'react-bootstrap';
import ScheduleList from './components/ScheduleList';

const App = observer(() => {    
    const { currentUserStore } = useStores();

    if (currentUserStore.shouldAttemptAuth) {
        currentUserStore.getMe();
    }

    return (
        <>
        <Navigation />
        <Container>
            <Row>
                <Col>
                    <ScheduleList></ScheduleList>
                </Col>
            </Row>
            <Row>
                <Col>
                    <Card body>
                        <UploadSchedule />
                    </Card>
                </Col>
            </Row>    
        </Container>
        </>
    )
});

export default App;
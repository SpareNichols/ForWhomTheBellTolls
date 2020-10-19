import React, { ReactElement, useEffect } from 'react'
import { useStores } from './hooks/use-stores';
import { observer } from 'mobx-react';
import Navigation from './components/Navigation';
import UploadSchedule from './components/UploadSchedule';
import { Container, Row, Col, Card } from 'react-bootstrap';
import ScheduleList from './components/ScheduleList';
import RingingBell from './components/RingingBell';
import { API_URL } from './Config';
import ScheduleTracker from './components/ScheduleTracker';

const App = observer(() => {    
    const { currentUserStore } = useStores();

    if (window.location.search) {
        var urlParams = new URLSearchParams(window.location.search);
        if (urlParams.has('code')) {
            var code = urlParams.get('code');
            var url = new URL(`${API_URL}/api/auth/google-login-callback`);
            url.search = urlParams.toString();
            fetch (url.toString(), {
                credentials: 'include'
            }).then(() => {
                window.location.search = "";
                currentUserStore.getMe();
            })
        }
    }

    if (currentUserStore.shouldAttemptAuth) {
        currentUserStore.getMe();
    }

    return (
        <>
            <Navigation />
            <Container>
            {currentUserStore.isLoggedIn
                ?
                <Row>
                    <Col>
                        <ScheduleList></ScheduleList>
                    </Col>
                </Row>
                :
                <Row style={{marginTop: "20px"}}>
                    <Col>
                        <Card>
                            <Card.Body>
                            <Row className="justify-content-center">
                                Please log in to use the bell scheduling utility!
                            </Row>
                            <Row className="justify-content-center">
                                <RingingBell size="300px" color="lightgrey" />
                            </Row>
                            </Card.Body>
                        </Card>
                    </Col>
                </Row>
            }
            </Container>
        </>
    )
});

export default App;
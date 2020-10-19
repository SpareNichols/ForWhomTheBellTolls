import React, { useState } from 'react'
import { useStores } from '../hooks/use-stores'
import { Card, Row, Col, Button, Table, Spinner, Collapse } from 'react-bootstrap';
import { observer } from 'mobx-react';
import RingingBell from './RingingBell';
import UploadSchedule from './UploadSchedule';
import ScheduleTracker from './ScheduleTracker';
import { faCalendarMinus } from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

interface Props {
    
}

const ScheduleList = observer((props: Props) => {
    const { currentUserStore, scheduleStore } = useStores();
    const [scheduleOverviewOpen, setScheduleOverviewOpen] = useState(false);

    if (scheduleStore.needsInitialization && currentUserStore.isLoggedIn) {
        scheduleStore.getSchedules();
    }

    return (
        <>
            {
                scheduleStore.isSchedulesLoading &&
                <Card body>
                    <div>
                        <RingingBell size="160px" color="lightgrey" />
                    </div>
                </Card>
            }
            
            <Card body style={{marginTop: "20px", marginBottom: "20px"}}>
                <span style={{fontSize: "2rem"}}>Schedules</span>
                <div style={{float:"right"}}>
                <UploadSchedule/></div>
            </Card>
            {
                scheduleStore.schedules.map((schedule) => 
                <Row key={schedule.scheduleId}><Col>
                <Card style={{marginBottom: "20px"}}>
                    <Card.Header>
                        <Card.Title>{schedule.scheduleName}</Card.Title>
                    </Card.Header>
                    <Card.Body>
                        <Row style={{marginBottom:"10px"}}>
                            <Col>
                            <ScheduleTracker allEvents={schedule.todaysEvents} scheduleName={schedule.scheduleName}/>
                            </Col>
                        </Row>
                        
                        <Row style={{marginBottom:"10px"}}>
                        <Col>
                            <Button onClick={() => setScheduleOverviewOpen(!scheduleOverviewOpen)}>Schedule Overview</Button>
                            <Collapse in={scheduleOverviewOpen}>
                            <Table>
                                <thead>
                                    <tr>
                                        <th>Name</th>
                                        <th>Schedule Description</th>
                                    </tr>
                                </thead>
                                {schedule.scheduleRules.map(r => 
                                <tr>
                                    <td>{r.name}</td>
                                    <td>{r.readableDescription}</td>
                                </tr>)}
                            </Table>
                        </Collapse>
                        </Col>
                        </Row>
                        
                        
                        {(scheduleStore.isCreatingCalendar ? 
                                <Button disabled>
                                    <RingingBell size="25px" color="white" />
                                </Button>
                                : 
                                <Button onClick={() => scheduleStore.createCalendarForSchedule(schedule.scheduleId)}>
                                    {schedule.googleCalendarId ? "Reset Google Calendar" : "Create Google Calendar"}
                                </Button>
                        )}
                        <Button style={{marginLeft: "20px"}} onClick={() => scheduleStore.deleteSchedule(schedule.scheduleId)} variant="danger"><FontAwesomeIcon icon={faCalendarMinus} /></Button>
                    </Card.Body>
                </Card>
                </Col>
                </Row>)
            }
        </>
    )
})

export default ScheduleList

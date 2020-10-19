import React from 'react'
import { useStores } from '../hooks/use-stores'
import { Card, Row, Col, Button, Table, Spinner } from 'react-bootstrap';
import { observer } from 'mobx-react';
import RingingBell from './RingingBell';
import UploadSchedule from './UploadSchedule';
import ScheduleTracker from './ScheduleTracker';

interface Props {
    
}

const ScheduleList = observer((props: Props) => {
    const { currentUserStore, scheduleStore } = useStores();

    if (scheduleStore.needsInitialization && currentUserStore.isLoggedIn) {
        scheduleStore.getSchedules();
    }

    
    
    return (
        <>
            <Row><Col></Col></Row>
            {
                scheduleStore.isSchedulesLoading &&
                <Card body>
                    <div>
                        <RingingBell size="160px" color="lightgrey" />
                    </div>
                </Card>
            }
            
            <Card body style={{marginTop: "20px"}}>
                <span style={{fontSize: "2rem"}}>Schedules</span>
                <div style={{float:"right"}}>
                <UploadSchedule/></div>
            </Card>
            {
                scheduleStore.schedules.map((schedule) => 
                <Row key={schedule.scheduleId}><Col>
                <Card style={{marginTop: "20px"}}>
                    <Card.Header>
                        <Card.Title>{schedule.scheduleName}</Card.Title>
                    </Card.Header>
                    <Card.Body>
                        <ScheduleTracker allEvents={schedule.todaysEvents} />
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
                        {(scheduleStore.isCreatingCalendar ? 
                                <Button disabled>
                                    <RingingBell size="25px" color="white" />
                                </Button>
                                : 
                                <Button onClick={() => scheduleStore.createCalendarForSchedule(schedule.scheduleId)}>
                                    {schedule.googleCalendarId ? "Reset Google Calendar" : "Create Google Calendar"}
                                </Button>
                        )}
                        <Button onClick={() => scheduleStore.deleteSchedule(schedule.scheduleId)} variant="danger">Delete Schedule</Button>
                    </Card.Body>
                </Card>
                </Col>
                </Row>)
            }
        </>
    )
})

export default ScheduleList

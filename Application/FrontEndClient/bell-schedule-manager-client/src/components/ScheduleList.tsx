import React from 'react'
import { useStores } from '../hooks/use-stores'
import { Card, Row, Col, Button, Table } from 'react-bootstrap';
import { observer } from 'mobx-react';

interface Props {
    
}

const ScheduleList = observer((props: Props) => {
    const { currentUserStore, scheduleStore } = useStores();
    if (scheduleStore.needsInitialization && currentUserStore.isLoggedIn) {
        scheduleStore.getSchedules();
    }
    
    return (
        <>
            <Row><Col><h2>Schedules</h2></Col></Row>
            {
                scheduleStore.schedules.map((schedule) => 
                <Row key={schedule.scheduleId}><Col>
                <Card style={{marginBottom: "20px"}}>
                    <Card.Header>
                        <Card.Title>{schedule.scheduleName}</Card.Title>
                    </Card.Header>
                    <Card.Body>
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
                        <Button onClick={() => scheduleStore.createCalendarForSchedule(schedule.scheduleId)}>Create Google Calendar</Button>
                        <Button onClick={() => scheduleStore.deleteSchedule(schedule.scheduleId)} variant="danger">Delete Schedule</Button>
                    </Card.Body>
                </Card>
                </Col></Row>)
            }
        </>
    )
})

export default ScheduleList

import React from 'react'
import { useStores } from '../hooks/use-stores'
import { Card, Row, Col } from 'react-bootstrap';
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
                <Row key={schedule.scheduleId}><Col><Card body>
                    {schedule.scheduleId} - {schedule.scheduleName}
                </Card></Col></Row>)
            }
        </>
    )
})

export default ScheduleList

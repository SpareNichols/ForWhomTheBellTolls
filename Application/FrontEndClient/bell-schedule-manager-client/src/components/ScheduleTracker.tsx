import React, { useState, useEffect } from 'react'
import { Row, Col, Card } from 'react-bootstrap'
import moment from 'moment'
import { ScheduleEvent, EventType } from '../stores/ScheduleStore';

interface Props {
    allEvents: ScheduleEvent[]
}

const ScheduleTracker = (props: Props) => {
    const [currentTime, setCurrentTime] = useState(moment());

    useEffect(() => {
        const intervalId = setInterval(() => {
            setCurrentTime(moment());
        }, 1000);

        return () => clearInterval(intervalId);
    }, [currentTime])

    props.allEvents.sort((a,b) => a.eventTime.valueOf() - b.eventTime.valueOf());
    const futureEvents = props.allEvents.filter(a => a.eventTime.isAfter(currentTime));
    const nextEvent = futureEvents[0];
    console.log('current: ' + currentTime.toString())
    console.log('next:' + nextEvent.eventTime.toString())
    return (
        <Row>
            <Col>
                <Card body>
                    {nextEvent && (nextEvent.eventType === EventType.end ? <p>{`Currently in ${nextEvent.name}`}</p> : <p>Currently in between bells -- take it easy!</p>)}
                    {nextEvent 
                    ?
                    <p>Up Next: {nextEvent.name} will {nextEvent.eventType} in {nextEvent.eventTime.diff(currentTime,"s")}</p>
                    :
                    <>All done for the day!</>
                    }
                    
                </Card>
            </Col>
        </Row>
    )
}

export default ScheduleTracker

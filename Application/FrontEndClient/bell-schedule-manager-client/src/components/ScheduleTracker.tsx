import React, { useState, useEffect } from 'react'
import { Row, Col, Card, Toast } from 'react-bootstrap'
import moment from 'moment'
import { ScheduleEvent, EventType } from '../stores/ScheduleStore';
import RingingBell from './RingingBell';

interface Props {
    scheduleName: string,
    allEvents: ScheduleEvent[]
}

const ScheduleTracker = (props: Props) => {
    const [currentTime, setCurrentTime] = useState(moment());
    const [lastEventMessage, setLastEventMessage] = useState("")
    const [showToast, setShowToast] = useState(false);
    
    useEffect(() => {
        const intervalId = setInterval(() => {
            setCurrentTime(moment());
        }, 1000);

        return () => clearInterval(intervalId);
    }, [currentTime]);

    let bellAudio = new Audio('/bell-sound.mp3');

    const convertSecondsToString = (seconds: number) => {
        let hours = Math.floor(seconds / 3600);
        let minutes = Math.floor((seconds % 3600) / 60);
        let sec = seconds % 60;

        return `${hours}h ${minutes}m ${sec}s`;
    }

    props.allEvents.sort((a,b) => a.eventTime.valueOf() - b.eventTime.valueOf());
    const futureEvents = props.allEvents.filter(a => a.eventTime.isAfter(currentTime));
    const nextEvent = futureEvents[0];

    if (nextEvent.eventTime.diff(currentTime, "ms") < 1000 && !showToast) {
        setLastEventMessage(nextEvent.name + " has " + nextEvent.eventType + "ed at " + nextEvent.eventTime.toString())
        setShowToast(true);
        bellAudio.play();
    }

    return (
        <Row>
            <Col>
                <Card body>
                    <Toast show={showToast} onClose={() => setShowToast(false)}>
                        <Toast.Header>
                            {props.scheduleName} Bell Sounding
                        </Toast.Header>
                        <Toast.Body>
                            <RingingBell size="50px" color="yellow"/>
                            {lastEventMessage}
                        </Toast.Body>
                    </Toast>
                    {nextEvent && (nextEvent.eventType === EventType.end ? <p>{`Currently in ${nextEvent.name}`}</p> : <p>Currently in between bells -- take it easy!</p>)}
                    {nextEvent 
                    ?
                    <p>Up Next: {nextEvent.name} will {nextEvent.eventType} in {convertSecondsToString(nextEvent.eventTime.diff(currentTime,"s"))}</p>
                    :
                    <>All done for the day!</>
                    }
                    
                </Card>
            </Col>
        </Row>
    )
}

export default ScheduleTracker

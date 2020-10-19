import React, { useState } from 'react'
import { useForm } from 'react-hook-form'
import { Form, Button, Modal } from 'react-bootstrap';
import { useStores } from '../hooks/use-stores';
import { observable } from 'mobx';

type Inputs = {
    scheduleName: string;
    file: FileList;
};

interface Props {
    
}

const UploadSchedule = (props: Props) => {
    const { scheduleStore } = useStores();
    const [show, setShow] = useState(false);

    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);

    const { register, handleSubmit } = useForm<Inputs>();
    const onSubmit = async (data: Inputs) => {
        console.log(data);
        await scheduleStore.addSchedule(data.scheduleName, data.file[0])
        handleClose();
    };

    return (
        <div style={{marginBottom: "20px"}}>
        <Button variant="success" onClick={handleShow}>+</Button>
        <Modal show={show} onHide={handleClose}>
            <Modal.Header className="content-justify-center">
                <Modal.Title>Add Schedule</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Form onSubmit={handleSubmit(onSubmit)}>
                    <Form.Group controlId="formScheduleName">
                        <Form.Label>Schedule Name</Form.Label>
                        <Form.Control required type="text" placeholder="Enter a name for this schedule" name="scheduleName" ref={register} />
                    </Form.Group>
                    <Form.Group controlId="formFile">
                        <Form.Label>File To Process</Form.Label>
                        <Form.Control required type="file" name="file" ref={register} />
                    </Form.Group>
                    <Button variant="primary" type="submit">Process Schedule</Button>
                </Form>
            </Modal.Body>
        </Modal>
        </div>
    )
}

export default UploadSchedule

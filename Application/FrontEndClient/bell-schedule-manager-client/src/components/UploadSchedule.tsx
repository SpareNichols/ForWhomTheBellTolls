import React from 'react'
import { useForm } from 'react-hook-form'
import { Form, Button } from 'react-bootstrap';

type Inputs = {
    scheduleName: string;
    file: FileList;
};

interface Props {
    
}

const UploadSchedule = (props: Props) => {
    const { register, handleSubmit } = useForm<Inputs>();
    const onSubmit = (data: Inputs) => {
        console.log(data);
        const formData = new FormData();
        formData.append("scheduleName", data.scheduleName);
        formData.append("file", data.file[0])
        fetch('https://localhost:5001/api/schedule/update-from-file', {
            credentials: 'include',
            method: 'POST',
            body: formData
        })
    };

    return (
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
        // <form onSubmit={handleSubmit(onSubmit)}>
        //     <input name="scheduleName" placeholder="Name Of Schedule" ref={register} />
        //     <input type="file" name="file" ref={register} />
        //     <input type="submit" />
        // </form>
    )
}

export default UploadSchedule

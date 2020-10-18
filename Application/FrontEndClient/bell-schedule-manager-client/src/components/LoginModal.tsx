import React, { useState } from 'react'
import { Modal, Button } from 'react-bootstrap'
import GoogleButton from 'react-google-button'

const API_URL_BASE = "https://localhost:5001"

interface Props {
    
}

class GoogleLoginResponse {
    redirectUrl!: string
}

const LoginButtonWithModal = (props: Props) => {
    const [show, setShow] = useState(false);

    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);
    const handleGoogleLogin = async () => { 

        const resp = await fetch(`${API_URL_BASE}/api/auth/google-login`, {
            method: 'GET'
        });

        let body: GoogleLoginResponse = await resp.json();

        if (body.redirectUrl) {
            console.log('redirecting to: ' + body.redirectUrl);
            window.location.replace(body.redirectUrl);
        }
    }

    return (
        <>
            <Button onClick={handleShow}>Login</Button>
            <Modal show={show} onHide={handleClose}>
                <Modal.Header className="content-justify-center">
                    <Modal.Title>Login</Modal.Title>
                </Modal.Header>

                <Modal.Body>
                    <GoogleButton onClick={handleGoogleLogin} />
                </Modal.Body>
            </Modal>
        </>
    )
}

export default LoginButtonWithModal

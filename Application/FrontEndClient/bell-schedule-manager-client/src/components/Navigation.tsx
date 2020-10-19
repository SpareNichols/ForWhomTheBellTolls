import React from 'react'
import { Navbar, Nav, NavDropdown } from 'react-bootstrap'
import { observer } from 'mobx-react'
import { useStores } from '../hooks/use-stores'
import LoginButtonWithModal from './LoginModal'

interface Props {
    
}

const Navigation = observer((props: Props) => {
    const { currentUserStore, scheduleStore } = useStores();

    return (
        <Navbar bg="light" expand="lg">
            <Navbar.Brand>For Whom The Bell Tolls</Navbar.Brand>
            <Nav className="justify-content-end">
                {(currentUserStore.isLoggedIn
                    ? <NavDropdown title={currentUserStore.user?.displayName} id="current-user-dropdown">
                        <NavDropdown.Item onClick={() => currentUserStore.signMeOut(scheduleStore)}>Log Out</NavDropdown.Item>
                    </NavDropdown>
                    : <Nav.Item>
                        <LoginButtonWithModal/>
                    </Nav.Item>)}
            </Nav>
        </Navbar>
    )
})

export default Navigation

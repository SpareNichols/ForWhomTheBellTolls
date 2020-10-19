import React from 'react'
import { Navbar, Nav, NavDropdown } from 'react-bootstrap'
import { observer } from 'mobx-react'
import { useStores } from '../hooks/use-stores'
import LoginButtonWithModal from './LoginModal'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faBell } from '@fortawesome/free-solid-svg-icons'

interface Props {
    
}

const Navigation = observer((props: Props) => {
    const { currentUserStore, scheduleStore } = useStores();

    return (
        <Navbar bg="light" expand="md">
            <Navbar.Brand>
                <FontAwesomeIcon icon={faBell} style={{paddingRight: "5px"}}/>
                For Whom The Bell Tolls
            </Navbar.Brand>
            <Navbar.Toggle />
            <Navbar.Collapse className="justify-content-end">
                <Nav className="justify-content-end">
                    {(currentUserStore.isLoggedIn
                        ? <NavDropdown title={currentUserStore.user?.displayName} id="current-user-dropdown">
                            <NavDropdown.Item onClick={() => currentUserStore.signMeOut(scheduleStore)}>Log Out</NavDropdown.Item>
                        </NavDropdown>
                        : <Nav.Item>
                            <LoginButtonWithModal/>
                        </Nav.Item>)}
                </Nav>
            </Navbar.Collapse>
        </Navbar>
    )
})

export default Navigation

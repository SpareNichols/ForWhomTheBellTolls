import React from 'react'
import { Navbar, Nav } from 'react-bootstrap'
import { observer } from 'mobx-react'
import { useStores } from '../hooks/use-stores'

interface Props {
    
}

const Navigation = observer((props: Props) => {

    const { currentUserStore } = useStores();

    return (
        <Navbar bg="light" expand="lg" fixed="top">
            <Navbar.Brand>For Whom The Bell Tolls</Navbar.Brand>
            <Nav className="justify-content-end">
                {(currentUserStore.isLoggedIn
                    ? <div>{currentUserStore.user?.name}</div>
                    : <Nav.Item>
                        <Nav.Link onClick={() => currentUserStore.getMe()}>Log In</Nav.Link>
                    </Nav.Item>)}
            </Nav>
        </Navbar>
    )
})

export default Navigation

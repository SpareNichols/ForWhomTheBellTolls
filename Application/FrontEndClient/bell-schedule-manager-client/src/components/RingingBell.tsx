import React from 'react'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faBell } from '@fortawesome/free-solid-svg-icons'
import './RingingBell.css'

interface Props {
    size: string,
    color: string
}

const RingingBell = (props: Props) => {
    return (
        <FontAwesomeIcon icon={faBell} className="bell" style={{height: props.size, width: props.size, color: props.color}} />
    )
}

export default RingingBell

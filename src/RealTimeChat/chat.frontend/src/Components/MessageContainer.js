import React, { useEffect, useRef } from 'react'

const MessageContainer = ({ messages }) => {
  const messageRef = useRef()

  useEffect(() => {
    if (messageRef && messageRef.current) {
      const { scrollHeight, clientHeight } = messageRef.current
      messageRef.current.scrollTo({
        left: 0,
        top: scrollHeight - clientHeight,
        behaviour: 'smooth',
      })
    }
  }, [messages])

  return (
    <div ref={messageRef} className='message-container'>
      {messages.map((message, index) => (
        <div key={index} className='user-message'>
          <div className='message bg-primary'>{message.message}</div>
          <div className='from-user'>{message.user}</div>
        </div>
      ))}
    </div>
  )
}

export default MessageContainer

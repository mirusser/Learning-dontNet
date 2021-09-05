import React, { useState } from 'react'
import './App.css'
import 'bootstrap/dist/css/bootstrap.min.css'
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr'
import Lobby from './Components/Lobby'
import Chat from './Components/Chat'

const App = () => {
  const [connection, setConnection] = useState()
  const [messages, setMessages] = useState([])
  const [users, setUsers] = useState([])

  const joinRoom = async (user, room) => {
    try {
      const connection = new HubConnectionBuilder()
        .withUrl('http://localhost:22733/chat')
        .configureLogging(LogLevel.Information)
        .build()

      connection.on('UsersInRoom', (users) => {
        setUsers(users)
      })

      connection.on('ReceiveMessage', (user, message) => {
        console.log(`message received: ${message}`)
        setMessages((messages) => [...messages, { user, message }])
      })

      connection.onclose((e) => {
        setConnection()
        setMessages([])
        setUsers([])
      })

      await connection.start()
      await connection.invoke('JoinRoom', { user, room })
      setConnection(connection)
    } catch (error) {
      console.log(error)
    }
  }

  const closeConnection = async () => {
    try {
      await connection.stop()
    } catch (error) {
      console.log(error)
    }
  }

  const sendMessage = async (message) => {
    try {
      await connection.invoke('SendMessage', message)
    } catch (error) {
      console.log(error)
    }
  }
  return (
    <div className='app'>
      <h2>Real-Time chat</h2>
      {/* <hr className='line' /> */}
      {!connection ? (
        <Lobby joinRoom={joinRoom} />
      ) : (
        <Chat
          messages={messages}
          sendMessage={sendMessage}
          closeConnection={closeConnection}
          users={users}
        />
      )}
    </div>
  )
}

export default App

import React from 'react'

const ConnectedUsers = ({ users }) => {
  return (
    <div className='user-list'>
      <h4>Connected Users</h4>
      {users.map((user, index) => {
        return <h5 key={index}>{user}</h5>
      })}
    </div>
  )
}

export default ConnectedUsers

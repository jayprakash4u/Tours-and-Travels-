import { useState, useEffect } from 'react'
import { Navbar } from '@components/Navbar'
import { Card } from '@components/Card'
import { Button } from '@components/Button'
import { LoadingSpinner } from '@components/LoadingSpinner'
import { ticketService } from '@services/ticketService'
import { useAuthStore } from '@store/authStore'
import { PlaneIcon, CarIcon, TrainIcon } from '../icons'

export function TicketsPage() {
  const [tickets, setTickets] = useState([])
  const [isLoading, setIsLoading] = useState(true)
  const [error, setError] = useState(null)
  const { user } = useAuthStore()

  useEffect(() => {
    loadTickets()
  }, [])

  const loadTickets = async () => {
    try {
      setIsLoading(true)
      const data = await ticketService.getMyTickets(user?.id)
      setTickets(data || [])
    } catch (err) {
      setError(err.message)
      console.error('Error loading tickets:', err)
    } finally {
      setIsLoading(false)
    }
  }

  if (isLoading) {
    return (
      <>
        <Navbar />
        <LoadingSpinner fullScreen />
      </>
    )
  }

  return (
    <>
      <Navbar />
      <div className="dashboard-container">
        <Card title="My Tickets">
          {tickets.length === 0 ? (
            <div className="empty-state">
              <span className="empty-icon"><PlaneIcon /></span>
              <h3>No Tickets Yet</h3>
              <p>You haven't booked any tickets yet. Book your first trip today!</p>
              <Button variant="primary">Book Now</Button>
            </div>
          ) : (
            <div className="tickets-list">
              {tickets.map((ticket) => (
                <div key={ticket.id} className="ticket-item">
                  <div className="ticket-info">
                    <h4>{ticket.ticketType}</h4>
                    <p>{ticket.from} to {ticket.to}</p>
                    <p className="ticket-date">{new Date(ticket.travelDate).toLocaleDateString()}</p>
                  </div>
                  <div className="ticket-status">
                    <span className={`status-badge status-${ticket.status?.toLowerCase()}`}>
                      {ticket.status}
                    </span>
                  </div>
                </div>
              ))}
            </div>
          )}
        </Card>

        <Card title="Book a Ticket">
          <div className="ticket-types">
            <div className="ticket-type">
              <span className="type-icon"><PlaneIcon /></span>
              <h4>Flights</h4>
              <p>Book domestic and international flights</p>
              <Button variant="outline" size="sm">Search Flights</Button>
            </div>
            <div className="ticket-type">
              <span className="type-icon"><CarIcon /></span>
              <h4>Bus</h4>
              <p>Inter-city bus tickets</p>
              <Button variant="outline" size="sm">Search Buses</Button>
            </div>
            <div className="ticket-type">
              <span className="type-icon"><TrainIcon /></span>
              <h4>Train</h4>
              <p>Railway ticket booking</p>
              <Button variant="outline" size="sm">Search Trains</Button>
            </div>
          </div>
        </Card>
      </div>
    </>
  )
}

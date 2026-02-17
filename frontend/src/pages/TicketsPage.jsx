import { useState, useEffect } from 'react'
import { Navbar } from '@components/Navbar'
import { Card } from '@components/Card'
import { Button } from '@components/Button'
import { LoadingSpinner } from '@components/LoadingSpinner'
import { PlaneIcon, CarIcon, TrainIcon } from '../icons'
import { ticketService } from '@services/ticketService'
import { useNotification } from '@hooks/useNotification'

export function TicketsPage() {
  const [tickets, setTickets] = useState([])
  const [isLoading, setIsLoading] = useState(true)
  const [showBookingForm, setShowBookingForm] = useState(false)
  const [formData, setFormData] = useState({
    fromLocation: '',
    toLocation: '',
    travelDate: '',
    ticketType: 'flight',
    numberOfPassengers: 1,
    notes: ''
  })
  const [isSubmitting, setIsSubmitting] = useState(false)
  const { success: showSuccess, error: showError } = useNotification()
  
  const user = localStorage.getItem('user') ? JSON.parse(localStorage.getItem('user')) : null
  const userId = user?.id

  useEffect(() => {
    loadTickets()
  }, [])

  const loadTickets = async () => {
    try {
      setIsLoading(true)
      if (!userId) {
        showError('User not authenticated')
        return
      }
      const response = await ticketService.getUserTickets(userId)
      if (response && response.data) {
        setTickets(Array.isArray(response.data) ? response.data : [])
      } else {
        setTickets([])
      }
    } catch (err) {
      console.error('Error loading tickets:', err)
      showError(err.response?.data?.message || err.message || 'Error loading tickets')
    } finally {
      setIsLoading(false)
    }
  }

  const handleSubmit = async (e) => {
    e.preventDefault()
    try {
      setIsSubmitting(true)
      if (!userId) {
        showError('User not authenticated')
        return
      }
      const response = await ticketService.createTicket(userId, formData)
      if (response && response.success) {
        showSuccess('Ticket booked successfully!')
        setFormData({
          fromLocation: '',
          toLocation: '',
          travelDate: '',
          ticketType: 'flight',
          numberOfPassengers: 1,
          notes: ''
        })
        setShowBookingForm(false)
        loadTickets() // Reload tickets
      } else {
        showError(response?.message || 'Failed to book ticket')
      }
    } catch (err) {
      showError(err.response?.data?.message || err.message || 'Error booking ticket')
      console.error('Error booking ticket:', err)
    } finally {
      setIsSubmitting(false)
    }
  }

  const handleInputChange = (e) => {
    const { name, value } = e.target
    setFormData(prev => ({
      ...prev,
      [name]: name === 'numberOfPassengers' ? parseInt(value) || 1 : value
    }))
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
              <Button 
                variant="primary"
                onClick={() => setShowBookingForm(true)}
              >
                Book Now
              </Button>
            </div>
          ) : (
            <div className="tickets-list">
              {tickets.map((ticket) => (
                <div key={ticket.id} className="ticket-item">
                  <div className="ticket-info">
                    <h4>{ticket.ticketType}</h4>
                    <p>{ticket.fromLocation} to {ticket.toLocation}</p>
                    <p className="ticket-date">{new Date(ticket.travelDate).toLocaleDateString()}</p>
                    <p className="ticket-details">Passengers: {ticket.numberOfPassengers || 1}</p>
                    {ticket.estimatedPrice && <p className="ticket-price">${ticket.estimatedPrice}</p>}
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

        {showBookingForm ? (
          <Card title="Book a Ticket">
            <form onSubmit={handleSubmit} className="booking-form">
              {/* Ticket Type Selector */}
              <div className="ticket-type-selector">
                <button
                  type="button"
                  className={`ticket-type-btn ${formData.ticketType === 'flight' ? 'active' : ''}`}
                  onClick={() => setFormData(prev => ({ ...prev, ticketType: 'flight' }))}
                >
                  <span className="type-icon">✈️</span>
                  <span>Flight</span>
                </button>
                <button
                  type="button"
                  className={`ticket-type-btn ${formData.ticketType === 'bus' ? 'active' : ''}`}
                  onClick={() => setFormData(prev => ({ ...prev, ticketType: 'bus' }))}
                >
                  <span className="type-icon">🚌</span>
                  <span>Bus</span>
                </button>
                <button
                  type="button"
                  className={`ticket-type-btn ${formData.ticketType === 'train' ? 'active' : ''}`}
                  onClick={() => setFormData(prev => ({ ...prev, ticketType: 'train' }))}
                >
                  <span className="type-icon">🚂</span>
                  <span>Train</span>
                </button>
              </div>

              <div className="form-row">
                <div className="form-group">
                  <label htmlFor="fromLocation">From Location</label>
                  <input
                    type="text"
                    id="fromLocation"
                    name="fromLocation"
                    value={formData.fromLocation}
                    onChange={handleInputChange}
                    placeholder="Departure city"
                    required
                  />
                </div>
                <div className="form-group">
                  <label htmlFor="toLocation">To Location</label>
                  <input
                    type="text"
                    id="toLocation"
                    name="toLocation"
                    value={formData.toLocation}
                    onChange={handleInputChange}
                    placeholder="Destination city"
                    required
                  />
                </div>
              </div>

              <div className="form-row">
                <div className="form-group">
                  <label htmlFor="travelDate">Travel Date</label>
                  <input
                    type="date"
                    id="travelDate"
                    name="travelDate"
                    value={formData.travelDate}
                    onChange={handleInputChange}
                    required
                  />
                </div>
                <div className="form-group">
                  <label htmlFor="numberOfPassengers">Number of Passengers</label>
                  <input
                    type="number"
                    id="numberOfPassengers"
                    name="numberOfPassengers"
                    min="1"
                    max="10"
                    value={formData.numberOfPassengers}
                    onChange={handleInputChange}
                  />
                </div>
              </div>

              <div className="form-group">
                <label htmlFor="notes">Special Requirements</label>
                <textarea
                  id="notes"
                  name="notes"
                  value={formData.notes}
                  onChange={handleInputChange}
                  placeholder="Any special requirements? (optional)"
                  rows="3"
                />
              </div>
              <div className="form-actions">
                <Button 
                  variant="primary" 
                  type="submit"
                  isLoading={isSubmitting}
                >
                  Book Ticket
                </Button>
                <Button 
                  variant="outline" 
                  type="button"
                  onClick={() => setShowBookingForm(false)}
                >
                  Cancel
                </Button>
              </div>
            </form>
          </Card>
        ) : (
          <Card title="Book a Ticket">
            <div className="ticket-types">
              <div className="ticket-type">
                <span className="type-icon"><PlaneIcon /></span>
                <h4>Flights</h4>
                <p>Book domestic and international flights</p>
                <Button 
                  variant="outline" 
                  size="sm"
                  onClick={() => {
                    setFormData(prev => ({ ...prev, ticketType: 'flight' }))
                    setShowBookingForm(true)
                  }}
                >
                  Search Flights
                </Button>
              </div>
              <div className="ticket-type">
                <span className="type-icon"><CarIcon /></span>
                <h4>Bus</h4>
                <p>Inter-city bus tickets</p>
                <Button 
                  variant="outline" 
                  size="sm"
                  onClick={() => {
                    setFormData(prev => ({ ...prev, ticketType: 'bus' }))
                    setShowBookingForm(true)
                  }}
                >
                  Search Buses
                </Button>
              </div>
              <div className="ticket-type">
                <span className="type-icon"><TrainIcon /></span>
                <h4>Train</h4>
                <p>Railway ticket booking</p>
                <Button 
                  variant="outline" 
                  size="sm"
                  onClick={() => {
                    setFormData(prev => ({ ...prev, ticketType: 'train' }))
                    setShowBookingForm(true)
                  }}
                >
                  Search Trains
                </Button>
              </div>
            </div>
          </Card>
        )}
      </div>
    </>
  )
}

import { useState, useEffect } from 'react'
import { Navbar } from '@components/Navbar'
import { Card } from '@components/Card'
import { Button } from '@components/Button'
import { LoadingSpinner } from '@components/LoadingSpinner'
import { CarIcon, DriveIcon, BusIcon } from '../icons'
import { vehicleService } from '@services/vehicleService'
import { useNotification } from '@hooks/useNotification'

export function VehiclesPage() {
  const [bookings, setBookings] = useState([])
  const [isLoading, setIsLoading] = useState(true)
  const [showBookingForm, setShowBookingForm] = useState(false)
  const [formData, setFormData] = useState({
    vehicleType: 'economy',
    pickupDate: '',
    dropDate: '',
    pickupLocation: '',
    dropLocation: '',
    numberOfPassengers: 1,
    notes: ''
  })
  const [isSubmitting, setIsSubmitting] = useState(false)
  const { success: showSuccess, error: showError } = useNotification()
  
  const user = localStorage.getItem('user') ? JSON.parse(localStorage.getItem('user')) : null
  const userId = user?.id

  useEffect(() => {
    loadBookings()
  }, [])

  const loadBookings = async () => {
    try {
      setIsLoading(true)
      if (!userId) {
        showError('User not authenticated')
        return
      }
      const response = await vehicleService.getUserBookings(userId)
      if (response && response.data) {
        setBookings(Array.isArray(response.data) ? response.data : [])
      } else {
        setBookings([])
      }
    } catch (err) {
      console.error('Error loading bookings:', err)
      showError(err.response?.data?.message || err.message || 'Error loading bookings')
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
      const response = await vehicleService.createBooking(userId, formData)
      if (response && response.success) {
        showSuccess('Vehicle booked successfully!')
        setFormData({
          vehicleType: 'economy',
          pickupDate: '',
          dropDate: '',
          pickupLocation: '',
          dropLocation: '',
          numberOfPassengers: 1,
          notes: ''
        })
        setShowBookingForm(false)
        loadBookings() // Reload bookings
      } else {
        showError(response?.message || 'Failed to book vehicle')
      }
    } catch (err) {
      showError(err.response?.data?.message || err.message || 'Error booking vehicle')
      console.error('Error booking vehicle:', err)
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
        <Card title="My Vehicle Bookings">
          {bookings.length === 0 ? (
            <div className="empty-state">
              <span className="empty-icon"><DriveIcon /></span>
              <h3>No Bookings Yet</h3>
              <p>You haven't rented any vehicles yet. Book your first ride today!</p>
              <Button 
                variant="primary"
                onClick={() => setShowBookingForm(true)}
              >
                Browse Vehicles
              </Button>
            </div>
          ) : (
            <div className="bookings-list">
              {bookings.map((booking) => (
                <div key={booking.id} className="booking-item">
                  <div className="booking-info">
                    <h4>{booking.vehicleType}</h4>
                    <p>{booking.pickupLocation} to {booking.dropLocation}</p>
                    <p className="booking-date">
                      {new Date(booking.pickupDate).toLocaleDateString()} - {new Date(booking.dropDate).toLocaleDateString()}
                    </p>
                    <p className="booking-details">Passengers: {booking.numberOfPassengers || 1}</p>
                    {booking.totalCost && <p className="booking-price">${booking.totalCost}</p>}
                  </div>
                  <div className="booking-status">
                    <span className={`status-badge status-${booking.status?.toLowerCase()}`}>
                      {booking.status}
                    </span>
                  </div>
                </div>
              ))}
            </div>
          )}
        </Card>

        {showBookingForm ? (
          <Card title="Rent a Vehicle">
            <form onSubmit={handleSubmit} className="booking-form">
              <div className="form-group">
                <label htmlFor="vehicleType">Vehicle Type:</label>
                <select
                  id="vehicleType"
                  name="vehicleType"
                  value={formData.vehicleType}
                  onChange={handleInputChange}
                >
                  <option value="economy">Economy Car</option>
                  <option value="suv">SUV</option>
                  <option value="minibus">Minibus</option>
                  <option value="luxury">Luxury Car</option>
                </select>
              </div>
              <div className="form-group">
                <label htmlFor="pickupLocation">Pickup Location:</label>
                <input
                  type="text"
                  id="pickupLocation"
                  name="pickupLocation"
                  value={formData.pickupLocation}
                  onChange={handleInputChange}
                  placeholder="Where to pick up?"
                  required
                />
              </div>
              <div className="form-group">
                <label htmlFor="dropLocation">Drop-off Location:</label>
                <input
                  type="text"
                  id="dropLocation"
                  name="dropLocation"
                  value={formData.dropLocation}
                  onChange={handleInputChange}
                  placeholder="Where to drop off?"
                  required
                />
              </div>
              <div className="form-group">
                <label htmlFor="pickupDate">Pickup Date:</label>
                <input
                  type="date"
                  id="pickupDate"
                  name="pickupDate"
                  value={formData.pickupDate}
                  onChange={handleInputChange}
                  required
                />
              </div>
              <div className="form-group">
                <label htmlFor="dropDate">Drop-off Date:</label>
                <input
                  type="date"
                  id="dropDate"
                  name="dropDate"
                  value={formData.dropDate}
                  onChange={handleInputChange}
                  required
                />
              </div>
              <div className="form-group">
                <label htmlFor="numberOfPassengers">Number of Passengers:</label>
                <input
                  type="number"
                  id="numberOfPassengers"
                  name="numberOfPassengers"
                  min="1"
                  value={formData.numberOfPassengers}
                  onChange={handleInputChange}
                />
              </div>
              <div className="form-group">
                <label htmlFor="notes">Notes:</label>
                <textarea
                  id="notes"
                  name="notes"
                  value={formData.notes}
                  onChange={handleInputChange}
                  placeholder="Any special requirements?"
                  rows="3"
                />
              </div>
              <div className="form-actions">
                <Button 
                  variant="primary" 
                  type="submit"
                  isLoading={isSubmitting}
                >
                  Book Vehicle
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
          <Card title="Rent a Vehicle">
            <div className="vehicle-types">
              <div className="vehicle-type">
                <span className="type-icon"><CarIcon /></span>
                <h4>Economy</h4>
                <p>Compact cars for budget travel</p>
                <span className="price">From $30/day</span>
                <Button 
                  variant="outline" 
                  size="sm"
                  onClick={() => {
                    setFormData(prev => ({ ...prev, vehicleType: 'economy' }))
                    setShowBookingForm(true)
                  }}
                >
                  Rent Now
                </Button>
              </div>
              <div className="vehicle-type">
                <span className="type-icon"><DriveIcon /></span>
                <h4>SUV</h4>
                <p>Spacious SUVs for family trips</p>
                <span className="price">From $50/day</span>
                <Button 
                  variant="outline" 
                  size="sm"
                  onClick={() => {
                    setFormData(prev => ({ ...prev, vehicleType: 'suv' }))
                    setShowBookingForm(true)
                  }}
                >
                  Rent Now
                </Button>
              </div>
              <div className="vehicle-type">
                <span className="type-icon"><BusIcon /></span>
                <h4>Minibus</h4>
                <p>Large groups and corporate travel</p>
                <span className="price">From $80/day</span>
                <Button 
                  variant="outline" 
                  size="sm"
                  onClick={() => {
                    setFormData(prev => ({ ...prev, vehicleType: 'minibus' }))
                    setShowBookingForm(true)
                  }}
                >
                  Rent Now
                </Button>
              </div>
            </div>
          </Card>
        )}
      </div>
    </>
  )
}

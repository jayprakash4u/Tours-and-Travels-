import { useState, useEffect } from 'react'
import { Navbar } from '@components/Navbar'
import { Card } from '@components/Card'
import { Button } from '@components/Button'
import { LoadingSpinner } from '@components/LoadingSpinner'
import { vehicleService } from '@services/vehicleService'
import { useAuthStore } from '@store/authStore'
import { CarIcon, DriveIcon, BusIcon, StarIcon } from '../icons'

export function VehiclesPage() {
  const [bookings, setBookings] = useState([])
  const [isLoading, setIsLoading] = useState(true)
  const [error, setError] = useState(null)
  const { user } = useAuthStore()

  useEffect(() => {
    loadBookings()
  }, [])

  const loadBookings = async () => {
    try {
      setIsLoading(true)
      const data = await vehicleService.getMyBookings(user?.id)
      setBookings(data || [])
    } catch (err) {
      setError(err.message)
      console.error('Error loading bookings:', err)
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
        <Card title="My Vehicle Bookings">
          {bookings.length === 0 ? (
            <div className="empty-state">
              <span className="empty-icon"><DriveIcon /></span>
              <h3>No Bookings Yet</h3>
              <p>You haven't rented any vehicles yet. Book your first ride today!</p>
              <Button variant="primary">Browse Vehicles</Button>
            </div>
          ) : (
            <div className="bookings-list">
              {bookings.map((booking) => (
                <div key={booking.id} className="booking-item">
                  <div className="booking-info">
                    <h4>{booking.vehicleType}</h4>
                    <p>{booking.pickupLocation} to {booking.dropoffLocation}</p>
                    <p className="booking-date">
                      {new Date(booking.pickupDate).toLocaleDateString()} - {new Date(booking.dropoffDate).toLocaleDateString()}
                    </p>
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

        <Card title="Rent a Vehicle">
          <div className="vehicle-types">
            <div className="vehicle-type">
              <span className="type-icon"><CarIcon /></span>
              <h4>Economy</h4>
              <p>Compact cars for budget travel</p>
              <span className="price">From $30/day</span>
              <Button variant="outline" size="sm">Rent Now</Button>
            </div>
            <div className="vehicle-type">
              <span className="type-icon"><DriveIcon /></span>
              <h4>SUV</h4>
              <p>Spacious SUVs for family trips</p>
              <span className="price">From $50/day</span>
              <Button variant="outline" size="sm">Rent Now</Button>
            </div>
            <div className="vehicle-type">
              <span className="type-icon"><BusIcon /></span>
              <h4>Minibus</h4>
              <p>Large groups and corporate travel</p>
              <span className="price">From $80/day</span>
              <Button variant="outline" size="sm">Rent Now</Button>
            </div>
            <div className="vehicle-type">
              <span className="type-icon"><StarIcon /></span>
              <h4>Luxury</h4>
              <p>Premium cars for special occasions</p>
              <span className="price">From $100/day</span>
              <Button variant="outline" size="sm">Rent Now</Button>
            </div>
          </div>
        </Card>
      </div>
    </>
  )
}

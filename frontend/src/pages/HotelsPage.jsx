import { useState } from 'react'
import { Button, Input, Navbar } from '@components/index'
import { useNotification } from '@hooks/useNotification'

export function HotelsPage() {
  const [activeTab, setActiveTab] = useState('book')
  const [bookings, setBookings] = useState([])
  const { success: showSuccess, error: showError } = useNotification()
  
  const [formData, setFormData] = useState({
    hotelName: '',
    location: '',
    checkIn: '',
    checkOut: '',
    guests: 1,
    guestName: '',
    guestEmail: '',
    guestPhone: '',
  })

  const handleSubmit = (e) => {
    e.preventDefault()
    
    if (!formData.hotelName || !formData.location || !formData.checkIn || !formData.checkOut) {
      showError('Please fill in all required fields')
      return
    }

    const newBooking = {
      id: Date.now(),
      hotelName: formData.hotelName,
      location: formData.location,
      checkIn: formData.checkIn,
      checkOut: formData.checkOut,
      guests: formData.guests,
      status: 'pending',
    }

    setBookings([newBooking, ...bookings])
    showSuccess('Hotel booking request submitted successfully!')
    
    setFormData({
      hotelName: '',
      location: '',
      checkIn: '',
      checkOut: '',
      guests: 1,
      guestName: '',
      guestEmail: '',
      guestPhone: '',
    })
    
    setActiveTab('my-bookings')
  }

  const handleInputChange = (e) => {
    const { name, value } = e.target
    setFormData(prev => ({
      ...prev,
      [name]: name === 'guests' ? parseInt(value) || 1 : value,
    }))
  }

  const getStatusClass = (status) => {
    switch (status) {
      case 'confirmed':
        return 'status-confirmed'
      case 'pending':
        return 'status-pending'
      case 'cancelled':
        return 'status-rejected'
      default:
        return ''
    }
  }

  return (
    <div>
      <Navbar />
      <div className="hotels-page">
        <div className="page-container">
          <div className="page-header">
            <h1 className="page-title">Hotel Booking</h1>
            <p className="page-subtitle">Book hotels and accommodations for your travel</p>
          </div>

          <div className="tabs">
            <button
              className={`tab ${activeTab === 'book' ? 'active' : ''}`}
              onClick={() => setActiveTab('book')}
            >
              Book a Hotel
            </button>
            <button
              className={`tab ${activeTab === 'my-bookings' ? 'active' : ''}`}
              onClick={() => setActiveTab('my-bookings')}
            >
              My Bookings ({bookings.length})
            </button>
          </div>

          {activeTab === 'book' && (
            <div className="service-section">
              <h2 className="service-section-title">Hotel Information</h2>
              <form onSubmit={handleSubmit} className="service-form">
                <div className="form-row">
                  <Input
                    label="Hotel Name"
                    name="hotelName"
                    placeholder="e.g., Grand Hotel Kathmandu"
                    value={formData.hotelName}
                    onChange={handleInputChange}
                    required
                  />
                  <Input
                    label="Location"
                    name="location"
                    placeholder="e.g., Kathmandu, Nepal"
                    value={formData.location}
                    onChange={handleInputChange}
                    required
                  />
                </div>
                
                <div className="form-row">
                  <Input
                    label="Check-in Date"
                    name="checkIn"
                    type="date"
                    value={formData.checkIn}
                    onChange={handleInputChange}
                    required
                  />
                  <Input
                    label="Check-out Date"
                    name="checkOut"
                    type="date"
                    value={formData.checkOut}
                    onChange={handleInputChange}
                    required
                  />
                </div>

                <div className="form-row">
                  <Input
                    label="Number of Guests"
                    name="guests"
                    type="number"
                    min="1"
                    max="10"
                    value={formData.guests}
                    onChange={handleInputChange}
                    required
                  />
                  <div></div>
                </div>

                <h3 className="service-section-title" style={{ marginTop: '1.5rem' }}>Guest Details</h3>
                
                <div className="form-row">
                  <Input
                    label="Full Name"
                    name="guestName"
                    placeholder="Your full name"
                    value={formData.guestName}
                    onChange={handleInputChange}
                    required
                  />
                  <Input
                    label="Email"
                    name="guestEmail"
                    type="email"
                    placeholder="your@email.com"
                    value={formData.guestEmail}
                    onChange={handleInputChange}
                    required
                  />
                </div>

                <div className="form-row">
                  <Input
                    label="Phone Number"
                    name="guestPhone"
                    placeholder="+977 98XXXXXXXX"
                    value={formData.guestPhone}
                    onChange={handleInputChange}
                    required
                  />
                  <div></div>
                </div>

                <div className="form-actions">
                  <Button type="submit" variant="primary">
                    Book Now
                  </Button>
                </div>
              </form>
            </div>
          )}

          {activeTab === 'my-bookings' && (
            <div className="service-section">
              <h2 className="service-section-title">My Hotel Bookings</h2>
              
              {bookings.length === 0 ? (
                <div className="empty-state">
                  <span className="empty-icon">🏨</span>
                  <h3>No Hotel Bookings Yet</h3>
                  <p>Start by booking your first hotel!</p>
                  <Button variant="primary" onClick={() => setActiveTab('book')}>
                    Book a Hotel
                  </Button>
                </div>
              ) : (
                <div className="bookings-list">
                  {bookings.map((booking) => (
                    <div key={booking.id} className="booking-item">
                      <div className="booking-info">
                        <h4>{booking.hotelName}</h4>
                        <p>{booking.location}</p>
                        <p>
                          {booking.checkIn} - {booking.checkOut} • {booking.guests} guest(s)
                        </p>
                      </div>
                      <span className={`booking-status ${getStatusClass(booking.status)}`}>
                        {booking.status.charAt(0).toUpperCase() + booking.status.slice(1)}
                      </span>
                    </div>
                  ))}
                </div>
              )}
            </div>
          )}
        </div>
      </div>
    </div>
  )
}

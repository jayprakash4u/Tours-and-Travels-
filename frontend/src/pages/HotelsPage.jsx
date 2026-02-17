import { useState, useEffect } from 'react'
import { Button, Input, Navbar, LoadingSpinner, Card } from '@components/index'
import { useNotification } from '@hooks/useNotification'
import { hotelService } from '@services/hotelService'

export function HotelsPage() {
  const [activeTab, setActiveTab] = useState('book')
  const [bookings, setBookings] = useState([])
  const [isLoading, setIsLoading] = useState(false)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const { success: showSuccess, error: showError } = useNotification()
  
  const user = localStorage.getItem('user') ? JSON.parse(localStorage.getItem('user')) : null
  const userId = user?.id
  
  const [formData, setFormData] = useState({
    hotelName: '',
    location: '',
    checkInDate: '',
    checkOutDate: '',
    numberOfGuests: 1,
    guestName: '',
    guestEmail: '',
    guestPhone: '',
    roomType: 'standard',
    specialRequests: ''
  })

  useEffect(() => {
    if (activeTab === 'my-bookings') {
      loadBookings()
    }
  }, [activeTab, userId])

  const loadBookings = async () => {
    try {
      setIsLoading(true)
      if (!userId) {
        showError('User not authenticated')
        return
      }
      const response = await hotelService.getUserBookings(userId)
      console.log('Raw bookings response:', response)
      
      // Handle different response structures
      let bookingsData = []
      if (response && typeof response === 'object') {
        // Check for nested data property
        if (response.data && Array.isArray(response.data)) {
          bookingsData = response.data
        } else if (Array.isArray(response)) {
          bookingsData = response
        } else {
          console.warn('Unexpected response structure:', response)
        }
      }
      
      console.log('Extracted bookings:', bookingsData)
      setBookings(bookingsData)
    } catch (err) {
      console.error('Error loading bookings:', err)
      setBookings([])
      showError(err.response?.data?.message || err.message || 'Error loading bookings')
    } finally {
      setIsLoading(false)
    }
  }

  const handleSubmit = async (e) => {
    e.preventDefault()
    
    if (!formData.hotelName || !formData.location || !formData.checkInDate || !formData.checkOutDate) {
      showError('Please fill in all required fields')
      return
    }

    try {
      setIsSubmitting(true)
      if (!userId) {
        showError('User not authenticated')
        return
      }

      const bookingData = {
        hotelName: formData.hotelName,
        location: formData.location,
        checkInDate: formData.checkInDate,
        checkOutDate: formData.checkOutDate,
        numberOfGuests: formData.numberOfGuests,
        guestName: formData.guestName,
        guestEmail: formData.guestEmail,
        guestPhone: formData.guestPhone,
        roomType: formData.roomType,
        specialRequests: formData.specialRequests
      }

      const response = await hotelService.createBooking(userId, bookingData)
      console.log('Booking response:', response)
      
      if (response && response.success) {
        showSuccess(response.message || 'Hotel booking created successfully!')
        setFormData({
          hotelName: '',
          location: '',
          checkInDate: '',
          checkOutDate: '',
          numberOfGuests: 1,
          guestName: '',
          guestEmail: '',
          guestPhone: '',
          roomType: 'standard',
          specialRequests: ''
        })
        setActiveTab('my-bookings')
        await loadBookings()
      } else {
        showError(response?.message || 'Failed to submit booking')
      }
    } catch (err) {
      console.error('Error booking:', err)
      showError(err.response?.data?.message || err.message || 'Error submitting booking')
    } finally {
      setIsSubmitting(false)
    }
  }

  const handleInputChange = (e) => {
    const { name, value } = e.target
    setFormData(prev => ({
      ...prev,
      [name]: name === 'numberOfGuests' ? parseInt(value) || 1 : value,
    }))
  }

  const getStatusClass = (status) => {
    // Handle both enum number and string values
    let statusStr = typeof status === 'string' ? status : ''
    
    // Map enum numbers to status strings (0=Pending, 1=Confirmed, 2=Cancelled, 3=Completed)
    if (typeof status === 'number') {
      const statusMap = {
        0: 'pending',
        1: 'confirmed',
        2: 'cancelled',
        3: 'completed'
      }
      statusStr = statusMap[status] || 'pending'
    } else if (typeof status === 'string') {
      statusStr = status.toLowerCase()
    }
    
    switch (statusStr) {
      case 'confirmed':
      case '1':
        return 'status-confirmed'
      case 'pending':
      case '0':
        return 'status-pending'
      case 'cancelled':
      case '2':
        return 'status-rejected'
      case 'completed':
      case '3':
        return 'status-confirmed'
      default:
        return 'status-pending'
    }
  }

  const getStatusLabel = (status) => {
    // Handle both enum number and string values
    if (typeof status === 'number') {
      const statusMap = {
        0: 'Pending',
        1: 'Confirmed',
        2: 'Cancelled',
        3: 'Completed'
      }
      return statusMap[status] || 'Pending'
    }
    return status?.charAt(0)?.toUpperCase() + status?.slice(1) || 'Pending'
  }

  if (isLoading && activeTab === 'my-bookings') {
    return (
      <>
        <Navbar />
        <LoadingSpinner fullScreen />
      </>
    )
  }

  return (
    <div>
      <Navbar />
      <div className="hotels-page">
        <div className="page-container">
          <div className="page-header">
            <h1 className="page-title">🏨 Hotel Booking</h1>
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
            <Card title="Hotel Information">
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
                    name="checkInDate"
                    type="date"
                    value={formData.checkInDate}
                    onChange={handleInputChange}
                    required
                  />
                  <Input
                    label="Check-out Date"
                    name="checkOutDate"
                    type="date"
                    value={formData.checkOutDate}
                    onChange={handleInputChange}
                    required
                  />
                </div>

                <div className="form-row">
                  <Input
                    label="Number of Guests"
                    name="numberOfGuests"
                    type="number"
                    min="1"
                    max="10"
                    value={formData.numberOfGuests}
                    onChange={handleInputChange}
                    required
                  />
                  <div>
                    <label htmlFor="roomType">Room Type</label>
                    <select
                      id="roomType"
                      name="roomType"
                      value={formData.roomType}
                      onChange={handleInputChange}
                      style={{ width: '100%', padding: '0.5rem' }}
                    >
                      <option value="standard">Standard Room</option>
                      <option value="deluxe">Deluxe Room</option>
                      <option value="suite">Suite</option>
                      <option value="penthouse">Penthouse</option>
                    </select>
                  </div>
                </div>

                <div style={{ marginTop: '1.5rem' }}>
                  <h3 className="service-section-title">Guest Details</h3>
                </div>
                
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
                </div>

                <div className="form-row">
                  <div style={{ width: '100%' }}>
                    <label htmlFor="specialRequests">Special Requests</label>
                    <textarea
                      id="specialRequests"
                      name="specialRequests"
                      placeholder="Any special requests? (optional)"
                      value={formData.specialRequests}
                      onChange={handleInputChange}
                      rows="3"
                      style={{ width: '100%' }}
                    />
                  </div>
                </div>

                <div className="form-actions">
                  <Button 
                    type="submit" 
                    variant="primary"
                    isLoading={isSubmitting}
                  >
                    Book Now
                  </Button>
                </div>
              </form>
            </Card>
          )}

          {activeTab === 'my-bookings' && (
            <Card title="My Hotel Bookings">
              <div style={{ marginBottom: '1rem', display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                <p style={{ fontSize: '0.9rem', color: '#666' }}>
                  Total bookings: {bookings.length}
                </p>
                <Button 
                  variant="secondary" 
                  onClick={() => loadBookings()}
                  style={{ padding: '0.5rem 1rem', fontSize: '0.85rem' }}
                  disabled={isLoading}
                >
                  {isLoading ? 'Refreshing...' : 'Refresh'}
                </Button>
              </div>
              
              {isLoading ? (
                <div style={{ textAlign: 'center', padding: '2rem' }}>
                  <p>Loading bookings...</p>
                </div>
              ) : bookings.length === 0 ? (
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
                  {bookings.map((booking) => {
                    try {
                      return (
                        <div key={booking.id} className="booking-item">
                          <div className="booking-info">
                            <h4>{booking.hotelName || 'N/A'}</h4>
                            <p>{booking.location || 'N/A'}</p>
                            <p>
                              {booking.checkInDate ? new Date(booking.checkInDate).toLocaleDateString() : 'N/A'} - {booking.checkOutDate ? new Date(booking.checkOutDate).toLocaleDateString() : 'N/A'} • {booking.numberOfGuests || 0} guest(s)
                            </p>
                            {booking.roomType && <p>Room Type: {booking.roomType}</p>}
                          </div>
                          <span className={`booking-status ${getStatusClass(booking.status)}`}>
                            {getStatusLabel(booking.status)}
                          </span>
                        </div>
                      )
                    } catch (err) {
                      console.error('Error rendering booking:', booking, err)
                      return (
                        <div key={booking.id} className="booking-item" style={{ backgroundColor: '#fee' }}>
                          <p>Error displaying booking: {err.message}</p>
                        </div>
                      )
                    }
                  })}
                </div>
              )}
            </Card>
          )}
        </div>
      </div>
    </div>
  )
}

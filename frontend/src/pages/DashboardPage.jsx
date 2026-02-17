import { useEffect, useState } from 'react'
import { useAuthStore } from '@store/authStore'
import { Card, Navbar, LoadingSpinner } from '@components/index'
import { useNotification } from '@hooks/useNotification'
import { 
  PlaneIcon, 
  CarIcon, 
  FileIcon, 
  MoneyIcon, 
  HomeIcon, 
  ShieldIcon,
  SettingsIcon,
  AddIcon,
  UserIcon
} from '../icons'
import { ticketService } from '@services/ticketService'
import { vehicleService } from '@services/vehicleService'
import { immigrationService } from '@services/immigrationService'
import { hotelService } from '@services/hotelService'

// Generate avatar from name
const getAvatar = (name) => {
  const colors = ['#0d9488', '#7c3aed', '#dc2626', '#16a34a', '#d97706', '#2563eb']
  const initials = name.split(' ').map(n => n[0]).join('').toUpperCase().slice(0, 2)
  const colorIndex = name.length % colors.length
  return `data:image/svg+xml,${encodeURIComponent(`<svg xmlns="http://www.w3.org/2000/svg" width="40" height="40" viewBox="0 0 40 40"><rect width="40" height="40" fill="${colors[colorIndex]}" rx="20"/><text x="50%" y="50%" dominant-baseline="central" text-anchor="middle" fill="white" font-family="Arial" font-size="14" font-weight="bold">${initials}</text></svg>`)}`
}

export function DashboardPage() {
  const { user } = useAuthStore()
  const { error: showError } = useNotification()
  const [stats, setStats] = useState([])
  const [recentActivities, setRecentActivities] = useState([])
  const [isLoading, setIsLoading] = useState(true)
  const userId = user?.id

  useEffect(() => {
    if (userId) {
      loadDashboardData()
    }
  }, [userId])

  const loadDashboardData = async () => {
    try {
      setIsLoading(true)
      
      // Fetch data from all services
      const [ticketsRes, vehiclesRes, immigrationRes, hotelsRes] = await Promise.allSettled([
        ticketService.getUserTickets(userId),
        vehicleService.getUserBookings(userId),
        immigrationService.getUserApplications(userId),
        hotelService.getUserBookings(userId)
      ])

      // Extract data safely
      const tickets = ticketsRes.status === 'fulfilled' ? (ticketsRes.value?.data || []) : []
      const vehicles = vehiclesRes.status === 'fulfilled' ? (vehiclesRes.value?.data || []) : []
      const applications = immigrationRes.status === 'fulfilled' ? (immigrationRes.value?.data || []) : []
      const hotels = hotelsRes.status === 'fulfilled' ? (hotelsRes.value?.data || []) : []

      // Calculate stats
      const totalBookings = tickets.length + vehicles.length + hotels.length
      const activeTrips = tickets.filter(t => t.status === 0 || t.status === 'Pending').length + 
                         vehicles.filter(v => v.status === 0 || v.status === 'Pending').length +
                         hotels.filter(h => h.status === 0 || h.status === 'Pending').length
      const totalApplications = applications.length

      setStats([
        { label: 'Total Bookings', value: totalBookings, icon: <FileIcon />, color: '#3b82f6' },
        { label: 'Active Trips', value: activeTrips, icon: <PlaneIcon />, color: '#22c55e' },
        { label: 'Applications', value: totalApplications, icon: <ShieldIcon />, color: '#a855f7' },
        { label: 'Total Services', value: totalBookings + totalApplications, icon: <MoneyIcon />, color: '#f59e0b' },
      ])

      // Build recent activities array
      const activities = []

      // Add ticket bookings
      tickets.forEach(ticket => {
        activities.push({
          title: `Flight booking: ${ticket.fromLocation} → ${ticket.toLocation}`,
          time: new Date(ticket.createdDate),
          icon: <PlaneIcon />,
          status: getStatusName(ticket.status),
          type: 'Ticket'
        })
      })

      // Add vehicle rentals
      vehicles.forEach(vehicle => {
        activities.push({
          title: `${vehicle.vehicleType} rental: ${vehicle.pickupLocation} → ${vehicle.dropLocation}`,
          time: new Date(vehicle.createdDate),
          icon: <CarIcon />,
          status: getStatusName(vehicle.status),
          type: 'Vehicle'
        })
      })

      // Add hotel bookings
      hotels.forEach(hotel => {
        activities.push({
          title: `Hotel: ${hotel.hotelName} in ${hotel.location}`,
          time: new Date(hotel.createdDate),
          icon: <HomeIcon />,
          status: getStatusName(hotel.status),
          type: 'Hotel'
        })
      })

      // Add visa applications
      applications.forEach(app => {
        activities.push({
          title: `${app.visaType} visa application for ${app.targetCountry}`,
          time: new Date(app.createdDate),
          icon: <ShieldIcon />,
          status: getStatusName(app.status),
          type: 'Visa'
        })
      })

      // Sort by date (newest first) and take last 6
      const sortedActivities = activities.sort((a, b) => b.time - a.time).slice(0, 6)
      setRecentActivities(sortedActivities)

    } catch (err) {
      console.error('Error loading dashboard data:', err)
      showError('Failed to load dashboard data')
      setStats([
        { label: 'Total Bookings', value: 0, icon: <FileIcon />, color: '#3b82f6' },
        { label: 'Active Trips', value: 0, icon: <PlaneIcon />, color: '#22c55e' },
        { label: 'Applications', value: 0, icon: <ShieldIcon />, color: '#a855f7' },
        { label: 'Total Services', value: 0, icon: <MoneyIcon />, color: '#f59e0b' },
      ])
      setRecentActivities([])
    } finally {
      setIsLoading(false)
    }
  }

  const getStatusName = (status) => {
    if (typeof status === 'number') {
      const statusMap = {
        0: 'pending',
        1: 'confirmed',
        2: 'cancelled',
        3: 'completed'
      }
      return statusMap[status] || 'pending'
    }
    return (status || 'pending').toLowerCase()
  }

  const getTimeAgo = (date) => {
    const now = new Date()
    const diff = now - date
    const seconds = Math.floor(diff / 1000)
    const minutes = Math.floor(seconds / 60)
    const hours = Math.floor(minutes / 60)
    const days = Math.floor(hours / 24)

    if (days > 0) return `${days}d ago`
    if (hours > 0) return `${hours}h ago`
    if (minutes > 0) return `${minutes}m ago`
    return 'Just now'
  }

  const quickActions = [
    { title: 'Book Flight', desc: 'Search & book flights', icon: <PlaneIcon />, link: '/tickets' },
    { title: 'Rent Vehicle', desc: 'Cars, SUVs & more', icon: <CarIcon />, link: '/vehicles' },
    { title: 'Visa Application', desc: 'Apply for visa', icon: <ShieldIcon />, link: '/immigration' },
    { title: 'Hotel Booking', desc: 'Find best hotels', icon: <HomeIcon />, link: '/hotels' },
    { title: 'Labor Approval', desc: 'Shram Swikriti', icon: <ShieldIcon />, link: '/labor-approval' },
  ]

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
        {/* Profile Header */}
        <div className="dashboard-header-new">
          <div className="profile-section">
            <img 
              src={user?.profilePicture || getAvatar(user?.fullName || 'User')} 
              alt="Profile" 
              className="profile-avatar"
            />
            <div className="profile-info">
              <h1 className="profile-name">Welcome back, {user?.fullName?.split(' ')[0]}!</h1>
              <p className="profile-email">{user?.email}</p>
              <span className="profile-role">{user?.role}</span>
            </div>
          </div>
          <div className="header-actions">
            <button 
              className="btn btn-outline"
              onClick={() => loadDashboardData()}
              disabled={isLoading}
            >
              {isLoading ? 'Refreshing...' : '⟳ Refresh'}
            </button>
            <button className="btn btn-primary"><AddIcon /> New Booking</button>
          </div>
        </div>

        {/* Stats Grid */}
        <div className="stats-grid">
          {stats.map((stat) => (
            <div key={stat.label} className="stat-card">
              <div className="stat-icon" style={{ backgroundColor: stat.color + '20', color: stat.color }}>
                {stat.icon}
              </div>
              <div className="stat-content">
                <p className="stat-value">{stat.value}</p>
                <p className="stat-label">{stat.label}</p>
              </div>
            </div>
          ))}
        </div>

        {/* Main Content Grid */}
        <div className="main-grid">
          {/* Quick Actions */}
          <Card title="Quick Actions">
            <div className="quick-actions-grid">
              {quickActions.map((action) => (
                <a key={action.title} href={action.link} className="quick-action-item">
                  <span className="quick-action-icon">{action.icon}</span>
                  <div className="quick-action-text">
                    <h4>{action.title}</h4>
                    <p>{action.desc}</p>
                  </div>
                </a>
              ))}
            </div>
          </Card>

          {/* Recent Activity */}
          <Card title="Recent Activity">
            <div className="activity-list">
              {recentActivities.length === 0 ? (
                <div style={{ textAlign: 'center', padding: '2rem', color: '#999' }}>
                  <p>No recent activities yet. Start booking to see your activities here!</p>
                </div>
              ) : (
                recentActivities.map((activity, index) => (
                  <div key={index} className="activity-item">
                    <span className="activity-icon">{activity.icon}</span>
                    <div className="activity-content">
                      <h4>{activity.title}</h4>
                      <p>{getTimeAgo(activity.time)}</p>
                    </div>
                    <span className={`activity-status status-${activity.status}`}>
                      {activity.status}
                    </span>
                  </div>
                ))
              )}
            </div>
          </Card>
        </div>

        {/* Upcoming Trips */}
        <Card title="Upcoming Trips">
          <div className="trips-container">
            <div className="trip-card">
              <div className="trip-date">
                <span className="trip-day">15</span>
                <span className="trip-month">Mar</span>
              </div>
              <div className="trip-details">
                <h4>Dubai International Airport</h4>
                <p>Flight EK 304 • 10:30 AM</p>
                <span className="trip-status confirmed">Confirmed</span>
              </div>
              <div className="trip-actions">
                <button className="btn btn-sm btn-outline">View</button>
              </div>
            </div>
            <div className="trip-card">
              <div className="trip-date">
                <span className="trip-day">20</span>
                <span className="trip-month">Mar</span>
              </div>
              <div className="trip-details">
                <h4>Toyota Innova (Rental)</h4>
                <p>Pickup: 9:00 AM • 3 Days</p>
                <span className="trip-status pending">Pending</span>
              </div>
              <div className="trip-actions">
                <button className="btn btn-sm btn-outline">View</button>
              </div>
            </div>
          </div>
        </Card>
      </div>
    </>
  )
}

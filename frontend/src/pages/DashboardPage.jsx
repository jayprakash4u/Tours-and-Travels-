import { useEffect, useState } from 'react'
import { useAuthStore } from '@store/authStore'
import { Card, Navbar, LoadingSpinner } from '@components/index'
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

// Generate avatar from name
const getAvatar = (name) => {
  const colors = ['#0d9488', '#7c3aed', '#dc2626', '#16a34a', '#d97706', '#2563eb']
  const initials = name.split(' ').map(n => n[0]).join('').toUpperCase().slice(0, 2)
  const colorIndex = name.length % colors.length
  return `data:image/svg+xml,${encodeURIComponent(`<svg xmlns="http://www.w3.org/2000/svg" width="40" height="40" viewBox="0 0 40 40"><rect width="40" height="40" fill="${colors[colorIndex]}" rx="20"/><text x="50%" y="50%" dominant-baseline="central" text-anchor="middle" fill="white" font-family="Arial" font-size="14" font-weight="bold">${initials}</text></svg>`)}`
}

export function DashboardPage() {
  const { user } = useAuthStore()
  const [stats, setStats] = useState([])
  const [isLoading, setIsLoading] = useState(true)

  useEffect(() => {
    setTimeout(() => {
      setStats([
        { label: 'Total Bookings', value: 12, icon: <FileIcon />, color: '#3b82f6' },
        { label: 'Active Trips', value: 3, icon: <PlaneIcon />, color: '#22c55e' },
        { label: 'Applications', value: 5, icon: <ShieldIcon />, color: '#a855f7' },
        { label: 'Total Spent', value: '$2,450', icon: <MoneyIcon />, color: '#f59e0b' },
      ])
      setIsLoading(false)
    }, 500)
  }, [])

  const recentActivities = [
    { title: 'Flight to Dubai confirmed', time: '2 hours ago', icon: <PlaneIcon />, status: 'confirmed' },
    { title: 'Visa application under review', time: '1 day ago', icon: <ShieldIcon />, status: 'pending' },
    { title: 'Hotel reservation completed', time: '2 days ago', icon: <HomeIcon />, status: 'confirmed' },
    { title: 'Travel insurance purchased', time: '3 days ago', icon: <ShieldIcon />, status: 'confirmed' },
  ]

  const quickActions = [
    { title: 'Book Flight', desc: 'Search & book flights', icon: <PlaneIcon />, link: '/tickets' },
    { title: 'Rent Vehicle', desc: 'Cars, SUVs & more', icon: <CarIcon />, link: '/vehicles' },
    { title: 'Visa Application', desc: 'Apply for visa', icon: <ShieldIcon />, link: '/immigration' },
    { title: 'Hotel Booking', desc: 'Find best hotels', icon: <HomeIcon />, link: '/hotels' },
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
            <button className="btn btn-outline"><SettingsIcon /> Settings</button>
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
              {recentActivities.map((activity, index) => (
                <div key={index} className="activity-item">
                  <span className="activity-icon">{activity.icon}</span>
                  <div className="activity-content">
                    <h4>{activity.title}</h4>
                    <p>{activity.time}</p>
                  </div>
                  <span className={`activity-status status-${activity.status}`}>
                    {activity.status}
                  </span>
                </div>
              ))}
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

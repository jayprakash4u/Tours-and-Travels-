import { Link, useNavigate } from 'react-router-dom'
import { useAuthStore } from '@store/authStore'
import { Button } from './Button'
import { useState } from 'react'
import { MenuIcon } from '../icons'

// Generate avatar from name
const getAvatar = (name) => {
  if (!name) return null
  const colors = ['#0d9488', '#7c3aed', '#dc2626', '#16a34a', '#d97706', '#2563eb']
  const initials = name.split(' ').map(n => n[0] || '').join('').toUpperCase().slice(0, 2)
  const colorIndex = name.length % colors.length
  return `data:image/svg+xml,${encodeURIComponent(`<svg xmlns="http://www.w3.org/2000/svg" width="32" height="32" viewBox="0 0 32 32"><rect width="32" height="32" fill="${colors[colorIndex]}" rx="16"/><text x="50%" y="50%" dominant-baseline="central" text-anchor="middle" fill="white" font-family="Arial" font-size="11" font-weight="bold">${initials}</text></svg>`)}`
}

export function Navbar() {
  const navigate = useNavigate()
  const { user, logout, token } = useAuthStore()
  const [isOpen, setIsOpen] = useState(false)

  const handleLogout = () => {
    logout()
    navigate('/login')
  }

  const navLinks = [
    { path: '/dashboard', label: 'Dashboard' },
    { path: '/tickets', label: 'Tickets' },
    { path: '/hotels', label: 'Hotels' },
    { path: '/vehicles', label: 'Vehicles' },
    { path: '/immigration', label: 'Immigration' },
  ]

  // Don't render navbar if not logged in
  if (!token || !user) {
    return null
  }

  return (
    <nav className="navbar">
      <div className="navbar-container">
        <div className="navbar-content">
          <div className="flex items-center gap-8">
            <h1 className="navbar-brand">AlfaTravels</h1>
            <div className="navbar-links md:flex">
              {navLinks.map((link) => (
                <Link
                  key={link.path}
                  to={link.path}
                  className="navbar-link"
                >
                  {link.label}
                </Link>
              ))}
            </div>
          </div>

          <div className="navbar-user md:flex">
            <div className="navbar-user-info">
              <img 
                src={user.profilePicture || getAvatar(user.fullName)} 
                alt="Profile" 
                className="navbar-avatar"
              />
              <span className="navbar-user-name">{user.fullName}</span>
            </div>
            <Button
              size="sm"
              variant="outline"
              onClick={handleLogout}
              className="btn-outline-white"
            >
              Logout
            </Button>
          </div>

          <button
            className="navbar-mobile-toggle md:hidden"
            onClick={() => setIsOpen(!isOpen)}
          >
            <MenuIcon />
          </button>
        </div>

        {isOpen && (
          <div className="navbar-mobile-menu md:hidden">
            {navLinks.map((link) => (
              <Link
                key={link.path}
                to={link.path}
                className="navbar-mobile-link"
              >
                {link.label}
              </Link>
            ))}
            <button
              onClick={handleLogout}
              className="navbar-mobile-link w-full text-left flex items-center gap-2"
            >
              Logout
            </button>
          </div>
        )}
      </div>
    </nav>
  )
}

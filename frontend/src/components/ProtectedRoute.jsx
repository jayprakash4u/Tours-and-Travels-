import { useAuthStore } from '@store/authStore'
import { Navigate } from 'react-router-dom'

export function ProtectedRoute({ children }) {
  const { token } = useAuthStore()

  // Debug logging
  console.log('ProtectedRoute check - token:', token ? 'exists' : 'null')

  // Check if user has a valid token
  if (!token) {
    console.log('Redirecting to login - no token')
    return <Navigate to="/login" replace />
  }

  return <>{children}</>
}

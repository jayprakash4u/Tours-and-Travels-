import { useEffect, useState } from 'react'
import { Routes, Route, Navigate } from 'react-router-dom'
import { useAuthStore } from '@store/authStore'
import { NotificationContainer, ProtectedRoute } from '@components/index'
import { LoginPage, RegisterPage, DashboardPage, LandingPage, TicketsPage, VehiclesPage, ImmigrationPage, HotelsPage, LaborApprovalPage } from '@pages/index'

function App() {
  const { hydrate, token } = useAuthStore()
  const [isReady, setIsReady] = useState(false)

  useEffect(() => {
    // Restore authentication state from localStorage on app load
    hydrate()
    // Give time for state to restore
    const timer = setTimeout(() => {
      setIsReady(true)
    }, 50)
    return () => clearTimeout(timer)
  }, [hydrate])

  // Show loading while hydrating
  if (!isReady) {
    return (
      <div style={{ 
        display: 'flex', 
        justifyContent: 'center', 
        alignItems: 'center', 
        height: '100vh',
        background: '#f9fafb'
      }}>
        <div style={{ textAlign: 'center' }}>
          <div style={{ fontSize: '2rem', marginBottom: '1rem' }}>✈️</div>
          <p>Loading AlfaTravels...</p>
        </div>
      </div>
    )
  }

  return (
    <>
      <NotificationContainer />
      <Routes>
        {/* Public Routes */}
        <Route path="/" element={<LandingPage />} />
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
        
        {/* Protected Routes */}
        <Route
          path="/dashboard"
          element={
            <ProtectedRoute>
              <DashboardPage />
            </ProtectedRoute>
          }
        />
        
        <Route
          path="/tickets"
          element={
            <ProtectedRoute>
              <TicketsPage />
            </ProtectedRoute>
          }
        />
        
        <Route
          path="/vehicles"
          element={
            <ProtectedRoute>
              <VehiclesPage />
            </ProtectedRoute>
          }
        />
        
        <Route
          path="/immigration"
          element={
            <ProtectedRoute>
              <ImmigrationPage />
            </ProtectedRoute>
          }
        />
        
        <Route
          path="/hotels"
          element={
            <ProtectedRoute>
              <HotelsPage />
            </ProtectedRoute>
          }
        />
        
        <Route
          path="/labor-approval"
          element={
            <ProtectedRoute>
              <LaborApprovalPage />
            </ProtectedRoute>
          }
        />

        {/* Catch-all route */}
        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </>
  )
}

export default App

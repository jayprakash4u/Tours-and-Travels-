import { useState, useEffect } from 'react'
import { Navbar } from '@components/Navbar'
import { Card } from '@components/Card'
import { Button } from '@components/Button'
import { LoadingSpinner } from '@components/LoadingSpinner'
import { immigrationService } from '@services/immigrationService'
import { useAuthStore } from '@store/authStore'
import { ShieldIcon, PassportIcon, SupportIcon, DocumentIcon } from '../icons'

export function ImmigrationPage() {
  const [applications, setApplications] = useState([])
  const [isLoading, setIsLoading] = useState(true)
  const [error, setError] = useState(null)
  const { user } = useAuthStore()

  useEffect(() => {
    loadApplications()
  }, [])

  const loadApplications = async () => {
    try {
      setIsLoading(true)
      const data = await immigrationService.getMyApplications(user?.id)
      setApplications(data || [])
    } catch (err) {
      setError(err.message)
      console.error('Error loading applications:', err)
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
        <Card title="My Immigration Applications">
          {applications.length === 0 ? (
            <div className="empty-state">
              <span className="empty-icon"><ShieldIcon /></span>
              <h3>No Applications Yet</h3>
              <p>You haven't submitted any immigration applications yet.</p>
              <Button variant="primary">Start Application</Button>
            </div>
          ) : (
            <div className="applications-list">
              {applications.map((app) => (
                <div key={app.id} className="application-item">
                  <div className="application-info">
                    <h4>{app.visaType}</h4>
                    <p>Applied on: {new Date(app.applicationDate).toLocaleDateString()}</p>
                    <p className="application-status">
                      Status: <span className={`status-badge status-${app.status?.toLowerCase()}`}>
                        {app.status}
                      </span>
                    </p>
                  </div>
                </div>
              ))}
            </div>
          )}
        </Card>

        <Card title="Visa & Immigration Services">
          <div className="visa-types">
            <div className="visa-type">
              <span className="type-icon"><PassportIcon /></span>
              <h4>Tourist Visa</h4>
              <p>For leisure travel and tourism</p>
              <span className="processing">5-7 days processing</span>
              <Button variant="outline" size="sm">Apply Now</Button>
            </div>
            <div className="visa-type">
              <span className="type-icon"><SupportIcon /></span>
              <h4>Work Visa</h4>
              <p>For employment opportunities abroad</p>
              <span className="processing">15-30 days processing</span>
              <Button variant="outline" size="sm">Apply Now</Button>
            </div>
            <div className="visa-type">
              <span className="type-icon"><DocumentIcon /></span>
              <h4>Student Visa</h4>
              <p>For international students</p>
              <span className="processing">10-20 days processing</span>
              <Button variant="outline" size="sm">Apply Now</Button>
            </div>
            <div className="visa-type">
              <span className="type-icon"><ShieldIcon /></span>
              <h4>Residence Permit</h4>
              <p>Long-term stay permits</p>
              <span className="processing">30-60 days processing</span>
              <Button variant="outline" size="sm">Apply Now</Button>
            </div>
          </div>
        </Card>
      </div>
    </>
  )
}

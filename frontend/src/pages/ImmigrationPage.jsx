import { useState, useEffect } from 'react'
import { Navbar } from '@components/Navbar'
import { Card } from '@components/Card'
import { Button } from '@components/Button'
import { LoadingSpinner } from '@components/LoadingSpinner'
import { ShieldIcon, PassportIcon, DocumentIcon } from '../icons'
import { immigrationService } from '@services/immigrationService'
import { useNotification } from '@hooks/useNotification'

export function ImmigrationPage() {
  const [applications, setApplications] = useState([])
  const [isLoading, setIsLoading] = useState(true)
  const [showApplicationForm, setShowApplicationForm] = useState(false)
  const [formData, setFormData] = useState({
    fullName: '',
    passportNumber: '',
    targetCountry: '',
    passportExpiryDate: '',
    visaType: 'tourist'
  })
  const [isSubmitting, setIsSubmitting] = useState(false)
  const { success: showSuccess, error: showError } = useNotification()
  
  const user = localStorage.getItem('user') ? JSON.parse(localStorage.getItem('user')) : null
  const userId = user?.id

  useEffect(() => {
    loadApplications()
  }, [])

  const loadApplications = async () => {
    try {
      setIsLoading(true)
      if (!userId) {
        showError('User not authenticated')
        return
      }
      const response = await immigrationService.getUserApplications(userId)
      if (response && response.data) {
        setApplications(Array.isArray(response.data) ? response.data : [])
      } else {
        setApplications([])
      }
    } catch (err) {
      console.error('Error loading applications:', err)
      showError(err.response?.data?.message || err.message || 'Error loading applications')
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
      const response = await immigrationService.createApplication(userId, formData)
      if (response && response.success) {
        showSuccess('Application submitted successfully!')
        setFormData({
          fullName: '',
          passportNumber: '',
          targetCountry: '',
          passportExpiryDate: '',
          visaType: 'tourist'
        })
        setShowApplicationForm(false)
        loadApplications() // Reload applications
      } else {
        showError(response?.message || 'Failed to submit application')
      }
    } catch (err) {
      showError(err.response?.data?.message || err.message || 'Error submitting application')
      console.error('Error submitting application:', err)
    } finally {
      setIsSubmitting(false)
    }
  }

  const handleInputChange = (e) => {
    const { name, value } = e.target
    setFormData(prev => ({
      ...prev,
      [name]: value
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
        {/* Service Cards */}
        <div className="services-grid" style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(280px, 1fr))', gap: '1rem', marginBottom: '2rem' }}>
          <div className="service-card" onClick={() => setShowApplicationForm(true)} style={{ padding: '1.5rem', borderRadius: '12px', background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)', color: 'white', cursor: 'pointer', transition: 'transform 0.2s' }}>
            <ShieldIcon />
            <h3>Visa Application</h3>
            <p>Apply for tourist, business, or work visa</p>
          </div>
          <a href="/labor-approval" className="service-card" style={{ padding: '1.5rem', borderRadius: '12px', background: 'linear-gradient(135deg, #11998e 0%, #38ef7d 100%)', color: 'white', textDecoration: 'none', display: 'block', transition: 'transform 0.2s' }}>
            <ShieldIcon />
            <h3>Labor Approval</h3>
            <p>Shram Swikriti - Nepal Foreign Employment</p>
          </a>
        </div>

        <Card title="My Immigration Applications">
          {applications.length === 0 ? (
            <div className="empty-state">
              <span className="empty-icon"><ShieldIcon /></span>
              <h3>No Applications Yet</h3>
              <p>You haven't submitted any immigration applications yet.</p>
              <Button 
                variant="primary"
                onClick={() => setShowApplicationForm(true)}
              >
                Start Application
              </Button>
            </div>
          ) : (
            <div className="applications-list">
              {applications.map((app) => (
                <div key={app.id} className="application-item">
                  <div className="application-info">
                    <h4>{app.visaType || 'Visa Application'}</h4>
                    <p>Full Name: {app.fullName}</p>
                    <p>Target Country: {app.targetCountry}</p>
                    <p>Applied on: {new Date(app.createdDate).toLocaleDateString()}</p>
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

        {showApplicationForm ? (
          <Card title="New Immigration Application">
            <form onSubmit={handleSubmit} className="application-form">
              <div className="form-group">
                <label htmlFor="fullName">Full Name:</label>
                <input
                  type="text"
                  id="fullName"
                  name="fullName"
                  value={formData.fullName}
                  onChange={handleInputChange}
                  placeholder="Your full name"
                  required
                />
              </div>
              <div className="form-group">
                <label htmlFor="passportNumber">Passport Number:</label>
                <input
                  type="text"
                  id="passportNumber"
                  name="passportNumber"
                  value={formData.passportNumber}
                  onChange={handleInputChange}
                  placeholder="Your passport number"
                  required
                />
              </div>
              <div className="form-group">
                <label htmlFor="passportExpiryDate">Passport Expiry Date:</label>
                <input
                  type="date"
                  id="passportExpiryDate"
                  name="passportExpiryDate"
                  value={formData.passportExpiryDate}
                  onChange={handleInputChange}
                  required
                />
              </div>
              <div className="form-group">
                <label htmlFor="targetCountry">Target Country:</label>
                <input
                  type="text"
                  id="targetCountry"
                  name="targetCountry"
                  value={formData.targetCountry}
                  onChange={handleInputChange}
                  placeholder="Country you want to visit"
                  required
                />
              </div>
              <div className="form-group">
                <label htmlFor="visaType">Visa Type:</label>
                <select
                  id="visaType"
                  name="visaType"
                  value={formData.visaType}
                  onChange={handleInputChange}
                >
                  <option value="tourist">Tourist Visa</option>
                  <option value="work">Work Visa</option>
                  <option value="student">Student Visa</option>
                  <option value="residence">Residence Permit</option>
                </select>
              </div>
              <div className="form-actions">
                <Button 
                  variant="primary" 
                  type="submit"
                  isLoading={isSubmitting}
                >
                  Submit Application
                </Button>
                <Button 
                  variant="outline" 
                  type="button"
                  onClick={() => setShowApplicationForm(false)}
                >
                  Cancel
                </Button>
              </div>
            </form>
          </Card>
        ) : (
          <Card title="Visa & Immigration Services">
            <div className="visa-types">
              <div className="visa-type">
                <span className="type-icon"><PassportIcon /></span>
                <h4>Tourist Visa</h4>
                <p>For leisure travel and tourism</p>
                <span className="processing">5-7 days processing</span>
                <Button 
                  variant="outline" 
                  size="sm"
                  onClick={() => {
                    setFormData(prev => ({ ...prev, visaType: 'tourist' }))
                    setShowApplicationForm(true)
                  }}
                >
                  Apply Now
                </Button>
              </div>
              <div className="visa-type">
                <span className="type-icon"><DocumentIcon /></span>
                <h4>Work Visa</h4>
                <p>For employment opportunities abroad</p>
                <span className="processing">15-30 days processing</span>
                <Button 
                  variant="outline" 
                  size="sm"
                  onClick={() => {
                    setFormData(prev => ({ ...prev, visaType: 'work' }))
                    setShowApplicationForm(true)
                  }}
                >
                  Apply Now
                </Button>
              </div>
              <div className="visa-type">
                <span className="type-icon"><DocumentIcon /></span>
                <h4>Student Visa</h4>
                <p>For international students</p>
                <span className="processing">10-20 days processing</span>
                <Button 
                  variant="outline" 
                  size="sm"
                  onClick={() => {
                    setFormData(prev => ({ ...prev, visaType: 'student' }))
                    setShowApplicationForm(true)
                  }}
                >
                  Apply Now
                </Button>
              </div>
              <div className="visa-type">
                <span className="type-icon"><ShieldIcon /></span>
                <h4>Residence Permit</h4>
                <p>Long-term stay permits</p>
                <span className="processing">30-60 days processing</span>
                <Button 
                  variant="outline" 
                  size="sm"
                  onClick={() => {
                    setFormData(prev => ({ ...prev, visaType: 'residence' }))
                    setShowApplicationForm(true)
                  }}
                >
                  Apply Now
                </Button>
              </div>
            </div>
          </Card>
        )}
      </div>
    </>
  )
}

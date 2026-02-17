import { useState, useEffect } from 'react';
import laborApprovalService from '../services/laborApprovalService';
import './LaborApprovalPage.css';

const LaborApprovalPage = () => {
  const [view, setView] = useState('list'); // list, create, details
  const [applications, setApplications] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [calculatedFee, setCalculatedFee] = useState(null);
  const [selectedApplication, setSelectedApplication] = useState(null);

  // Form state
  const [formData, setFormData] = useState({
    fullName: '',
    passportNumber: '',
    passportExpiryDate: '',
    gender: 'Male',
    dateOfBirth: '',
    permanentAddress: '',
    currentAddress: '',
    destinationCountry: '',
    recruitingAgency: '',
    companyName: '',
    jobCategory: '',
    visaType: '',
    offeredSalary: '',
    contractDuration: '',
    hasPoliceClearance: false,
    hasMedicalCertificate: false,
    hasTrainingCertificate: false,
  });

  const countries = [
    'Malaysia', 'Qatar', 'UAE', 'Saudi Arabia', 'Kuwait', 
    'Bahrain', 'Oman', 'Iran', 'Iraq', 'Libya', 'Afghanistan'
  ];

  const jobCategories = [
    'Manager', 'Engineer', 'Doctor', 'Technician', 
    'Skilled Worker', 'Semi-Skilled', 'Unskilled'
  ];

  useEffect(() => {
    loadApplications();
  }, []);

  const loadApplications = async () => {
    setLoading(true);
    try {
      const data = await laborApprovalService.getMyApplications();
      setApplications(data);
    } catch (err) {
      setError('Failed to load applications');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleInputChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData({
      ...formData,
      [name]: type === 'checkbox' ? checked : value,
    });
  };

  const handleCalculateFee = async () => {
    if (!formData.destinationCountry || !formData.jobCategory || !formData.offeredSalary) {
      setError('Please fill in destination country, job category, and offered salary');
      return;
    }
    try {
      const fee = await laborApprovalService.calculateFee(
        formData.destinationCountry,
        formData.jobCategory,
        parseFloat(formData.offeredSalary)
      );
      setCalculatedFee(fee);
    } catch (err) {
      setError('Failed to calculate fee');
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError('');
    try {
      const submitData = {
        ...formData,
        offeredSalary: parseFloat(formData.offeredSalary),
        contractDuration: parseInt(formData.contractDuration),
      };
      await laborApprovalService.createApplication(submitData);
      setView('list');
      loadApplications();
      alert('Application submitted successfully!');
    } catch (err) {
      setError(err.response?.data?.message || 'Failed to submit application');
    } finally {
      setLoading(false);
    }
  };

  const handleViewDetails = (application) => {
    setSelectedApplication(application);
    setView('details');
  };

  const getStatusColor = (status) => {
    const colors = {
      'Pending': '#ffc107',
      'UnderReview': '#17a2b8',
      'DocumentsVerified': '#6f42c1',
      'FeePending': '#fd7e14',
      'FeePaid': '#28a745',
      'Approved': '#20c997',
      'Rejected': '#dc3545',
      'Completed': '#28a745',
    };
    return colors[status] || '#6c757d';
  };

  const getStatusLabel = (status) => {
    const labels = {
      'Pending': 'Pending Review',
      'UnderReview': 'Under Review',
      'DocumentsVerified': 'Documents Verified',
      'FeePending': 'Awaiting Payment',
      'FeePaid': 'Payment Received',
      'Approved': 'Approved',
      'Rejected': 'Rejected',
      'Completed': 'Completed',
    };
    return labels[status] || status;
  };

  return (
    <div className="labor-approval-page">
      <div className="page-header">
        <h1>🏢 Labor Approval (Shram Swikriti)</h1>
        <p>Nepal Foreign Employment Service - Apply for Labor Permission</p>
      </div>

      {error && <div className="error-message">{error}</div>}

      {view === 'list' && (
        <div className="applications-list">
          <div className="section-header">
            <h2>My Applications</h2>
            <button className="btn-primary" onClick={() => setView('create')}>
              + New Application
            </button>
          </div>

          {loading ? (
            <div className="loading">Loading applications...</div>
          ) : applications.length === 0 ? (
            <div className="empty-state">
              <p>No applications yet. Click "New Application" to apply for labor approval.</p>
            </div>
          ) : (
            <div className="applications-grid">
              {applications.map((app) => (
                <div key={app.id} className="application-card">
                  <div className="card-header">
                    <h3>{app.fullName}</h3>
                    <span 
                      className="status-badge" 
                      style={{ backgroundColor: getStatusColor(app.status) }}
                    >
                      {getStatusLabel(app.status)}
                    </span>
                  </div>
                  <div className="card-body">
                    <p><strong>Passport:</strong> {app.passportNumber}</p>
                    <p><strong>Destination:</strong> {app.destinationCountry}</p>
                    <p><strong>Company:</strong> {app.companyName}</p>
                    <p><strong>Job:</strong> {app.jobCategory}</p>
                    <p><strong>Fee:</strong> NPR {app.totalFee?.toLocaleString()}</p>
                    <p><strong>Paid:</strong> {app.isFeePaid ? '✅ Yes' : '❌ No'}</p>
                    <p><strong>Date:</strong> {new Date(app.createdDate).toLocaleDateString()}</p>
                  </div>
                  <button 
                    className="btn-secondary" 
                    onClick={() => handleViewDetails(app)}
                  >
                    View Details
                  </button>
                </div>
              ))}
            </div>
          )}
        </div>
      )}

      {view === 'create' && (
        <div className="application-form">
          <div className="section-header">
            <h2>New Labor Approval Application</h2>
            <button className="btn-secondary" onClick={() => setView('list')}>
              ← Back to List
            </button>
          </div>

          <form onSubmit={handleSubmit}>
            <div className="form-section">
              <h3>📋 Personal Information</h3>
              <div className="form-grid">
                <div className="form-group">
                  <label>Full Name *</label>
                  <input
                    type="text"
                    name="fullName"
                    value={formData.fullName}
                    onChange={handleInputChange}
                    required
                  />
                </div>
                <div className="form-group">
                  <label>Gender *</label>
                  <select name="gender" value={formData.gender} onChange={handleInputChange}>
                    <option value="Male">Male</option>
                    <option value="Female">Female</option>
                    <option value="Other">Other</option>
                  </select>
                </div>
                <div className="form-group">
                  <label>Date of Birth *</label>
                  <input
                    type="date"
                    name="dateOfBirth"
                    value={formData.dateOfBirth}
                    onChange={handleInputChange}
                    required
                  />
                </div>
              </div>
            </div>

            <div className="form-section">
              <h3>🛂 Passport Details</h3>
              <div className="form-grid">
                <div className="form-group">
                  <label>Passport Number *</label>
                  <input
                    type="text"
                    name="passportNumber"
                    value={formData.passportNumber}
                    onChange={handleInputChange}
                    required
                  />
                </div>
                <div className="form-group">
                  <label>Passport Expiry Date *</label>
                  <input
                    type="date"
                    name="passportExpiryDate"
                    value={formData.passportExpiryDate}
                    onChange={handleInputChange}
                    required
                  />
                </div>
              </div>
            </div>

            <div className="form-section">
              <h3>📍 Address Information</h3>
              <div className="form-grid">
                <div className="form-group">
                  <label>Permanent Address *</label>
                  <input
                    type="text"
                    name="permanentAddress"
                    value={formData.permanentAddress}
                    onChange={handleInputChange}
                    required
                  />
                </div>
                <div className="form-group">
                  <label>Current Address</label>
                  <input
                    type="text"
                    name="currentAddress"
                    value={formData.currentAddress}
                    onChange={handleInputChange}
                  />
                </div>
              </div>
            </div>

            <div className="form-section">
              <h3>🌍 Employment Details</h3>
              <div className="form-grid">
                <div className="form-group">
                  <label>Destination Country *</label>
                  <select
                    name="destinationCountry"
                    value={formData.destinationCountry}
                    onChange={handleInputChange}
                    required
                  >
                    <option value="">Select Country</option>
                    {countries.map((country) => (
                      <option key={country} value={country}>{country}</option>
                    ))}
                  </select>
                </div>
                <div className="form-group">
                  <label>Job Category *</label>
                  <select
                    name="jobCategory"
                    value={formData.jobCategory}
                    onChange={handleInputChange}
                    required
                  >
                    <option value="">Select Job Category</option>
                    {jobCategories.map((job) => (
                      <option key={job} value={job}>{job}</option>
                    ))}
                  </select>
                </div>
                <div className="form-group">
                  <label>Recruiting Agency</label>
                  <input
                    type="text"
                    name="recruitingAgency"
                    value={formData.recruitingAgency}
                    onChange={handleInputChange}
                  />
                </div>
                <div className="form-group">
                  <label>Company Name *</label>
                  <input
                    type="text"
                    name="companyName"
                    value={formData.companyName}
                    onChange={handleInputChange}
                    required
                  />
                </div>
                <div className="form-group">
                  <label>Visa Type</label>
                  <input
                    type="text"
                    name="visaType"
                    value={formData.visaType}
                    onChange={handleInputChange}
                    placeholder="e.g., Employment Visa"
                  />
                </div>
                <div className="form-group">
                  <label>Offered Salary (NPR) *</label>
                  <input
                    type="number"
                    name="offeredSalary"
                    value={formData.offeredSalary}
                    onChange={handleInputChange}
                    required
                  />
                </div>
                <div className="form-group">
                  <label>Contract Duration (Months)</label>
                  <input
                    type="number"
                    name="contractDuration"
                    value={formData.contractDuration}
                    onChange={handleInputChange}
                  />
                </div>
              </div>
            </div>

            <div className="form-section">
              <h3>📄 Required Documents</h3>
              <div className="checkbox-group">
                <label className="checkbox-label">
                  <input
                    type="checkbox"
                    name="hasPoliceClearance"
                    checked={formData.hasPoliceClearance}
                    onChange={handleInputChange}
                  />
                  Police Clearance Certificate
                </label>
                <label className="checkbox-label">
                  <input
                    type="checkbox"
                    name="hasMedicalCertificate"
                    checked={formData.hasMedicalCertificate}
                    onChange={handleInputChange}
                  />
                  Medical Certificate
                </label>
                <label className="checkbox-label">
                  <input
                    type="checkbox"
                    name="hasTrainingCertificate"
                    checked={formData.hasTrainingCertificate}
                    onChange={handleInputChange}
                  />
                  Training Certificate
                </label>
              </div>
            </div>

            <div className="fee-calculation">
              <button 
                type="button" 
                className="btn-secondary"
                onClick={handleCalculateFee}
              >
                Calculate Fee
              </button>
              {calculatedFee && (
                <div className="fee-details">
                  <p><strong>Application Fee:</strong> NPR {calculatedFee.applicationFee.toLocaleString()}</p>
                  <p><strong>Service Charge:</strong> NPR {calculatedFee.serviceCharge.toLocaleString()}</p>
                  <p><strong>Total Fee:</strong> NPR {calculatedFee.totalFee.toLocaleString()}</p>
                </div>
              )}
            </div>

            {error && <div className="error-message">{error}</div>}

            <div className="form-actions">
              <button type="button" className="btn-secondary" onClick={() => setView('list')}>
                Cancel
              </button>
              <button type="submit" className="btn-primary" disabled={loading}>
                {loading ? 'Submitting...' : 'Submit Application'}
              </button>
            </div>
          </form>
        </div>
      )}

      {view === 'details' && selectedApplication && (
        <div className="application-details">
          <div className="section-header">
            <h2>Application Details</h2>
            <button className="btn-secondary" onClick={() => setView('list')}>
              ← Back to List
            </button>
          </div>

          <div className="details-card">
            <div className="detail-section">
              <h3>Personal Information</h3>
              <div className="detail-grid">
                <div className="detail-item">
                  <label>Full Name:</label>
                  <span>{selectedApplication.fullName}</span>
                </div>
                <div className="detail-item">
                  <label>Gender:</label>
                  <span>{selectedApplication.gender}</span>
                </div>
                <div className="detail-item">
                  <label>Date of Birth:</label>
                  <span>{new Date(selectedApplication.dateOfBirth).toLocaleDateString()}</span>
                </div>
              </div>
            </div>

            <div className="detail-section">
              <h3>Passport Information</h3>
              <div className="detail-grid">
                <div className="detail-item">
                  <label>Passport Number:</label>
                  <span>{selectedApplication.passportNumber}</span>
                </div>
                <div className="detail-item">
                  <label>Expiry Date:</label>
                  <span>{new Date(selectedApplication.passportExpiryDate).toLocaleDateString()}</span>
                </div>
              </div>
            </div>

            <div className="detail-section">
              <h3>Address</h3>
              <div className="detail-grid">
                <div className="detail-item">
                  <label>Permanent:</label>
                  <span>{selectedApplication.permanentAddress}</span>
                </div>
                <div className="detail-item">
                  <label>Current:</label>
                  <span>{selectedApplication.currentAddress}</span>
                </div>
              </div>
            </div>

            <div className="detail-section">
              <h3>Employment Details</h3>
              <div className="detail-grid">
                <div className="detail-item">
                  <label>Destination:</label>
                  <span>{selectedApplication.destinationCountry}</span>
                </div>
                <div className="detail-item">
                  <label>Company:</label>
                  <span>{selectedApplication.companyName}</span>
                </div>
                <div className="detail-item">
                  <label>Job Category:</label>
                  <span>{selectedApplication.jobCategory}</span>
                </div>
                <div className="detail-item">
                  <label>Visa Type:</label>
                  <span>{selectedApplication.visaType}</span>
                </div>
                <div className="detail-item">
                  <label>Salary:</label>
                  <span>NPR {selectedApplication.offeredSalary?.toLocaleString()}</span>
                </div>
                <div className="detail-item">
                  <label>Contract:</label>
                  <span>{selectedApplication.contractDuration} months</span>
                </div>
              </div>
            </div>

            <div className="detail-section">
              <h3>Documents</h3>
              <div className="detail-grid">
                <div className="detail-item">
                  <label>Police Clearance:</label>
                  <span>{selectedApplication.hasPoliceClearance ? '✅ Yes' : '❌ No'}</span>
                </div>
                <div className="detail-item">
                  <label>Medical Certificate:</label>
                  <span>{selectedApplication.hasMedicalCertificate ? '✅ Yes' : '❌ No'}</span>
                </div>
                <div className="detail-item">
                  <label>Training Certificate:</label>
                  <span>{selectedApplication.hasTrainingCertificate ? '✅ Yes' : '❌ No'}</span>
                </div>
              </div>
            </div>

            <div className="detail-section">
              <h3>Payment Information</h3>
              <div className="detail-grid">
                <div className="detail-item">
                  <label>Application Fee:</label>
                  <span>NPR {selectedApplication.applicationFee?.toLocaleString()}</span>
                </div>
                <div className="detail-item">
                  <label>Service Charge:</label>
                  <span>NPR {selectedApplication.serviceCharge?.toLocaleString()}</span>
                </div>
                <div className="detail-item">
                  <label>Total Fee:</label>
                  <span>NPR {selectedApplication.totalFee?.toLocaleString()}</span>
                </div>
                <div className="detail-item">
                  <label>Payment Status:</label>
                  <span>{selectedApplication.isFeePaid ? '✅ Paid' : '❌ Pending'}</span>
                </div>
                {selectedApplication.paymentReference && (
                  <div className="detail-item">
                    <label>Payment Reference:</label>
                    <span>{selectedApplication.paymentReference}</span>
                  </div>
                )}
              </div>
            </div>

            <div className="detail-section">
              <h3>Application Status</h3>
              <div className="status-timeline">
                <div 
                  className="status-current"
                  style={{ backgroundColor: getStatusColor(selectedApplication.status) }}
                >
                  {getStatusLabel(selectedApplication.status)}
                </div>
                <div className="status-dates">
                  <p><strong>Submitted:</strong> {new Date(selectedApplication.createdDate).toLocaleString()}</p>
                  {selectedApplication.approvedDate && (
                    <p><strong>Approved:</strong> {new Date(selectedApplication.approvedDate).toLocaleString()}</p>
                  )}
                  {selectedApplication.completedDate && (
                    <p><strong>Completed:</strong> {new Date(selectedApplication.completedDate).toLocaleString()}</p>
                  )}
                </div>
                {selectedApplication.adminNotes && (
                  <div className="admin-notes">
                    <label>Admin Notes:</label>
                    <p>{selectedApplication.adminNotes}</p>
                  </div>
                )}
                {selectedApplication.rejectionReason && (
                  <div className="rejection-reason">
                    <label>Rejection Reason:</label>
                    <p>{selectedApplication.rejectionReason}</p>
                  </div>
                )}
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default LaborApprovalPage;

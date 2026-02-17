import axios from "axios";

const API_URL = "https://localhost:7001/api/laborapprovals";

const getAuthHeader = () => {
  const token = localStorage.getItem("token");
  return {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  };
};

// Get all applications (Admin only)
export const getAllApplications = async () => {
  const response = await axios.get(API_URL, getAuthHeader());
  return response.data;
};

// Get current user's applications
export const getMyApplications = async () => {
  const response = await axios.get(
    `${API_URL}/my-applications`,
    getAuthHeader(),
  );
  return response.data;
};

// Get single application by ID
export const getApplicationById = async (id) => {
  const response = await axios.get(`${API_URL}/${id}`, getAuthHeader());
  return response.data;
};

// Create new application
export const createApplication = async (applicationData) => {
  const response = await axios.post(API_URL, applicationData, getAuthHeader());
  return response.data;
};

// Update application status (Admin only)
export const updateApplicationStatus = async (id, statusData) => {
  const response = await axios.put(
    `${API_URL}/${id}/status`,
    statusData,
    getAuthHeader(),
  );
  return response.data;
};

// Process payment
export const processPayment = async (id, paymentData) => {
  const response = await axios.post(
    `${API_URL}/${id}/payment`,
    paymentData,
    getAuthHeader(),
  );
  return response.data;
};

// Calculate fee
export const calculateFee = async (country, jobCategory, salary) => {
  const response = await axios.get(
    `${API_URL}/calculate-fee?country=${encodeURIComponent(country)}&jobCategory=${encodeURIComponent(jobCategory)}&salary=${salary}`,
    getAuthHeader(),
  );
  return response.data;
};

// Delete application (Admin only)
export const deleteApplication = async (id) => {
  const response = await axios.delete(`${API_URL}/${id}`, getAuthHeader());
  return response.data;
};

export default {
  getAllApplications,
  getMyApplications,
  getApplicationById,
  createApplication,
  updateApplicationStatus,
  processPayment,
  calculateFee,
  deleteApplication,
};

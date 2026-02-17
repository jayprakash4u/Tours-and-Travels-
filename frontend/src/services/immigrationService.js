import axios from "axios";

const API_URL = "/api/immigration";

// Create axios instance with interceptors
const apiClient = axios.create({
  baseURL: "",
  headers: {
    "Content-Type": "application/json",
  },
});

// Add request interceptor to inject token
apiClient.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem("token");
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  },
);

// Add response interceptor to handle 401 errors
apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem("token");
      localStorage.removeItem("user");
      window.location.href = "/login";
    }
    return Promise.reject(error);
  },
);

export const immigrationService = {
  // Get all user applications
  getUserApplications: async (userId) => {
    try {
      const response = await apiClient.get(`${API_URL}/user/${userId}`);
      return response.data;
    } catch (error) {
      throw error;
    }
  },

  // Get application by ID
  getApplicationById: async (id) => {
    try {
      const response = await apiClient.get(`${API_URL}/${id}`);
      return response.data;
    } catch (error) {
      throw error;
    }
  },

  // Create new application
  createApplication: async (userId, applicationData) => {
    try {
      const response = await apiClient.post(
        `${API_URL}?userId=${userId}`,
        applicationData,
      );
      return response.data;
    } catch (error) {
      throw error;
    }
  },

  // Update application
  updateApplication: async (id, applicationData) => {
    try {
      const response = await apiClient.put(`${API_URL}/${id}`, applicationData);
      return response.data;
    } catch (error) {
      throw error;
    }
  },

  // Delete application
  deleteApplication: async (id) => {
    try {
      const response = await apiClient.delete(`${API_URL}/${id}`);
      return response.data;
    } catch (error) {
      throw error;
    }
  },

  // Update application status
  updateApplicationStatus: async (id, statusData) => {
    try {
      const response = await apiClient.put(
        `${API_URL}/${id}/status`,
        statusData,
      );
      return response.data;
    } catch (error) {
      throw error;
    }
  },
};

export default immigrationService;

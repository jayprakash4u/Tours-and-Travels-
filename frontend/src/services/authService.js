import axios from "axios";

const API_URL = "/api/auth";

// Create axios instance
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
      // Token expired or invalid
      localStorage.removeItem("token");
      localStorage.removeItem("user");
      window.location.href = "/login";
    }
    return Promise.reject(error);
  },
);

export const authService = {
  login: async (credentials) => {
    const response = await apiClient.post(`${API_URL}/login`, credentials);
    return response.data;
  },

  register: async (userData) => {
    const response = await apiClient.post(`${API_URL}/register`, userData);
    return response.data;
  },

  logout: async () => {
    localStorage.removeItem("token");
    localStorage.removeItem("user");
  },

  changePassword: async (currentPassword, newPassword) => {
    const response = await apiClient.post(`${API_URL}/change-password`, {
      currentPassword,
      newPassword,
    });
    return response.data;
  },

  updateProfile: async (userId, profileData) => {
    const response = await apiClient.put(`/api/users/${userId}`, profileData);
    return response.data;
  },

  getUserProfile: async (userId) => {
    const response = await apiClient.get(`/api/users/${userId}`);
    return response.data;
  },
};

export default authService;

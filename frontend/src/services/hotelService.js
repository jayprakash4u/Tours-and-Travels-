import axios from "axios";

const API_URL = "/api/hotels";

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
      // Token expired or invalid
      localStorage.removeItem("token");
      localStorage.removeItem("user");
      window.location.href = "/login";
    }
    return Promise.reject(error);
  },
);

export const hotelService = {
  // Get all user bookings
  getUserBookings: async (userId) => {
    try {
      const response = await apiClient.get(`${API_URL}/user/${userId}`);
      return response.data;
    } catch (error) {
      throw error;
    }
  },

  // Get booking by ID
  getBookingById: async (id) => {
    try {
      const response = await apiClient.get(`${API_URL}/${id}`);
      return response.data;
    } catch (error) {
      throw error;
    }
  },

  // Create new booking
  createBooking: async (userId, bookingData) => {
    try {
      const response = await apiClient.post(
        `${API_URL}?userId=${userId}`,
        bookingData,
      );
      return response.data;
    } catch (error) {
      throw error;
    }
  },

  // Update booking
  updateBooking: async (id, bookingData) => {
    try {
      const response = await apiClient.put(`${API_URL}/${id}`, bookingData);
      return response.data;
    } catch (error) {
      throw error;
    }
  },

  // Delete booking
  deleteBooking: async (id) => {
    try {
      const response = await apiClient.delete(`${API_URL}/${id}`);
      return response.data;
    } catch (error) {
      throw error;
    }
  },

  // Update booking status
  updateBookingStatus: async (id, statusData) => {
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

  // Search hotels
  searchHotels: async (location, checkInDate, checkOutDate, guests) => {
    try {
      const response = await apiClient.get(
        `${API_URL}/search?location=${location}&checkIn=${checkInDate}&checkOut=${checkOutDate}&guests=${guests}`,
      );
      return response.data;
    } catch (error) {
      throw error;
    }
  },
};

export default hotelService;

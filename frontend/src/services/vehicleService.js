import apiClient from "./apiClient";

export const vehicleService = {
  getAllBookings: async () => {
    const response = await apiClient.get("/vehicles");
    return response.data;
  },

  getBookingById: async (id) => {
    const response = await apiClient.get(`/vehicles/${id}`);
    return response.data;
  },

  getMyBookings: async (userId) => {
    const response = await apiClient.get(`/vehicles/user/${userId}`);
    return response.data;
  },

  createBooking: async (data) => {
    const response = await apiClient.post("/vehicles", data);
    return response.data;
  },

  updateBooking: async (id, data) => {
    const response = await apiClient.put(`/vehicles/${id}`, data);
    return response.data;
  },

  updateBookingStatus: async (id, status) => {
    const response = await apiClient.put(`/vehicles/${id}/status`, { status });
    return response.data;
  },

  deleteBooking: async (id) => {
    const response = await apiClient.delete(`/vehicles/${id}`);
    return response.data;
  },

  getAvailableVehicles: async (pickupDate, dropDate) => {
    const response = await apiClient.get("/vehicles/available", {
      params: { pickupDate, dropDate },
    });
    return response.data;
  },

  getBookingsByStatus: async (status) => {
    const response = await apiClient.get(`/vehicles/status/${status}`);
    return response.data;
  },
};

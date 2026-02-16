import apiClient from "./apiClient";

export const userService = {
  getAllUsers: async () => {
    const response = await apiClient.get("/users");
    return response.data;
  },

  getUserById: async (id) => {
    const response = await apiClient.get(`/users/${id}`);
    return response.data;
  },

  getUserProfile: async () => {
    const response = await apiClient.get("/users/profile");
    return response.data;
  },

  updateUser: async (id, data) => {
    const response = await apiClient.put(`/users/${id}`, data);
    return response.data;
  },

  deleteUser: async (id) => {
    const response = await apiClient.delete(`/users/${id}`);
    return response.data;
  },
};

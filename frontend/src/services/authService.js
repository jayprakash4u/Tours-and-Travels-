import apiClient from "./apiClient";

export const authService = {
  register: async (data) => {
    const response = await apiClient.post("/auth/register", data);
    return response.data;
  },

  login: async (credentials) => {
    const response = await apiClient.post("/auth/login", credentials);
    return response.data;
  },

  changePassword: async (currentPassword, newPassword) => {
    const response = await apiClient.post("/auth/change-password", {
      currentPassword,
      newPassword,
    });
    return response.data;
  },
};

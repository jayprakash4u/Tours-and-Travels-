import apiClient from "./apiClient";

export const immigrationService = {
  getAllApplications: async () => {
    const response = await apiClient.get("/immigration");
    return response.data;
  },

  getApplicationById: async (id) => {
    const response = await apiClient.get(`/immigration/${id}`);
    return response.data;
  },

  getMyApplications: async (userId) => {
    const response = await apiClient.get(`/immigration/user/${userId}`);
    return response.data;
  },

  createApplication: async (data) => {
    const response = await apiClient.post("/immigration", data);
    return response.data;
  },

  updateApplication: async (id, data) => {
    const response = await apiClient.put(`/immigration/${id}`, data);
    return response.data;
  },

  updateApplicationStatus: async (id, status) => {
    const response = await apiClient.put(`/immigration/${id}/status`, {
      status,
    });
    return response.data;
  },

  deleteApplication: async (id) => {
    const response = await apiClient.delete(`/immigration/${id}`);
    return response.data;
  },

  uploadDocuments: async (id, files) => {
    const formData = new FormData();
    files.forEach((file) => formData.append("files", file));
    const response = await apiClient.post(
      `/immigration/${id}/documents`,
      formData,
      {
        headers: { "Content-Type": "multipart/form-data" },
      },
    );
    return response.data;
  },

  getApplicationsByStatus: async (status) => {
    const response = await apiClient.get(`/immigration/status/${status}`);
    return response.data;
  },

  getApplicationsByCountry: async (country) => {
    const response = await apiClient.get(`/immigration/country/${country}`);
    return response.data;
  },
};

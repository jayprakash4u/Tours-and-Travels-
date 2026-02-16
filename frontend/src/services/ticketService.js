import apiClient from "./apiClient";

export const ticketService = {
  getAllTickets: async () => {
    const response = await apiClient.get("/tickets");
    return response.data;
  },

  getTicketById: async (id) => {
    const response = await apiClient.get(`/tickets/${id}`);
    return response.data;
  },

  getMyTickets: async (userId) => {
    const response = await apiClient.get(`/tickets/user/${userId}`);
    return response.data;
  },

  createTicket: async (data) => {
    const response = await apiClient.post("/tickets", data);
    return response.data;
  },

  updateTicket: async (id, data) => {
    const response = await apiClient.put(`/tickets/${id}`, data);
    return response.data;
  },

  updateTicketStatus: async (id, status) => {
    const response = await apiClient.put(`/tickets/${id}/status`, { status });
    return response.data;
  },

  deleteTicket: async (id) => {
    const response = await apiClient.delete(`/tickets/${id}`);
    return response.data;
  },

  getTicketsByStatus: async (status) => {
    const response = await apiClient.get(`/tickets/status/${status}`);
    return response.data;
  },
};

import { create } from "zustand";
import { authService } from "@services/authService";

export const useAuthStore = create((set) => ({
  user: null,
  token: null,
  isAuthenticated: false,
  isLoading: false,
  error: null,

  login: async (email, password) => {
    try {
      set({ isLoading: true, error: null });
      const response = await authService.login({ email, password });

      if (response.success && response.data) {
        const loginData = response.data;
        set({
          token: loginData.token,
          user: {
            id: loginData.id,
            fullName: loginData.fullName,
            email: loginData.email,
            role: loginData.role || "Customer",
            profilePicture: loginData.profilePicture,
            isActive: true,
            createdDate: new Date().toISOString(),
          },
          isAuthenticated: true,
        });
        localStorage.setItem("token", loginData.token);
        localStorage.setItem("user", JSON.stringify(loginData));
      } else {
        throw new Error(response.message || "Login failed");
      }
    } catch (error) {
      const message =
        error.response?.data?.message || error.message || "Login failed";
      set({ error: message });
      throw error;
    } finally {
      set({ isLoading: false });
    }
  },

  register: async (data) => {
    try {
      set({ isLoading: true, error: null });
      const response = await authService.register(data);

      if (response.success) {
        set({ isLoading: false });
      } else {
        throw new Error(response.message || "Registration failed");
      }
    } catch (error) {
      const message =
        error.response?.data?.message || error.message || "Registration failed";
      set({ error: message, isLoading: false });
      throw error;
    }
  },

  logout: () => {
    set({
      user: null,
      token: null,
      isAuthenticated: false,
      error: null,
    });
    localStorage.removeItem("token");
    localStorage.removeItem("user");
  },

  setUser: (user) => {
    set({ user });
    if (user) {
      localStorage.setItem("user", JSON.stringify(user));
    }
  },

  setToken: (token) => {
    set({ token });
    if (token) {
      localStorage.setItem("token", token);
    }
  },

  clearError: () => set({ error: null }),

  hydrate: () => {
    const token = localStorage.getItem("token");
    const userStr = localStorage.getItem("user");

    if (token && userStr) {
      try {
        const user = JSON.parse(userStr);
        set({
          token,
          user,
          isAuthenticated: true,
        });
      } catch {
        // Invalid stored data
        localStorage.removeItem("token");
        localStorage.removeItem("user");
      }
    }
  },
}));

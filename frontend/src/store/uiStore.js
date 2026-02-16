import { create } from "zustand";

export const useUIStore = create((set) => ({
  notifications: [],

  addNotification: (notification) => {
    const id = `${Date.now()}-${Math.random()}`;
    const duration =
      notification.duration || (notification.type === "error" ? 5000 : 3000);

    set((state) => ({
      notifications: [...state.notifications, { ...notification, id }],
    }));

    if (duration > 0) {
      setTimeout(() => {
        set((state) => ({
          notifications: state.notifications.filter((n) => n.id !== id),
        }));
      }, duration);
    }
  },

  removeNotification: (id) => {
    set((state) => ({
      notifications: state.notifications.filter((n) => n.id !== id),
    }));
  },

  clearNotifications: () => {
    set({ notifications: [] });
  },
}));

import { useCallback } from "react";
import { useUIStore } from "@store/uiStore";

export function useNotification() {
  const addNotification = useUIStore((state) => state.addNotification);
  const removeNotification = useUIStore((state) => state.removeNotification);

  const success = useCallback(
    (message, duration) => {
      addNotification({ type: "success", message, duration });
    },
    [addNotification],
  );

  const error = useCallback(
    (message, duration) => {
      addNotification({ type: "error", message, duration: duration || 5000 });
    },
    [addNotification],
  );

  const info = useCallback(
    (message, duration) => {
      addNotification({ type: "info", message, duration });
    },
    [addNotification],
  );

  const warning = useCallback(
    (message, duration) => {
      addNotification({ type: "warning", message, duration });
    },
    [addNotification],
  );

  return {
    success,
    error,
    info,
    warning,
    removeNotification,
  };
}

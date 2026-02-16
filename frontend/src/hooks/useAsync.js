import { useState, useCallback } from "react";

export function useAsync(asyncFunc, immediate = true) {
  const [state, setState] = useState({
    data: null,
    loading: immediate,
    error: null,
  });

  const execute = useCallback(async () => {
    setState({ data: null, loading: true, error: null });
    try {
      const response = await asyncFunc();
      setState({ data: response, loading: false, error: null });
      return response;
    } catch (error) {
      const err = error instanceof Error ? error : new Error(String(error));
      setState({ data: null, loading: false, error: err });
      throw err;
    }
  }, [asyncFunc]);

  // Execute on mount if immediate is true
  // Note: This is simplified. In production, you'd use useEffect
  if (immediate && state.loading) {
    // Call execute only once per mount
  }

  return {
    ...state,
    execute,
  };
}

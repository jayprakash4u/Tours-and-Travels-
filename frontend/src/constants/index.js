// API Configuration
export const API_BASE_URL =
  import.meta.env.VITE_API_URL || "https://localhost:5001/api";
export const API_TIMEOUT = 30000;

// User Roles
export const ROLES = {
  ADMIN: "Admin",
  CUSTOMER: "Customer",
};

// Ticket Types
export const TICKET_TYPES = {
  FLIGHT: "Flight",
  BUS: "Bus",
  TRAIN: "Train",
};

// Booking Status
export const BOOKING_STATUS = {
  PENDING: "Pending",
  CONFIRMED: "Confirmed",
  IN_PROGRESS: "InProgress",
  COMPLETED: "Completed",
  CANCELLED: "Cancelled",
};

// Application Status
export const APPLICATION_STATUS = {
  PENDING: "Pending",
  UNDER_REVIEW: "UnderReview",
  APPROVED: "Approved",
  REJECTED: "Rejected",
};

// Visa Types
export const VISA_TYPES = {
  WORK: "Work",
  STUDY: "Study",
  TOURIST: "Tourist",
  FAMILY: "Family",
};

// Pagination
export const DEFAULT_PAGE_SIZE = 10;
export const PAGE_SIZE_OPTIONS = [5, 10, 25, 50, 100];

// Notification Duration (ms)
export const NOTIFICATION_DURATION = {
  SUCCESS: 3000,
  ERROR: 5000,
  INFO: 3000,
  WARNING: 4000,
};

// Storage Keys
export const STORAGE_KEYS = {
  TOKEN: "token",
  USER: "user",
  PREFERENCES: "preferences",
};

// API Error Messages
export const ERROR_MESSAGES = {
  NETWORK_ERROR: "Network error. Please check your connection.",
  UNAUTHORIZED: "Unauthorized. Please log in again.",
  FORBIDDEN: "You do not have permission to perform this action.",
  NOT_FOUND: "The requested resource was not found.",
  SERVER_ERROR: "Server error. Please try again later.",
  VALIDATION_ERROR: "Please check your input and try again.",
  DEFAULT: "An unexpected error occurred. Please try again.",
};

// Success Messages
export const SUCCESS_MESSAGES = {
  CREATED: "Successfully created.",
  UPDATED: "Successfully updated.",
  DELETED: "Successfully deleted.",
  LOGGED_IN: "Logged in successfully.",
  LOGGED_OUT: "Logged out successfully.",
};

// API Response Types
export interface ApiResponse<T = any> {
  success: boolean
  message: string
  data?: T
}

// User Types
export interface User {
  id: number
  fullName: string
  email: string
  role: 'Admin' | 'Customer'
  phoneNumber?: string
  address?: string
  profilePicture?: string
  isActive: boolean
  createdDate: string
}

export interface LoginCredentials {
  email: string
  password: string
}

export interface LoginResponse {
  id: number
  fullName: string
  email: string
  role: string
  profilePicture?: string
  token: string
}

// Ticket Types
export interface TicketRequest {
  id: number
  userId: number
  user?: User
  ticketType: 'Flight' | 'Bus' | 'Train'
  sourceLocation: string
  destinationLocation: string
  departureDate: string
  arrivalDate: string
  numberOfSeats: number
  status: 'Pending' | 'Confirmed' | 'Cancelled'
  totalPrice: number
  notes?: string
  createdDate: string
  updatedDate?: string
}

// Vehicle Booking Types
export interface VehicleBooking {
  id: number
  userId: number
  user?: User
  vehicleType: string
  pickupLocation: string
  dropLocation: string
  pickupDate: string
  dropDate: string
  status: 'Pending' | 'Confirmed' | 'InProgress' | 'Completed' | 'Cancelled'
  dailyRate: number
  totalPrice: number
  notes?: string
  createdDate: string
  updatedDate?: string
}

// Immigration Application Types
export interface ImmigrationApplication {
  id: number
  userId: number
  user?: User
  fullName: string
  passportNumber: string
  targetCountry: string
  passportExpiryDate: string
  status: 'Pending' | 'UnderReview' | 'Approved' | 'Rejected'
  documentsPath?: string
  visaType: string | null
  rejectionReason?: string
  adminNotes?: string
  createdDate: string
  updatedDate?: string
  completedDate?: string
}

// Auth State Type
export interface AuthState {
  user: User | null
  token: string | null
  isAuthenticated: boolean
  isLoading: boolean
  error: string | null
  login: (email: string, password: string) => Promise<void>
  logout: () => void
  setUser: (user: User | null) => void
  setToken: (token: string | null) => void
}

// Pagination Type
export interface PaginatedResponse<T> {
  items: T[]
  totalCount: number
  pageNumber: number
  pageSize: number
  totalPages: number
}

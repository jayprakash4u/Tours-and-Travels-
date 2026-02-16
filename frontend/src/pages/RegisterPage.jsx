import { useState } from 'react'
import { useNavigate, Link } from 'react-router-dom'
import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { z } from 'zod'
import { useAuthStore } from '@store/authStore'
import { useNotification } from '@hooks/useNotification'
import { Card, Button, Input } from '@components/index'

const registerSchema = z.object({
  fullName: z.string().min(2, 'Full name must be at least 2 characters'),
  email: z.string().email('Invalid email address'),
  password: z.string().min(6, 'Password must be at least 6 characters'),
  phoneNumber: z.string().optional(),
  address: z.string().optional(),
})

export function RegisterPage() {
  const navigate = useNavigate()
  const { register: registerUser } = useAuthStore()
  const { success: showSuccess, error: showError } = useNotification()
  const [isLoading, setIsLoading] = useState(false)
  
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm({
    resolver: zodResolver(registerSchema),
  })

  const onSubmit = async (data) => {
    try {
      setIsLoading(true)
      await registerUser(data)
      showSuccess('Registration successful! Please log in.')
      navigate('/login')
    } catch (err) {
      showError('Registration failed. Please try again.')
    } finally {
      setIsLoading(false)
    }
  }

  return (
    <div className="auth-page">
      <Card className="auth-card">
        <div className="text-center mb-8">
          <h1 className="auth-title">Create Account</h1>
          <p className="auth-subtitle">Join Alfa Travels & Tours today</p>
        </div>

        <form onSubmit={handleSubmit(onSubmit)} className="auth-form">
          <Input
            label="Full Name"
            placeholder="John Doe"
            error={errors.fullName?.message}
            {...register('fullName')}
          />

          <Input
            label="Email"
            type="email"
            placeholder="you@example.com"
            error={errors.email?.message}
            {...register('email')}
          />

          <Input
            label="Password"
            type="password"
            placeholder="••••••••"
            error={errors.password?.message}
            {...register('password')}
          />

          <Input
            label="Phone Number (Optional)"
            type="tel"
            placeholder="+1 (555) 000-0000"
            {...register('phoneNumber')}
          />

          <Input
            label="Address (Optional)"
            placeholder="123 Main St, City, State"
            {...register('address')}
          />

          <Button
            type="submit"
            variant="primary"
            className="w-full"
            isLoading={isLoading}
          >
            Create Account
          </Button>
        </form>

        <div className="mt-6 text-center">
          <p className="text-gray-600">
            Already have an account?{' '}
            <Link to="/login" className="auth-link">
              Sign in
            </Link>
          </p>
        </div>
      </Card>
    </div>
  )
}

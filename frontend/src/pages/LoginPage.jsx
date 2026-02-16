import { useState } from 'react'
import { useNavigate, Link } from 'react-router-dom'
import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { z } from 'zod'
import axios from 'axios'
import { useNotification } from '@hooks/useNotification'
import { Card, Button, Input } from '@components/index'

const loginSchema = z.object({
  email: z.string().email('Invalid email address'),
  password: z.string().min(6, 'Password must be at least 6 characters'),
})

export function LoginPage() {
  const navigate = useNavigate()
  const { error: showError } = useNotification()
  const [isLoading, setIsLoading] = useState(false)
  
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm({
    resolver: zodResolver(loginSchema),
  })

  const onSubmit = async (data) => {
    try {
      setIsLoading(true)
      const response = await axios.post('/api/auth/login', { email: data.email, password: data.password })
      
      if (response.data.success && response.data.data) {
        const loginData = response.data.data
        localStorage.setItem('token', loginData.token)
        localStorage.setItem('user', JSON.stringify(loginData))
        navigate('/dashboard')
      } else {
        throw new Error(response.data.message || 'Login failed')
      }
    } catch (err) {
      const message = err.response?.data?.message || err.message || 'Login failed'
      showError(message)
    } finally {
      setIsLoading(false)
    }
  }

  return (
    <div className="auth-page">
      <Card className="auth-card">
        <div className="text-center mb-8">
          <h1 className="auth-title">Alfa Travels & Tours</h1>
          <p className="auth-subtitle">Travel & Immigration Management</p>
        </div>

        <form onSubmit={handleSubmit(onSubmit)} className="auth-form">
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

          <Button
            type="submit"
            variant="primary"
            className="w-full"
            isLoading={isLoading}
          >
            Sign In
          </Button>
        </form>

        <div className="mt-6 text-center">
          <p className="text-gray-600">
            Don't have an account?{' '}
            <Link to="/register" className="auth-link">
              Create one
            </Link>
          </p>
        </div>
      </Card>
    </div>
  )
}

import { useNavigate } from 'react-router-dom'
import { Button } from '@components/Button'

export default function LandingPage() {
  const navigate = useNavigate()

  return (
    <div className="landing-page">
      {/* Navigation Bar */}
      <nav className="landing-nav">
        <div className="landing-nav-container">
          <div className="landing-nav-brand text-white text-3xl font-bold flex items-center gap-2">
            <span>✈️</span>
            Alfa Travels & Tours
          </div>
          <div className="flex gap-4">
            <Button
              variant="outline"
              onClick={() => navigate('/login')}
            >
              Login
            </Button>
            <Button
              variant="primary"
              onClick={() => navigate('/register')}
            >
              Register
            </Button>
          </div>
        </div>
      </nav>

      {/* Hero Section */}
      <section className="landing-hero">
        <div className="landing-hero-grid">
          {/* Left Content */}
          <div className="text-white">
            <h1 className="landing-hero-title">
              Your Journey Starts Here
            </h1>
            <p className="landing-hero-text">
              Book flights, find vehicle rentals, and manage your immigration documents all in one place.
              Experience seamless travel management with Alfa Travels & Tours.
            </p>
            <div className="landing-hero-buttons">
              <Button
                size="lg"
                variant="primary"
                onClick={() => navigate('/register')}
                className="text-lg"
              >
                Get Started
              </Button>
              <Button
                size="lg"
                variant="outline"
                onClick={() => navigate('/login')}
                className="text-lg"
              >
                Sign In
              </Button>
            </div>
          </div>

          {/* Right Image/Icon */}
          <div className="landing-hero-image">
            <div className="relative">
              <div className="landing-hero-image-bg"></div>
              <span className="landing-hero-icon">✈️</span>
            </div>
          </div>
        </div>
      </section>

      {/* Features Section */}
      <section className="landing-features">
        <div className="landing-features-container">
          <h2 className="landing-features-title">
            Why Choose Alfa Travels & Tours?
          </h2>

          <div className="landing-features-grid">
            {/* Feature 1 */}
            <div className="landing-feature-card">
              <span className="landing-feature-icon text-primary-300">🛫</span>
              <h3 className="landing-feature-title">Flight Bookings</h3>
              <p className="landing-feature-text">
                Search and book flights to thousands of destinations worldwide with competitive prices.
              </p>
            </div>

            {/* Feature 2 */}
            <div className="landing-feature-card">
              <span className="landing-feature-icon text-secondary-300">🚗</span>
              <h3 className="landing-feature-title">Vehicle Rentals</h3>
              <p className="landing-feature-text">
                Rent reliable vehicles from our extensive fleet for your travel needs.
              </p>
            </div>

            {/* Feature 3 */}
            <div className="landing-feature-card">
              <span className="landing-feature-icon text-success-300">⚡</span>
              <h3 className="landing-feature-title">Quick Processing</h3>
              <p className="landing-feature-text">
                Fast and efficient processing of bookings and applications in minutes.
              </p>
            </div>

            {/* Feature 4 */}
            <div className="landing-feature-card">
              <span className="landing-feature-icon text-danger-300">🎧</span>
              <h3 className="landing-feature-title">24/7 Support</h3>
              <p className="landing-feature-text">
                Our dedicated support team is always ready to help you anytime, anywhere.
              </p>
            </div>
          </div>
        </div>
      </section>

      {/* CTA Section */}
      <section className="landing-cta">
        <div className="landing-cta-card">
          <h2 className="landing-cta-title">
            Ready to Start Your Journey?
          </h2>
          <p className="landing-cta-text">
            Join thousands of travelers who trust Alfa Travels & Tours for their travel and immigration needs.
          </p>
          <div className="landing-cta-buttons">
            <Button
              size="lg"
              variant="primary"
              onClick={() => navigate('/register')}
              className="text-lg"
            >
              Create Account
            </Button>
            <Button
              size="lg"
              variant="outline"
              onClick={() => navigate('/login')}
              className="text-lg"
            >
              Login
            </Button>
          </div>
        </div>
      </section>

      {/* Footer */}
      <footer className="landing-footer">
        <div className="landing-footer-container">
          <div className="landing-footer-grid">
            <div>
              <h4 className="landing-footer-title">About</h4>
              <ul className="landing-footer-list">
                <li className="landing-footer-item"><a href="#" className="landing-footer-link">About Us</a></li>
                <li className="landing-footer-item"><a href="#" className="landing-footer-link">Careers</a></li>
                <li className="landing-footer-item"><a href="#" className="landing-footer-link">Blog</a></li>
              </ul>
            </div>
            <div>
              <h4 className="landing-footer-title">Services</h4>
              <ul className="landing-footer-list">
                <li className="landing-footer-item"><a href="#" className="landing-footer-link">Flights</a></li>
                <li className="landing-footer-item"><a href="#" className="landing-footer-link">Rentals</a></li>
                <li className="landing-footer-item"><a href="#" className="landing-footer-link">Immigration</a></li>
              </ul>
            </div>
            <div>
              <h4 className="landing-footer-title">Support</h4>
              <ul className="landing-footer-list">
                <li className="landing-footer-item"><a href="#" className="landing-footer-link">Contact Us</a></li>
                <li className="landing-footer-item"><a href="#" className="landing-footer-link">FAQ</a></li>
                <li className="landing-footer-item"><a href="#" className="landing-footer-link">Help</a></li>
              </ul>
            </div>
            <div>
              <h4 className="landing-footer-title">Legal</h4>
              <ul className="landing-footer-list">
                <li className="landing-footer-item"><a href="#" className="landing-footer-link">Privacy</a></li>
                <li className="landing-footer-item"><a href="#" className="landing-footer-link">Terms</a></li>
                <li className="landing-footer-item"><a href="#" className="landing-footer-link">Cookies</a></li>
              </ul>
            </div>
          </div>

          <div className="landing-footer-bottom">
            <p>&copy; 2024 Alfa Travels & Tours. All rights reserved.</p>
          </div>
        </div>
      </footer>
    </div>
  )
}

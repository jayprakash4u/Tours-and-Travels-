export function Card({ children, className, title, subtitle }) {
  const cardClassName = ['card', className].filter(Boolean).join(' ')
  
  return (
    <div className={cardClassName}>
      {title && (
        <div className="mb-4">
          <h3 className="card-title">{title}</h3>
          {subtitle && <p className="card-subtitle">{subtitle}</p>}
        </div>
      )}
      {children}
    </div>
  )
}

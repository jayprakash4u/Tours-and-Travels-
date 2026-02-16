export function LoadingSpinner({ size = 'md', fullScreen = false }) {
  const sizeClasses = {
    sm: 'spinner-sm',
    md: 'spinner-md',
    lg: 'spinner-lg',
  }

  const spinner = (
    <div className={`spinner ${sizeClasses[size]}`} />
  )

  if (fullScreen) {
    return (
      <div className="loading-fullscreen">
        {spinner}
      </div>
    )
  }

  return <div className="flex items-center justify-center">{spinner}</div>
}

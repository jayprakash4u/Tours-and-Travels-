export function Button({
  variant = 'primary',
  size = 'md',
  isLoading = false,
  disabled,
  className,
  children,
  ...props
}) {
  const baseStyles = 'btn'
  
  const variantStyles = {
    primary: 'btn-primary',
    secondary: 'btn-secondary',
    danger: 'btn-danger',
    outline: 'btn-outline',
  }

  const sizeStyles = {
    sm: 'btn-sm',
    md: 'btn-md',
    lg: 'btn-lg',
  }

  const combinedClassName = [
    baseStyles,
    variantStyles[variant],
    sizeStyles[size],
    className
  ].filter(Boolean).join(' ')

  return (
    <button
      className={combinedClassName}
      disabled={disabled || isLoading}
      {...props}
    >
      {isLoading ? 'Loading...' : children}
    </button>
  )
}

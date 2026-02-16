import { forwardRef } from 'react'

export const Input = forwardRef(
  ({ label, error, helperText, className, ...props }, ref) => {
    const inputClassName = [
      'input-field',
      error ? 'error' : '',
      className
    ].filter(Boolean).join(' ')

    return (
      <div className="input-group">
        {label && (
          <label className="input-label">
            {label}
          </label>
        )}
        <input
          ref={ref}
          className={inputClassName}
          {...props}
        />
        {error && <p className="input-error">{error}</p>}
        {helperText && !error && <p className="input-helper">{helperText}</p>}
      </div>
    )
  }
)

Input.displayName = 'Input'

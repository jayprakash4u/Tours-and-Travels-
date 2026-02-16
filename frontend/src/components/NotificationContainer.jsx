import { useUIStore } from '@store/uiStore'
import { CheckIcon, CloseIcon, WarningIcon, InfoIcon } from '../icons'

export function NotificationContainer() {
  const notifications = useUIStore((state) => state.notifications)
  const removeNotification = useUIStore((state) => state.removeNotification)

  const getNotificationClass = (type) => {
    const classes = {
      success: 'notification-success',
      error: 'notification-error',
      info: 'notification-info',
      warning: 'notification-warning',
    }
    return classes[type] || 'notification-info'
  }

  const getIcon = (type) => {
    switch (type) {
      case 'success': return <CheckIcon />
      case 'error': return <CloseIcon />
      case 'info': return <InfoIcon />
      case 'warning': return <WarningIcon />
      default: return <InfoIcon />
    }
  }

  return (
    <div className="notification-container">
      {notifications.map((notification) => (
        <div
          key={notification.id}
          className={`notification ${getNotificationClass(notification.type)}`}
        >
          <div className="notification-icon">
            {getIcon(notification.type)}
          </div>
          <div className="notification-message">{notification.message}</div>
          <button
            onClick={() => removeNotification(notification.id)}
            className="notification-close"
          >
            <CloseIcon />
          </button>
        </div>
      ))}
    </div>
  )
}

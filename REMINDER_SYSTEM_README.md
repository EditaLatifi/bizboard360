# Invoice Reminder System

## Overview
A comprehensive payment reminder system that alerts you whenever you have invoices or receipts that need to be paid. The system automatically tracks due dates, payment status, and sends notifications for urgent payments.

## Features

### 🔔 **Automatic Reminders**
- **Real-time monitoring** of invoice due dates
- **Automatic alerts** for overdue payments
- **Priority-based notifications** (Critical, High, Medium, Low)
- **Smart filtering** to show only urgent reminders

### 📊 **Dashboard Overview**
- **Summary cards** showing overdue, due today, and due this week counts
- **Visual indicators** with color-coded priority levels
- **Real-time updates** every 2 minutes
- **Quick action buttons** for managing reminders

### 🚨 **Notification System**
- **Browser notifications** (with permission)
- **Audio alerts** for urgent reminders
- **In-page alerts** with detailed information
- **Page title updates** showing urgent count
- **Auto-refresh** every minute

### 💰 **Payment Tracking**
- **Due date monitoring** with automatic status updates
- **Payment status tracking** (Pending, Sent, Acknowledged, Paid, Overdue, Cancelled)
- **Amount tracking** with currency formatting
- **Client information** management

## How It Works

### 1. **Reminder Creation**
- Add new reminders with invoice details
- Set due dates and priority levels
- Add notes and client information
- Automatic status management

### 2. **Smart Monitoring**
- System checks reminders every minute
- Filters for urgent items (overdue, due today, critical priority)
- Updates counts and displays in real-time
- Sends notifications when action is needed

### 3. **Alert System**
- **Overdue invoices**: Red alerts with immediate attention required
- **Due today**: Yellow warnings for same-day payments
- **Critical priority**: Pulsing red alerts for high-value items
- **Browser notifications**: Desktop alerts even when tab is not active

## File Structure

```
Models/
├── InvoiceReminder.cs          # Reminder data model
├── ReminderStatus.cs           # Payment status enums
└── ReminderPriority.cs         # Priority level enums

Services/
├── IInvoiceReminderService.cs  # Service interface
└── InvoiceReminderService.cs   # Business logic implementation

Controllers/
└── ReminderController.cs       # API endpoints for reminders

Views/
├── Reminder/
│   └── Index.cshtml           # Main reminder management page
└── Shared/
    └── _ReminderWidget.cshtml # Sidebar widget component

wwwroot/js/
└── reminder-notifications.js   # Notification system
```

## Usage

### **Viewing Reminders**
1. Navigate to `/Reminder` for the full reminder dashboard
2. See summary cards with counts
3. View detailed reminder table
4. Filter by priority and status

### **Adding New Reminders**
1. Click "Add Reminder" button
2. Fill in invoice details:
   - Invoice number
   - Client name
   - Amount
   - Due date
   - Priority level
   - Notes
3. Save to create the reminder

### **Managing Reminders**
- **Mark as Paid**: Update payment status
- **Send Reminder**: Track reminder communications
- **View Details**: See full reminder information
- **Edit/Delete**: Modify existing reminders

### **Notification Settings**
- **Browser notifications**: Grant permission when prompted
- **Audio alerts**: Automatic for urgent reminders
- **Page title updates**: Shows urgent count in browser tab
- **Auto-refresh**: Updates every 2 minutes

## Sample Data

The system comes with sample reminders to demonstrate functionality:

- **Marilyn Workman**: $650,036.34 - Due in 3 days (High Priority)
- **Talan Siphron**: $450,000.00 - Overdue (Critical Priority)
- **Tech Solutions Inc**: $125,000.00 - Due in 7 days (Medium Priority)
- **Global Enterprises**: $890,000.00 - Due today (High Priority)

## Technical Details

### **Dependencies**
- ASP.NET Core 8.0
- Entity Framework Core
- jQuery for frontend interactions
- Bootstrap for UI components
- Font Awesome for icons

### **Service Registration**
```csharp
// Program.cs
builder.Services.AddScoped<IInvoiceReminderService, InvoiceReminderService>();
```

### **API Endpoints**
- `GET /Reminder` - Main reminder page
- `GET /Reminder/GetReminders` - Get all active reminders
- `GET /Reminder/GetOverdue` - Get overdue invoices
- `GET /Reminder/GetDueToday` - Get today's due payments
- `GET /Reminder/GetDueThisWeek` - Get this week's due payments
- `POST /Reminder/CreateReminder` - Create new reminder
- `POST /Reminder/MarkAsPaid` - Mark invoice as paid
- `POST /Reminder/SendReminder` - Send reminder notification

## Customization

### **Reminder Priorities**
- **Critical**: Red alerts, immediate attention
- **High**: Orange alerts, same-day action
- **Medium**: Blue alerts, within week
- **Low**: Gray alerts, standard processing

### **Notification Timing**
- **Check interval**: Every 60 seconds
- **Page refresh**: Every 2 minutes
- **Alert auto-dismiss**: 30 seconds
- **Sound duration**: 0.3 seconds

### **Display Limits**
- **Sidebar widget**: Maximum 5 urgent reminders
- **Invoice page**: Maximum 5 urgent reminders
- **Dashboard**: All active reminders

## Future Enhancements

### **Planned Features**
- Email notifications
- SMS alerts
- Calendar integration
- Payment gateway integration
- Automated reminder scheduling
- Client communication tracking

### **Integration Possibilities**
- QuickBooks/accounting software
- CRM systems
- Project management tools
- Financial reporting
- Audit trails

## Support

For questions or issues with the reminder system:
1. Check the browser console for JavaScript errors
2. Verify the reminder service is registered in Program.cs
3. Ensure the ReminderController is accessible
4. Check notification permissions in browser settings

## Security Notes

- Reminder data is stored in memory (for demo purposes)
- In production, implement proper authentication
- Add user-specific reminder filtering
- Implement audit logging for all actions
- Secure API endpoints with proper authorization

using Finlab.Models;

namespace Finlab.Services
{
    public class InvoiceReminderService : IInvoiceReminderService
    {
        private readonly List<InvoiceReminder> _reminders = new();
        
        public InvoiceReminderService()
        {
            // Initialize with sample data for demonstration
            InitializeSampleData();
        }
        
        private void InitializeSampleData()
        {
            _reminders.AddRange(new List<InvoiceReminder>
            {
                new InvoiceReminder
                {
                    Id = 1,
                    InvoiceNumber = "INV-0001234",
                    ClientName = "Marilyn Workman",
                    Amount = 650036.34m,
                    DueDate = DateTime.Today.AddDays(3),
                    Status = ReminderStatus.Pending,
                    Priority = ReminderPriority.High,
                    Notes = "Client requested payment extension",
                    ReminderDate = DateTime.Today.AddDays(1)
                },
                new InvoiceReminder
                {
                    Id = 2,
                    InvoiceNumber = "INV-0001235",
                    ClientName = "Talan Siphron",
                    Amount = 450000.00m,
                    DueDate = DateTime.Today.AddDays(-2),
                    Status = ReminderStatus.Overdue,
                    Priority = ReminderPriority.Critical,
                    Notes = "Payment overdue - immediate action required",
                    ReminderDate = DateTime.Today,
                    ReminderCount = 2
                },
                new InvoiceReminder
                {
                    Id = 3,
                    InvoiceNumber = "INV-0001236",
                    ClientName = "Tech Solutions Inc",
                    Amount = 125000.00m,
                    DueDate = DateTime.Today.AddDays(7),
                    Status = ReminderStatus.Pending,
                    Priority = ReminderPriority.Medium,
                    Notes = "Standard payment terms",
                    ReminderDate = DateTime.Today.AddDays(5)
                },
                new InvoiceReminder
                {
                    Id = 4,
                    InvoiceNumber = "INV-0001237",
                    ClientName = "Global Enterprises",
                    Amount = 890000.00m,
                    DueDate = DateTime.Today,
                    Status = ReminderStatus.Pending,
                    Priority = ReminderPriority.High,
                    Notes = "Due today - send reminder",
                    ReminderDate = DateTime.Today
                }
            });
        }
        
        public async Task<List<InvoiceReminder>> GetActiveRemindersAsync()
        {
            return await Task.FromResult(_reminders.Where(r => r.IsActive && r.Status != ReminderStatus.Paid).ToList());
        }
        
        public async Task<List<InvoiceReminder>> GetOverdueInvoicesAsync()
        {
            return await Task.FromResult(_reminders.Where(r => r.IsActive && r.DueDate < DateTime.Today && r.Status != ReminderStatus.Paid).ToList());
        }
        
        public async Task<List<InvoiceReminder>> GetDueThisWeekAsync()
        {
            var weekEnd = DateTime.Today.AddDays(7);
            return await Task.FromResult(_reminders.Where(r => r.IsActive && r.DueDate >= DateTime.Today && r.DueDate <= weekEnd && r.Status != ReminderStatus.Paid).ToList());
        }
        
        public async Task<List<InvoiceReminder>> GetDueTodayAsync()
        {
            return await Task.FromResult(_reminders.Where(r => r.IsActive && r.DueDate.Date == DateTime.Today.Date && r.Status != ReminderStatus.Paid).ToList());
        }
        
        public async Task<int> GetReminderCountAsync()
        {
            var overdue = await GetOverdueInvoicesAsync();
            var dueToday = await GetDueTodayAsync();
            var dueThisWeek = await GetDueThisWeekAsync();
            
            return overdue.Count + dueToday.Count + dueThisWeek.Count;
        }
        
        public async Task<InvoiceReminder> CreateReminderAsync(InvoiceReminder reminder)
        {
            reminder.Id = _reminders.Max(r => r.Id) + 1;
            reminder.CreatedDate = DateTime.Now;
            reminder.IsActive = true;
            reminder.Status = ReminderStatus.Pending;
            
            _reminders.Add(reminder);
            return await Task.FromResult(reminder);
        }
        
        public async Task<InvoiceReminder> UpdateReminderAsync(InvoiceReminder reminder)
        {
            var existing = _reminders.FirstOrDefault(r => r.Id == reminder.Id);
            if (existing != null)
            {
                var index = _reminders.IndexOf(existing);
                _reminders[index] = reminder;
                return await Task.FromResult(reminder);
            }
            throw new ArgumentException("Reminder not found");
        }
        
        public async Task<bool> MarkAsPaidAsync(int reminderId)
        {
            var reminder = _reminders.FirstOrDefault(r => r.Id == reminderId);
            if (reminder != null)
            {
                reminder.Status = ReminderStatus.Paid;
                reminder.IsActive = false;
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
        
        public async Task<bool> SendReminderAsync(int reminderId)
        {
            var reminder = _reminders.FirstOrDefault(r => r.Id == reminderId);
            if (reminder != null)
            {
                reminder.LastReminderSent = DateTime.Now;
                reminder.ReminderCount++;
                reminder.Status = ReminderStatus.Sent;
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
        
        public async Task<List<InvoiceReminder>> GetRemindersByPriorityAsync(ReminderPriority priority)
        {
            return await Task.FromResult(_reminders.Where(r => r.IsActive && r.Priority == priority && r.Status != ReminderStatus.Paid).ToList());
        }
    }
}

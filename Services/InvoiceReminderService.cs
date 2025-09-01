using Finlab.Models;
using Finlab.Data;
using Microsoft.EntityFrameworkCore;

namespace Finlab.Services
{
    public class InvoiceReminderService : IInvoiceReminderService
    {
        private readonly ApplicationDbContext _context;
        
        public InvoiceReminderService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<List<InvoiceReminder>> GetActiveRemindersAsync()
        {
            return await _context.InvoiceReminders
                .Where(r => r.IsActive && r.Status != ReminderStatus.Paid)
                .ToListAsync();
        }
        
        public async Task<List<InvoiceReminder>> GetOverdueInvoicesAsync()
        {
            return await _context.InvoiceReminders
                .Where(r => r.IsActive && r.DueDate < DateTime.Today && r.Status != ReminderStatus.Paid)
                .ToListAsync();
        }
        
        public async Task<List<InvoiceReminder>> GetDueThisWeekAsync()
        {
            var weekEnd = DateTime.Today.AddDays(7);
            return await _context.InvoiceReminders
                .Where(r => r.IsActive && r.DueDate >= DateTime.Today && r.DueDate <= weekEnd && r.Status != ReminderStatus.Paid)
                .ToListAsync();
        }
        
        public async Task<List<InvoiceReminder>> GetDueTodayAsync()
        {
            return await _context.InvoiceReminders
                .Where(r => r.IsActive && r.DueDate.Date == DateTime.Today.Date && r.Status != ReminderStatus.Paid)
                .ToListAsync();
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
            reminder.CreatedDate = DateTime.Now;
            reminder.IsActive = true;
            reminder.Status = ReminderStatus.Pending;
            
            _context.InvoiceReminders.Add(reminder);
            await _context.SaveChangesAsync();
            return reminder;
        }
        
        public async Task<InvoiceReminder> UpdateReminderAsync(InvoiceReminder reminder)
        {
            var existing = await _context.InvoiceReminders.FindAsync(reminder.Id);
            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(reminder);
                await _context.SaveChangesAsync();
                return reminder;
            }
            throw new ArgumentException("Reminder not found");
        }
        
        public async Task<bool> MarkAsPaidAsync(int reminderId)
        {
            var reminder = await _context.InvoiceReminders.FindAsync(reminderId);
            if (reminder != null)
            {
                reminder.Status = ReminderStatus.Paid;
                reminder.IsActive = false;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        
        public async Task<bool> SendReminderAsync(int reminderId)
        {
            var reminder = await _context.InvoiceReminders.FindAsync(reminderId);
            if (reminder != null)
            {
                reminder.LastReminderSent = DateTime.Now;
                reminder.ReminderCount++;
                reminder.Status = ReminderStatus.Sent;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        
        public async Task<bool> ConfirmInvoiceAsync(int reminderId)
        {
            var reminder = await _context.InvoiceReminders.FindAsync(reminderId);
            if (reminder != null)
            {
                reminder.Status = ReminderStatus.Acknowledged;
                reminder.Notes = reminder.Notes + "\n[CONFIRMED] Invoice confirmed on " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        
        public async Task<List<InvoiceReminder>> GetRemindersByPriorityAsync(ReminderPriority priority)
        {
            return await _context.InvoiceReminders
                .Where(r => r.IsActive && r.Priority == priority && r.Status != ReminderStatus.Paid)
                .ToListAsync();
        }
    }
}

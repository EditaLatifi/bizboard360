using Finlab.Models;

namespace Finlab.Services
{
    public interface IInvoiceReminderService
    {
        Task<List<InvoiceReminder>> GetActiveRemindersAsync();
        Task<List<InvoiceReminder>> GetOverdueInvoicesAsync();
        Task<List<InvoiceReminder>> GetDueThisWeekAsync();
        Task<List<InvoiceReminder>> GetDueTodayAsync();
        Task<int> GetReminderCountAsync();
        Task<InvoiceReminder> CreateReminderAsync(InvoiceReminder reminder);
        Task<InvoiceReminder> UpdateReminderAsync(InvoiceReminder reminder);
        Task<bool> MarkAsPaidAsync(int reminderId);
        Task<bool> SendReminderAsync(int reminderId);
        Task<bool> ConfirmInvoiceAsync(int reminderId);
        Task<List<InvoiceReminder>> GetRemindersByPriorityAsync(ReminderPriority priority);
    }
}

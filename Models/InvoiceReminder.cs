using System.ComponentModel.DataAnnotations;

namespace Finlab.Models
{
    public class InvoiceReminder
    {
        public int Id { get; set; }
        [Required]
        public required string InvoiceNumber { get; set; }
        [Required]
        public required string ClientName { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        public DateTime? ReminderDate { get; set; }
        public ReminderStatus Status { get; set; }
        public ReminderPriority Priority { get; set; }
        public string? Notes { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? LastReminderSent { get; set; }
        public int ReminderCount { get; set; } = 0;
    }
    public enum ReminderStatus { Pending, Sent, Acknowledged, Paid, Overdue, Cancelled }
    public enum ReminderPriority { Low, Medium, High, Critical }
}

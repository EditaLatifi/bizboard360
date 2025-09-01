using System.ComponentModel.DataAnnotations;

namespace Finlab.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        
        [Required]
        public required string InvoiceNumber { get; set; }
        
        [Required]
        public InvoiceType Type { get; set; }
        
        [Required]
        public InvoiceStatus Status { get; set; }
        
        [Required]
        public decimal Subtotal { get; set; }
        
        public decimal TaxAmount { get; set; } = 0;
        
        public decimal DiscountAmount { get; set; } = 0;
        
        [Required]
        public decimal TotalAmount { get; set; }
        
        [Required]
        public DateTime IssueDate { get; set; }
        
        [Required]
        public DateTime DueDate { get; set; }
        
        public DateTime? PaymentDate { get; set; }
        
        public PaymentStatus PaymentStatus { get; set; }
        
        public PaymentMethod PaymentMethod { get; set; }
        
        [Required]
        public required string ClientName { get; set; }
        
        public string? ClientEmail { get; set; }
        
        public string? ClientPhone { get; set; }
        
        public string? ClientAddress { get; set; }
        
        public string? Description { get; set; }
        
        public string? Notes { get; set; }
        
        public string? Terms { get; set; }
        
        public string? AttachmentPath { get; set; }
        
        public bool IsRecurring { get; set; } = false;
        
        public RecurrenceType? RecurrenceType { get; set; }
        
        public int? RecurrenceInterval { get; set; }
        
        public DateTime? NextRecurrenceDate { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        public DateTime? ModifiedDate { get; set; }
        
        public string? CreatedBy { get; set; }
        
        public string? ModifiedBy { get; set; }
        
        // Navigation properties
        public int? ClientId { get; set; }
        public Client? Client { get; set; }
        
        public List<InvoiceItem> Items { get; set; } = new();
        
        public List<PaymentSchedule> PaymentSchedules { get; set; } = new();
    }

    public class InvoiceItem
    {
        public int Id { get; set; }
        
        [Required]
        public required string Description { get; set; }
        
        public int Quantity { get; set; } = 1;
        
        [Required]
        public decimal UnitPrice { get; set; }
        
        public decimal DiscountPercentage { get; set; } = 0;
        
        [Required]
        public decimal TotalPrice { get; set; }
        
        public string? Notes { get; set; }
        
        // Navigation property
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; } = null!;
    }

    public class PaymentSchedule
    {
        public int Id { get; set; }
        
        [Required]
        public DateTime DueDate { get; set; }
        
        [Required]
        public decimal Amount { get; set; }
        
        public PaymentStatus Status { get; set; }
        
        public DateTime? PaymentDate { get; set; }
        
        public string? Notes { get; set; }
        
        // Navigation property
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; } = null!;
    }

    public enum InvoiceType
    {
        Sales,
        Service,
        Recurring,
        Credit,
        Debit
    }

    public enum InvoiceStatus
    {
        Draft,
        Sent,
        Viewed,
        Paid,
        Overdue,
        Cancelled,
        Disputed
    }

    public enum PaymentStatus
    {
        Pending,
        Partial,
        Paid,
        Overdue,
        Cancelled
    }
}

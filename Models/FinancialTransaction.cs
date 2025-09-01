using System.ComponentModel.DataAnnotations;

namespace Finlab.Models
{
    public class FinancialTransaction
    {
        public int Id { get; set; }
        
        [Required]
        public required string TransactionNumber { get; set; }
        
        [Required]
        public TransactionType Type { get; set; }
        
        [Required]
        public TransactionCategory Category { get; set; }
        
        [Required]
        public decimal Amount { get; set; }
        
        [Required]
        public required string Description { get; set; }
        
        public string? ReferenceNumber { get; set; }
        
        public DateTime TransactionDate { get; set; }
        
        public DateTime? DueDate { get; set; }
        
        public TransactionStatus Status { get; set; }
        
        public PaymentMethod PaymentMethod { get; set; }
        
        public string? Notes { get; set; }
        
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
        public int? InvoiceId { get; set; }
        public Invoice? Invoice { get; set; }
        
        public int? ClientId { get; set; }
        public Client? Client { get; set; }
        
        public int? VendorId { get; set; }
        public Vendor? Vendor { get; set; }
    }

    public enum TransactionType
    {
        Income,
        Expense
    }

    public enum TransactionCategory
    {
        // Income Categories
        Sales,
        Services,
        RentIncome,
        Interest,
        OtherIncome,
        
        // Expense Categories
        RentExpense,
        Salaries,
        DeliveryFees,
        Maintenance,
        Utilities,
        Insurance,
        Marketing,
        OfficeSupplies,
        Travel,
        OtherExpenses
    }

    public enum TransactionStatus
    {
        Pending,
        Approved,
        Completed,
        Cancelled,
        Overdue
    }

    public enum PaymentMethod
    {
        Cash,
        BankTransfer,
        CreditCard,
        Check,
        PayPal,
        Other
    }

    public enum RecurrenceType
    {
        Daily,
        Weekly,
        Monthly,
        Quarterly,
        Yearly
    }
}

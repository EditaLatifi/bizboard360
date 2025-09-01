using System.ComponentModel.DataAnnotations;

namespace Finlab.Models
{
    public class POSTransaction
    {
        public int Id { get; set; }
        
        [Required]
        public required string TransactionNumber { get; set; }
        
        [Required]
        public POSTransactionType Type { get; set; }
        
        [Required]
        public POSTransactionStatus Status { get; set; }
        
        [Required]
        public decimal Subtotal { get; set; }
        
        public decimal TaxAmount { get; set; } = 0;
        
        public decimal DiscountAmount { get; set; } = 0;
        
        [Required]
        public decimal TotalAmount { get; set; }
        
        public decimal AmountPaid { get; set; } = 0;
        
        public decimal ChangeAmount { get; set; } = 0;
        
        [Required]
        public DateTime TransactionDate { get; set; }
        
        public DateTime? PaymentDate { get; set; }
        
        public PaymentMethod PaymentMethod { get; set; }
        
        public string? ReceiptNumber { get; set; }
        
        public string? ReferenceNumber { get; set; }
        
        public string? Notes { get; set; }
        
        public bool IsRefund { get; set; } = false;
        
        public int? RefundedTransactionId { get; set; }
        
        public string? CashierName { get; set; }
        
        public string? TerminalId { get; set; }
        
        public string? Location { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        public DateTime? ModifiedDate { get; set; }
        
        public string? CreatedBy { get; set; }
        
        public string? ModifiedBy { get; set; }
        
        // Navigation properties
        public int? ClientId { get; set; }
        public Client? Client { get; set; }
        
        public List<POSTransactionItem> Items { get; set; } = new();
        
        public List<POSTransactionPayment> Payments { get; set; } = new();
    }

    public class POSTransactionItem
    {
        public int Id { get; set; }
        
        [Required]
        public required string ProductName { get; set; }
        
        public string? ProductCode { get; set; }
        
        public string? SKU { get; set; }
        
        [Required]
        public int Quantity { get; set; }
        
        [Required]
        public decimal UnitPrice { get; set; }
        
        public decimal DiscountPercentage { get; set; } = 0;
        
        [Required]
        public decimal TotalPrice { get; set; }
        
        public string? Notes { get; set; }
        
        // Navigation property
        public int POSTransactionId { get; set; }
        public POSTransaction POSTransaction { get; set; } = null!;
    }

    public class POSTransactionPayment
    {
        public int Id { get; set; }
        
        [Required]
        public PaymentMethod Method { get; set; }
        
        [Required]
        public decimal Amount { get; set; }
        
        public string? ReferenceNumber { get; set; }
        
        public string? Notes { get; set; }
        
        public DateTime PaymentDate { get; set; }
        
        // Navigation property
        public int POSTransactionId { get; set; }
        public POSTransaction POSTransaction { get; set; } = null!;
    }

    public enum POSTransactionType
    {
        Sale,
        Refund,
        Exchange,
        Void,
        Adjustment
    }

    public enum POSTransactionStatus
    {
        Pending,
        Completed,
        Cancelled,
        Refunded,
        Voided
    }
}

using System.ComponentModel.DataAnnotations;

namespace Finlab.Models
{
    public class Client
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }
        
        [MaxLength(100)]
        public string? CompanyName { get; set; }
        
        [EmailAddress]
        [MaxLength(100)]
        public string? Email { get; set; }
        
        [Phone]
        [MaxLength(20)]
        public string? Phone { get; set; }
        
        [MaxLength(200)]
        public string? Address { get; set; }
        
        [MaxLength(50)]
        public string? City { get; set; }
        
        [MaxLength(50)]
        public string? State { get; set; }
        
        [MaxLength(20)]
        public string? PostalCode { get; set; }
        
        [MaxLength(50)]
        public string? Country { get; set; }
        
        [MaxLength(50)]
        public string? TaxNumber { get; set; }
        
        public ClientType Type { get; set; }
        
        public ClientStatus Status { get; set; }
        
        public decimal CreditLimit { get; set; } = 0;
        
        public int PaymentTerms { get; set; } = 30; // Days
        
        public string? Notes { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        public DateTime? ModifiedDate { get; set; }
        
        public string? CreatedBy { get; set; }
        
        public string? ModifiedBy { get; set; }
        
        // Navigation properties
        public List<Invoice> Invoices { get; set; } = new();
        
        public List<FinancialTransaction> Transactions { get; set; } = new();
    }

    public enum ClientType
    {
        Individual,
        Business,
        Government,
        NonProfit
    }

    public enum ClientStatus
    {
        Active,
        Inactive,
        Suspended,
        Blacklisted
    }
}

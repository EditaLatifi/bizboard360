using System.ComponentModel.DataAnnotations;

namespace Finlab.Models
{
    public class Vendor
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
        
        [MaxLength(50)]
        public string? AccountNumber { get; set; }
        
        public VendorType Type { get; set; }
        
        public VendorStatus Status { get; set; }
        
        public int PaymentTerms { get; set; } = 30; // Days
        
        public decimal CreditLimit { get; set; } = 0;
        
        public string? ContactPerson { get; set; }
        
        public string? Notes { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        public DateTime? ModifiedDate { get; set; }
        
        public string? CreatedBy { get; set; }
        
        public string? ModifiedBy { get; set; }
        
        // Navigation properties
        public List<FinancialTransaction> Transactions { get; set; } = new();
    }

    public enum VendorType
    {
        Supplier,
        ServiceProvider,
        Contractor,
        Consultant,
        Other
    }

    public enum VendorStatus
    {
        Active,
        Inactive,
        Suspended,
        Blacklisted
    }
}

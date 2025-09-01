using Microsoft.EntityFrameworkCore;
using Finlab.Models;

namespace Finlab.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<InvoiceReminder> InvoiceReminders { get; set; }
        public DbSet<FinancialTransaction> FinancialTransactions { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
        public DbSet<PaymentSchedule> PaymentSchedules { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<POSTransaction> POSTransactions { get; set; }
        public DbSet<POSTransactionItem> POSTransactionItems { get; set; }
        public DbSet<POSTransactionPayment> POSTransactionPayments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.IsActive).HasDefaultValue(true);

                // Add unique constraints
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Configure FinancialTransaction entity
            modelBuilder.Entity<FinancialTransaction>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TransactionNumber).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Amount).HasPrecision(18, 2);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(500);
                
                // Add unique constraint for transaction number
                entity.HasIndex(e => e.TransactionNumber).IsUnique();
                
                // Configure relationships
                entity.HasOne(e => e.Invoice)
                    .WithMany()
                    .HasForeignKey(e => e.InvoiceId)
                    .OnDelete(DeleteBehavior.SetNull);
                
                entity.HasOne(e => e.Client)
                    .WithMany(c => c.Transactions)
                    .HasForeignKey(e => e.ClientId)
                    .OnDelete(DeleteBehavior.SetNull);
                
                entity.HasOne(e => e.Vendor)
                    .WithMany(v => v.Transactions)
                    .HasForeignKey(e => e.VendorId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure Invoice entity
            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.InvoiceNumber).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Subtotal).HasPrecision(18, 2);
                entity.Property(e => e.TaxAmount).HasPrecision(18, 2);
                entity.Property(e => e.DiscountAmount).HasPrecision(18, 2);
                entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
                
                // Add unique constraint for invoice number
                entity.HasIndex(e => e.InvoiceNumber).IsUnique();
                
                // Configure relationships
                entity.HasOne(e => e.Client)
                    .WithMany(c => c.Invoices)
                    .HasForeignKey(e => e.ClientId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure InvoiceItem entity
            modelBuilder.Entity<InvoiceItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UnitPrice).HasPrecision(18, 2);
                entity.Property(e => e.TotalPrice).HasPrecision(18, 2);
                
                // Configure relationship
                entity.HasOne(e => e.Invoice)
                    .WithMany(i => i.Items)
                    .HasForeignKey(e => e.InvoiceId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure PaymentSchedule entity
            modelBuilder.Entity<PaymentSchedule>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Amount).HasPrecision(18, 2);
                
                // Configure relationship
                entity.HasOne(e => e.Invoice)
                    .WithMany(i => i.PaymentSchedules)
                    .HasForeignKey(e => e.InvoiceId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Client entity
            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CreditLimit).HasPrecision(18, 2);
                
                // Add unique constraint for email
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Configure Vendor entity
            modelBuilder.Entity<Vendor>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CreditLimit).HasPrecision(18, 2);
                
                // Add unique constraint for email
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Configure POSTransaction entity
            modelBuilder.Entity<POSTransaction>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TransactionNumber).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Subtotal).HasPrecision(18, 2);
                entity.Property(e => e.TaxAmount).HasPrecision(18, 2);
                entity.Property(e => e.DiscountAmount).HasPrecision(18, 2);
                entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
                entity.Property(e => e.AmountPaid).HasPrecision(18, 2);
                entity.Property(e => e.ChangeAmount).HasPrecision(18, 2);
                
                // Add unique constraint for transaction number
                entity.HasIndex(e => e.TransactionNumber).IsUnique();
                
                // Configure relationships
                entity.HasOne(e => e.Client)
                    .WithMany()
                    .HasForeignKey(e => e.ClientId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure POSTransactionItem entity
            modelBuilder.Entity<POSTransactionItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UnitPrice).HasPrecision(18, 2);
                entity.Property(e => e.TotalPrice).HasPrecision(18, 2);
                
                // Configure relationship
                entity.HasOne(e => e.POSTransaction)
                    .WithMany(t => t.Items)
                    .HasForeignKey(e => e.POSTransactionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure POSTransactionPayment entity
            modelBuilder.Entity<POSTransactionPayment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Amount).HasPrecision(18, 2);
                
                // Configure relationship
                entity.HasOne(e => e.POSTransaction)
                    .WithMany(t => t.Payments)
                    .HasForeignKey(e => e.POSTransactionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}


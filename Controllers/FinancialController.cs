using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Finlab.Data;
using Finlab.Models;
using System.Security.Claims;

namespace Finlab.Controllers
{
    public class FinancialController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<FinancialController> _logger;

        public FinancialController(ApplicationDbContext context, ILogger<FinancialController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Financial Dashboard
        public async Task<IActionResult> Dashboard()
        {
            try
            {
                var dashboardData = new FinancialDashboardViewModel
                {
                    TotalIncome = await _context.FinancialTransactions
                        .Where(t => t.Type == TransactionType.Income && t.Status == TransactionStatus.Completed)
                        .SumAsync(t => t.Amount),
                    
                    TotalExpenses = await _context.FinancialTransactions
                        .Where(t => t.Type == TransactionType.Expense && t.Status == TransactionStatus.Completed)
                        .SumAsync(t => t.Amount),
                    
                    PendingInvoices = await _context.Invoices
                        .Where(i => i.Status == InvoiceStatus.Sent || i.Status == InvoiceStatus.Viewed)
                        .CountAsync(),
                    
                    OverdueInvoices = await _context.Invoices
                        .Where(i => i.DueDate < DateTime.Today && i.Status != InvoiceStatus.Paid)
                        .CountAsync(),
                    
                    RecentTransactions = await _context.FinancialTransactions
                        .OrderByDescending(t => t.TransactionDate)
                        .Take(10)
                        .ToListAsync(),
                    
                    UpcomingPayments = await _context.PaymentSchedules
                        .Where(p => p.DueDate >= DateTime.Today && p.Status == PaymentStatus.Pending)
                        .OrderBy(p => p.DueDate)
                        .Take(10)
                        .ToListAsync()
                };

                return View(dashboardData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading financial dashboard");
                return View("Error");
            }
        }

        // GET: Income Management
        public async Task<IActionResult> Income()
        {
            var incomeTransactions = await _context.FinancialTransactions
                .Where(t => t.Type == TransactionType.Income)
                .Include(t => t.Client)
                .Include(t => t.Invoice)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();

            return View(incomeTransactions);
        }

        // GET: Expense Management
        public async Task<IActionResult> Expenses()
        {
            var expenseTransactions = await _context.FinancialTransactions
                .Where(t => t.Type == TransactionType.Expense)
                .Include(t => t.Vendor)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();

            return View(expenseTransactions);
        }

        // GET: Create Income Transaction
        public IActionResult CreateIncome()
        {
            ViewBag.Clients = _context.Clients.Where(c => c.IsActive).ToList();
            ViewBag.Categories = Enum.GetValues<TransactionCategory>()
                .Where(c => c == TransactionCategory.Sales || 
                           c == TransactionCategory.Services || 
                           c == TransactionCategory.RentIncome || 
                           c == TransactionCategory.Interest || 
                           c == TransactionCategory.OtherIncome)
                .ToList();

            return View();
        }

        // POST: Create Income Transaction
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateIncome([Bind("TransactionNumber,Category,Amount,Description,ReferenceNumber,TransactionDate,DueDate,PaymentMethod,Notes,ClientId")] FinancialTransaction transaction)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    transaction.Type = TransactionType.Income;
                    transaction.Status = TransactionStatus.Pending;
                    transaction.CreatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    transaction.CreatedDate = DateTime.Now;

                    _context.Add(transaction);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Income transaction created successfully!";
                    return RedirectToAction(nameof(Income));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating income transaction");
                    ModelState.AddModelError("", "An error occurred while creating the transaction.");
                }
            }

            ViewBag.Clients = _context.Clients.Where(c => c.IsActive).ToList();
            ViewBag.Categories = Enum.GetValues<TransactionCategory>()
                .Where(c => c == TransactionCategory.Sales || 
                           c == TransactionCategory.Services || 
                           c == TransactionCategory.RentIncome || 
                           c == TransactionCategory.Interest || 
                           c == TransactionCategory.OtherIncome)
                .ToList();

            return View(transaction);
        }

        // GET: Create Expense Transaction
        public IActionResult CreateExpense()
        {
            ViewBag.Vendors = _context.Vendors.Where(v => v.IsActive).ToList();
            ViewBag.Categories = Enum.GetValues<TransactionCategory>()
                .Where(c => c == TransactionCategory.RentExpense || 
                           c == TransactionCategory.Salaries || 
                           c == TransactionCategory.DeliveryFees || 
                           c == TransactionCategory.Maintenance || 
                           c == TransactionCategory.Utilities || 
                           c == TransactionCategory.Insurance || 
                           c == TransactionCategory.Marketing || 
                           c == TransactionCategory.OfficeSupplies || 
                           c == TransactionCategory.Travel || 
                           c == TransactionCategory.OtherExpenses)
                .ToList();

            return View();
        }

        // POST: Create Expense Transaction
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateExpense([Bind("TransactionNumber,Category,Amount,Description,ReferenceNumber,TransactionDate,DueDate,PaymentMethod,Notes,VendorId")] FinancialTransaction transaction)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    transaction.Type = TransactionType.Expense;
                    transaction.Status = TransactionStatus.Pending;
                    transaction.CreatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    transaction.CreatedDate = DateTime.Now;

                    _context.Add(transaction);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Expense transaction created successfully!";
                    return RedirectToAction(nameof(Expenses));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating expense transaction");
                    ModelState.AddModelError("", "An error occurred while creating the transaction.");
                }
            }

            ViewBag.Vendors = _context.Vendors.Where(v => v.IsActive).ToList();
            ViewBag.Categories = Enum.GetValues<TransactionCategory>()
                .Where(c => c == TransactionCategory.RentExpense || 
                           c == TransactionCategory.Salaries || 
                           c == TransactionCategory.DeliveryFees || 
                           c == TransactionCategory.Maintenance || 
                           c == TransactionCategory.Utilities || 
                           c == TransactionCategory.Insurance || 
                           c == TransactionCategory.Marketing || 
                           c == TransactionCategory.OfficeSupplies || 
                           c == TransactionCategory.Travel || 
                           c == TransactionCategory.OtherExpenses)
                .ToList();

            return View(transaction);
        }

        // GET: Transaction Details
        public async Task<IActionResult> TransactionDetails(int id)
        {
            var transaction = await _context.FinancialTransactions
                .Include(t => t.Client)
                .Include(t => t.Vendor)
                .Include(t => t.Invoice)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Update Transaction Status
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, TransactionStatus status)
        {
            var transaction = await _context.FinancialTransactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            try
            {
                transaction.Status = status;
                transaction.ModifiedDate = DateTime.Now;
                transaction.ModifiedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _context.Update(transaction);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Status updated successfully!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating transaction status");
                return Json(new { success = false, message = "An error occurred while updating the status." });
            }
        }
    }

    public class FinancialDashboardViewModel
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal NetIncome => TotalIncome - TotalExpenses;
        public int PendingInvoices { get; set; }
        public int OverdueInvoices { get; set; }
        public List<FinancialTransaction> RecentTransactions { get; set; } = new();
        public List<PaymentSchedule> UpcomingPayments { get; set; } = new();
    }
}

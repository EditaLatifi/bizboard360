using Microsoft.AspNetCore.Mvc;
using Finlab.Models;
using Finlab.Services;
using System.ComponentModel.DataAnnotations;

namespace Finlab.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly IInvoiceReminderService _reminderService;

        public InvoiceController(IInvoiceReminderService reminderService)
        {
            _reminderService = reminderService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadInvoices([FromForm] InvoiceUploadModel model)
        {
            try
            {
                if (model.Files == null || model.Files.Count == 0)
                {
                    return BadRequest(new { success = false, message = "No files were uploaded." });
                }

                if (string.IsNullOrEmpty(model.ClientName) || model.Amount <= 0)
                {
                    return BadRequest(new { success = false, message = "Client name and amount are required." });
                }

                var uploadedFiles = new List<string>();
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "invoices");

                // Create directory if it doesn't exist
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                foreach (var file in model.Files)
                {
                    if (file.Length > 0)
                    {
                        // Generate unique filename
                        var fileName = $"{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                        var filePath = Path.Combine(uploadPath, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        uploadedFiles.Add(fileName);
                    }
                }

                // Create reminder for the uploaded invoice
                var reminder = new InvoiceReminder
                {
                    InvoiceNumber = $"UPLOAD_{DateTime.Now:yyyyMMddHHmmss}",
                    ClientName = model.ClientName,
                    Amount = model.Amount,
                    DueDate = model.UploadDate.AddDays(30), // Default due date 30 days from upload
                    Status = ReminderStatus.Pending,
                    Priority = model.Amount > 10000 ? ReminderPriority.High : ReminderPriority.Medium,
                    Notes = $"Uploaded invoice(s): {string.Join(", ", uploadedFiles)}\n{model.Notes}",
                    ReminderDate = model.UploadDate,
                    IsActive = true
                };

                await _reminderService.CreateReminderAsync(reminder);

                return Json(new { 
                    success = true, 
                    message = $"Successfully uploaded {uploadedFiles.Count} file(s) for {model.ClientName}",
                    reminderId = reminder.Id,
                    files = uploadedFiles
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Error uploading files: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.ClientName) || model.Amount <= 0)
                {
                    return BadRequest(new { success = false, message = "Client name and amount are required." });
                }

                // Create reminder for the new invoice
                var reminder = new InvoiceReminder
                {
                    InvoiceNumber = $"INV_{DateTime.Now:yyyyMMddHHmmss}",
                    ClientName = model.ClientName,
                    Amount = model.Amount,
                    DueDate = model.DueDate,
                    Status = ReminderStatus.Pending,
                    Priority = model.Amount > 10000 ? ReminderPriority.High : ReminderPriority.Medium,
                    Notes = model.Description,
                    ReminderDate = model.InvoiceDate,
                    IsActive = true
                };

                await _reminderService.CreateReminderAsync(reminder);

                return Json(new { 
                    success = true, 
                    message = $"Invoice created successfully for {model.ClientName}",
                    reminderId = reminder.Id,
                    invoiceNumber = reminder.InvoiceNumber
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Error creating invoice: {ex.Message}" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetInvoices()
        {
            try
            {
                var reminders = await _reminderService.GetActiveRemindersAsync();
                return Json(new { success = true, data = reminders });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Error retrieving invoices: {ex.Message}" });
            }
        }
    }

    public class InvoiceUploadModel
    {
        [Required]
        public string ClientName { get; set; } = string.Empty;
        
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }
        
        [Required]
        public DateTime UploadDate { get; set; }
        
        public string? Notes { get; set; }
        
        [Required]
        public List<IFormFile> Files { get; set; } = new();
    }

    public class CreateInvoiceModel
    {
        [Required]
        public string ClientName { get; set; } = string.Empty;
        
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }
        
        [Required]
        public DateTime InvoiceDate { get; set; }
        
        [Required]
        public DateTime DueDate { get; set; }
        
        public string? Description { get; set; }
    }
}

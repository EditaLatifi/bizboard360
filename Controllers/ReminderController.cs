using Finlab.Models;
using Finlab.Services;
using Microsoft.AspNetCore.Mvc;

namespace Finlab.Controllers
{
    public class ReminderController : Controller
    {
        private readonly IInvoiceReminderService _reminderService;
        
        public ReminderController(IInvoiceReminderService reminderService)
        {
            _reminderService = reminderService;
        }
        
        public async Task<IActionResult> Index()
        {
            var overdue = await _reminderService.GetOverdueInvoicesAsync();
            var dueToday = await _reminderService.GetDueTodayAsync();
            var dueThisWeek = await _reminderService.GetDueThisWeekAsync();
            
            ViewBag.OverdueCount = overdue.Count;
            ViewBag.DueTodayCount = dueToday.Count;
            ViewBag.DueThisWeekCount = dueThisWeek.Count;
            
            return View();
        }
        
        public async Task<IActionResult> GetReminders()
        {
            var reminders = await _reminderService.GetActiveRemindersAsync();
            return Json(reminders);
        }
        
        public async Task<IActionResult> GetOverdue()
        {
            var overdue = await _reminderService.GetOverdueInvoicesAsync();
            return Json(overdue);
        }
        
        public async Task<IActionResult> GetDueToday()
        {
            var dueToday = await _reminderService.GetDueTodayAsync();
            return Json(dueToday);
        }
        
        public async Task<IActionResult> GetDueThisWeek()
        {
            var dueThisWeek = await _reminderService.GetDueThisWeekAsync();
            return Json(dueThisWeek);
        }
        
        [HttpPost]
        public async Task<IActionResult> MarkAsPaid(int id)
        {
            var result = await _reminderService.MarkAsPaidAsync(id);
            return Json(new { success = result });
        }
        
        [HttpPost]
        public async Task<IActionResult> SendReminder(int id)
        {
            var result = await _reminderService.SendReminderAsync(id);
            return Json(new { success = result });
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateReminder([FromBody] InvoiceReminder reminder)
        {
            try
            {
                var created = await _reminderService.CreateReminderAsync(reminder);
                return Json(new { success = true, data = created });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        
        public async Task<IActionResult> GetReminderCount()
        {
            var count = await _reminderService.GetReminderCountAsync();
            return Json(new { count = count });
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace Finlab.Controllers
{
	public class AjaxController : Controller
	{
		public IActionResult Activity()
		{
			return View();
		}
		public IActionResult EventAgenda()
		{
			return View();
		}
		public IActionResult FeaturedCompanies()
		{
			return View();
		}
		public IActionResult Message()
		{
			return View();
		}
		public IActionResult RecentActivity()
		{
			return View();
		}
	}
}

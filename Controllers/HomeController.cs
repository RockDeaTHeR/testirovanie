using Microsoft.AspNetCore.Mvc;

namespace testirovanie.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}

using Microsoft.AspNetCore.Mvc;

namespace HeartmadeCandles.API.Controllers
{
    public class CandleConstructorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.Api;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
//        private readonly IValuesService _ValuesService;
//
//        public HomeController(IValuesService valuesService) => _ValuesService = valuesService;
        
        public IActionResult Index() => View();
         
        public IActionResult ContactUs() => View();

        public IActionResult Cart() => View();

        public IActionResult BlogSingle() => View();

        public IActionResult Blog() => View();

        public IActionResult ErrorPage404() => View();

        public IActionResult ValuesServiceTest([FromServices] IValuesService valuesService) => View(valuesService.Get());
    }
}
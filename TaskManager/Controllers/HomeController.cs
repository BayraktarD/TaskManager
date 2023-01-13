using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TaskManager.Models;

//
namespace TaskManager.Controllers
{
    public class HomeController : Controller
    {
        private Repository.EmployeeRepository _employeeRepository;

        private readonly ILogger<HomeController> _logger;


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        


        public IActionResult Index()
        {
            //bool employeeData = _employeeRepository.GetAllEmployees() != null ? true : false;   
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
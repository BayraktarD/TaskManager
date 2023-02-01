using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using System.Diagnostics;
using TaskManager.Data;
using TaskManager.Models;
using TaskManager.Repository;

//
namespace TaskManager.Controllers
{
    public class HomeController : Controller
    {
        private Repository.EmployeeRepository _employeeRepository;
        private Repository.TaskRepository _taskRepository;
        private readonly IConfiguration _configuration;


        private readonly ILogger<HomeController> _logger;

        private EmployeeModel LoggedEmployee { get; set; }


        public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _logger = logger;
            _taskRepository = new Repository.TaskRepository(dbContext);
            _employeeRepository = new Repository.EmployeeRepository(dbContext, configuration);

        }


        private EmployeeModel GetLoggedEmployee()
        {
            var userClaimsArray = User.Claims.Select(x => x.Value).ToArray();
            if (userClaimsArray.Length > 0)
            {

                var loggedUser = User.Claims.Select(x => x.Value).ToArray()[0];
                Guid.TryParse(loggedUser, out Guid userId);
                LoggedEmployee = _employeeRepository.GetEmployeeByUserId(userId);
                ViewBag.EmployeeId = LoggedEmployee.IdEmployee;

            }
            return LoggedEmployee;

        }


        public IActionResult Index()
        {
            var userClaimsArray = User.Claims.Select(x => x.Value).ToArray();

            if (userClaimsArray.Length > 0)
            {
                var tasks = _taskRepository.GetAllEmployeeTasks(GetLoggedEmployee().IdEmployee);

                return View("Index", tasks);
            }
            else
            {
                var demoAccounts = _employeeRepository.GetAllDemoEmployees().Result;
                return View("IndexDemo",demoAccounts);

            }

        }

        public ActionResult Details(Guid id)
        {
            var task = _taskRepository.GetTasksById(id);
            return View("~/Views/Task/TaskDetails.cshtml", task);
        }

        public ActionResult ViewProfile(Guid id)
        {
            var user = _employeeRepository.GetEmployeeById(id);
            return View("~/Views/Employee/EmployeeDetails.cshtml", user);
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
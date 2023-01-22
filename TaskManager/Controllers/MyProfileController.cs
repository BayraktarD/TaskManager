using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using TaskManager.Data;
using TaskManager.Models;
using TaskManager.Repository;

namespace TaskManager.Controllers
{
    public class MyProfileController : Controller
    {
        private Repository.EmployeeRepository _repository;
        private readonly IConfiguration _configuration;

        public MyProfileController(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _configuration = configuration;
            _repository = new Repository.EmployeeRepository(dbContext,_configuration);
           
        }

        public EmployeeModel LoggedEmployee { get; set; }


        private EmployeeModel GetLoggedEmployee()
        {
            var loggedUser = User.Claims.Select(x => x.Value).ToArray()[0];
            Guid.TryParse(loggedUser, out Guid userId);
            LoggedEmployee = _repository.GetEmployeeByUserId(userId);
            return LoggedEmployee;
        }


        // GET: MyProfileController
        public ActionResult Index()
        {
            return View();
        }

        // GET: MyProfileController/TaskDetails/5
        public ActionResult Details()
        {
            var user = _repository.GetEmployeeById(GetLoggedEmployee().IdEmployee);
            return View("MyProfileDetails", user);
        }

        // GET: MyProfileController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MyProfileController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MyProfileController/Edit/5
        public ActionResult Edit(Guid id)
        {
            return RedirectToAction("Edit", "Employee", new { id = GetLoggedEmployee().IdEmployee });
        }

        // POST: MyProfileController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MyProfileController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MyProfileController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

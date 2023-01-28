using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Protocol.Core.Types;
using PagedList;
using TaskManager.Data;
using TaskManager.Models;
using TaskManager.Models.DBObjects;
using TaskManager.Repository;

namespace TaskManager.Controllers
{
    public class EmployeeController : Controller
    {
        private Repository.EmployeeRepository _repository;
        private Repository.DepartmentRepository _departmentRepository;
        private Repository.JobTitleRepository _jobTitlesRepository;

        private readonly IConfiguration _configuration;





        public EmployeeController(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _configuration = configuration;
            _repository = new Repository.EmployeeRepository(dbContext, _configuration);
            _departmentRepository = new Repository.DepartmentRepository(dbContext);
            _jobTitlesRepository = new Repository.JobTitleRepository(dbContext);
        }

        protected EmployeeModel LoggedEmployee { get; set; }

        private async Task<EmployeeModel> GetLoggedEmployee()
        {
            var loggedUser = User.Claims.Select(x => x.Value).ToArray()[0];
            Guid.TryParse(loggedUser, out Guid userId);
            LoggedEmployee = _repository.GetEmployeeByUserId(userId);
            return await System.Threading.Tasks.Task.FromResult(LoggedEmployee);
        }

        // GET: EmployeeController
        public async Task<ActionResult> Index()
        {
            await GetLoggedEmployee();
            await GetPermissions();
            var employees = _repository.GetAllEmployees(LoggedEmployee.IdEmployee).Result;

            return View("Index", employees);
        }

        public ActionResult Index1()
        {
            return View("Index1");
        }

        private async Task<bool> GetPermissions()
        {
            GetLoggedEmployee();
            ViewBag.CanCreate = LoggedEmployee.CanCreateProfiles;
            ViewBag.CanEdit = LoggedEmployee.CanModifyProfiles;
            ViewBag.CanDelete = LoggedEmployee.CanDeleteProfiles;
            return true;

        }

        // GET: EmployeeController/TaskDetails/5
        public ActionResult Details(Guid id)
        {
            var user = _repository.GetEmployeeById(id);
            return View("EmployeeDetails", user);
        }

        // GET: EmployeeController/Create
        public ActionResult Create()
        {
            SelectCategory(null, null, null, null);
            return View("EmployeeCreate");
        }

        // POST: EmployeeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            try
            {
                Models.EmployeeModel model = new Models.EmployeeModel();

                Guid jobTitle;
                Guid department;
                GetPermissions();
                string jobTypesListSelectedValue, departemntsListSelectedValue;

                jobTypesListSelectedValue = Request.Form["ddlJobsList"].ToString();
                departemntsListSelectedValue = Request.Form["ddlDepartmentsList"].ToString();

                if (Guid.TryParse(jobTypesListSelectedValue, out jobTitle)
                    && Guid.TryParse(departemntsListSelectedValue, out department)
                    )
                {

                    model.JobTitle = jobTitle;
                    model.Department = department;

                    model.JobTitleString = jobTitle.ToString();
                    model.DepartmentString = department.ToString();


                    var task = TryUpdateModelAsync(model);
                    task.Wait();
                    if (task.Result)
                    {
                        var idEmployee = await _repository.InsertEmployee(model);

                    }
                    else
                    {
                        TempData["creationFailed"] = @"The employee profile was not created due to an error (CODE:0N54\/3_C)!";

                        return View("Index");
                    }

                    TempData["createdWithSuccess"] = "The employee profile was created with success!";
                    return RedirectToAction("Index");

                }
                else
                {
                    TempData["creationFailed"] = @"The employee profile was not created due to an error (CODE:0NDD1D39J0B_C)!";

                    return View("Index");
                };

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }




        // GET: EmployeeController/Edit/5
        public ActionResult Edit(Guid id)
        {
            Models.EmployeeModel model = _repository.GetEmployeeById(id);
            SelectCategory(model.DepartmentString, model.Department.ToString(), model.JobTitleString, model.JobTitle.ToString());
            return View("EmployeeEdit", model);
        }

        // POST: EmployeeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, IFormCollection collection)
        {
            try
            {
                Models.EmployeeModel model = new Models.EmployeeModel();

                Guid jobTitle, department;
                string jobTypesListSelectedValue, departemntsListSelectedValue;


                jobTypesListSelectedValue = Request.Form["ddlJobsList"].ToString();
                departemntsListSelectedValue = Request.Form["ddlDepartmentsList"].ToString();

                if (Guid.TryParse(jobTypesListSelectedValue, out jobTitle)
                    && Guid.TryParse(departemntsListSelectedValue, out department)
                    )
                {
                    model.JobTitle = jobTitle;
                    model.Department = department;

                    var task = TryUpdateModelAsync(model);
                    task.Wait();
                    if (task.Result)
                    {
                        _repository.UpdateEmployee(model);
                        TempData["modifiedWithSuccess"] = "The employee data was modified with success!";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["employeeModificationFailed"] = @"The employee data was not modified due to an error (CODE:0N54\/3_E)!";
                        return RedirectToAction("Index", id);
                    }

                }
                else
                {
                    TempData["employeeModificationFailed"] = @"The employee profile was not created due to an error (CODE:0NDD1D39J0B_E)!";

                    return RedirectToAction("Index", id);
                };
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


        // GET: EmployeeController/Delete/5
        public ActionResult Delete(Guid id)
        {
            EmployeeModel model;
            bool hasTasks = _repository.UserHasTasksToAccomplish(id);
            if (hasTasks)
            {
                TempData["alertMsg"] = "You can't delete this employee. He has tasks to accomplish!";
                return RedirectToAction("Index");
            }
            else
            {
                model = _repository.GetEmployeeById(id);
                return View("EmployeeDelete", model);
            }
        }

        // POST: EmployeeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, IFormCollection collection)
        {
            try
            {
                _repository.DeleteEmployee(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("EmployeeDelete", id);
            }
        }



        public async Task<IActionResult> SelectCategory(string departmentText, string departmentValue, string jobTitleText, string jobTitleValue)
        {

            //Populate departments ddl
            List<SelectListItem> departmentsList = new List<SelectListItem>();


            foreach (var item in _departmentRepository.GetAllDepartments().OrderBy(x => x.Department1))
            {
                departmentsList.Add(new SelectListItem
                {
                    Text = item.Department1.ToString(),
                    Value = item.IdDepartment.ToString()
                });
            }
            ViewBag.DepartmentsList = departmentsList;

            if (departmentText != null && departmentValue != null)
            {
                var selectedDepartmentIndex = departmentsList.IndexOf(departmentsList.Where(x => x.Value == departmentValue).FirstOrDefault());
                ViewBag.SelectedDepartmentIndex = selectedDepartmentIndex + 1;
            }

            //Populate job titles ddl
            List<SelectListItem> jobTitlesList = new List<SelectListItem>();

            foreach (var item in _jobTitlesRepository.GetAllJobTitles().OrderBy(x => x.JobTitle1))
            {
                jobTitlesList.Add(new SelectListItem
                {
                    Text = item.JobTitle1.ToString(),
                    Value = item.IdJobTitle.ToString()
                });
            }

            ViewBag.JobTitlesList = jobTitlesList;

            if (jobTitleText != null && jobTitleValue != null)
            {
                var selectedJobTitleIndex = jobTitlesList.IndexOf(jobTitlesList.Where(x => x.Value == jobTitleValue).FirstOrDefault());
                ViewBag.SelectedJobTitleIndex = selectedJobTitleIndex + 1;
            }


            List<SelectListItem> employees = new List<SelectListItem>();

            var result = GetLoggedEmployee().Result;
            foreach (var item in await _repository.GetAllEmployees(result.IdEmployee))
            {
                employees.Add(new SelectListItem
                {
                    Text = item.Name + item.Surname,
                    Value = item.IdEmployee.ToString()
                });
            }

            ViewBag.Employees = employees;
            return Ok();
        }

    }
}

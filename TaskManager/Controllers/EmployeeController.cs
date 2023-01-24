using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Protocol.Core.Types;
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

        public EmployeeModel LoggedEmployee { get; set; }

        private EmployeeModel GetLoggedEmployee()
        {
            var loggedUser = User.Claims.Select(x => x.Value).ToArray()[0];
            Guid.TryParse(loggedUser, out Guid userId);
            LoggedEmployee = _repository.GetEmployeeByUserId(userId);
            return LoggedEmployee;
        }

        // GET: EmployeeController
        public ActionResult Index()
        {
            GetLoggedEmployee();
            GetPermissions();
            var employees = _repository.GetAllEmployees(LoggedEmployee.IdEmployee);
            return View("Index", employees);
        }

        private void GetPermissions()
        {
            ViewBag.CanCreate = LoggedEmployee.CanCreateProfiles;
            ViewBag.CanEdit = LoggedEmployee.CanModifyProfiles;
            ViewBag.CanDelete = LoggedEmployee.CanDeleteProfiles;
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
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                Models.EmployeeModel model = new Models.EmployeeModel();

                Guid jobTitle;
                Guid department;

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
                        _repository.InsertEmployee(model);
                        return RedirectToAction("Index");
                    }

                    SelectCategory(null, null, null, null);

                    return View("EmployeeCreate");

                }
                else
                {
                    SelectCategory(null, null, null, null);
                    return View("EmployeeCreate");
                };

            }
            catch
            {
                SelectCategory(null, null, null, null);
                return View("EmployeeCreate");
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


                jobTypesListSelectedValue = Request.Form["JobTitlesList"].ToString();
                departemntsListSelectedValue = Request.Form["DepartmentsList"].ToString();

                if (Guid.TryParse(jobTypesListSelectedValue, out jobTitle)
                    && Guid.TryParse(departemntsListSelectedValue, out department)
                    )
                {
                    model.JobTitle = jobTitle;
                    model.Department = department;
                }
                else
                {
                    return RedirectToAction("Index", id);
                };

                var task = TryUpdateModelAsync(model);
                task.Wait();
                if (task.Result)
                {
                    _repository.UpdateEmployee(model);
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index", id);
                }
            }
            catch
            {
                return RedirectToAction("Index", id);
            }
        }


        // GET: EmployeeController/Delete/5
        public ActionResult Delete(Guid id)
        {
            var model = _repository.GetEmployeeById(id);
            return View("EmployeeDelete", model);
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



        public ActionResult SelectCategory(string departmentText, string departmentValue, string jobTitleText, string jobTitleValue)
        {

            //Populate departments ddl
            List<SelectListItem> departmentsList = new List<SelectListItem>();

            if (departmentText != null && departmentValue != null)
            {
                departmentsList.Add(new SelectListItem
                {
                    Text = departmentText,
                    Value = departmentValue
                });
            }

            foreach (var item in _departmentRepository.GetAllDepartments().Where(x => x.IdDepartment.ToString() != departmentValue).OrderBy(x => x.Department1))
            {
                departmentsList.Add(new SelectListItem
                {
                    Text = item.Department1.ToString(),
                    Value = item.IdDepartment.ToString()
                });
            }

            ViewBag.DepartmentsList = departmentsList;

            //Populate job titles ddl
            List<SelectListItem> jobTitlesList = new List<SelectListItem>();

            if (jobTitleText != null && jobTitleValue != null)
            {
                jobTitlesList.Add(new SelectListItem
                {
                    Text = jobTitleText,
                    Value = jobTitleValue
                });
            }

            foreach (var item in _jobTitlesRepository.GetAllJobTitles().Where(x => x.IdJobTitle.ToString() != jobTitleValue).OrderBy(x => x.JobTitle1))
            {
                jobTitlesList.Add(new SelectListItem
                {
                    Text = item.JobTitle1.ToString(),
                    Value = item.IdJobTitle.ToString()
                });
            }

            ViewBag.JobTitlesList = jobTitlesList;


            List<SelectListItem> employees = new List<SelectListItem>();

            foreach (var item in _repository.GetAllEmployees(GetLoggedEmployee().IdEmployee))
            {
                employees.Add(new SelectListItem
                {
                    Text = item.Name + item.Surname,
                    Value = item.IdEmployee.ToString()
                });
            }

            ViewBag.Employees = employees;
            return View();
        }

    }
}

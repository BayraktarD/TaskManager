using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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




        public EmployeeController(ApplicationDbContext dbContext)
        {
            _repository = new Repository.EmployeeRepository(dbContext);
            _departmentRepository = new Repository.DepartmentRepository(dbContext);
            _jobTitlesRepository = new Repository.JobTitleRepository(dbContext);

        }



        // GET: EmployeeController
        public ActionResult Index()
        {
            var users = _repository.GetAllEmployees();
            return View("Index", users);
        }

        // GET: EmployeeController/Details/5
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

                string  ddlJobTitles, ddlDepartments;

                ddlJobTitles = "JobTitlesList";
                ddlDepartments = "DepartmentsList";


                GetGuidFromDdl(collection, ddlJobTitles, out jobTypesListSelectedValue);
                GetGuidFromDdl(collection, ddlDepartments, out departemntsListSelectedValue);


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
                        _repository.InsertUser(model);
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("EmployeeCreate");
                };

            }
            catch
            {
                return RedirectToAction("EmployeeCreate");
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
                Guid  jobTitle, department;
                string  jobTypesListSelectedValue, departemntsListSelectedValue;

                string  ddlJobTitles, ddlDepartments;

                ddlJobTitles = "JobTitlesList";
                ddlDepartments = "DepartmentsList";


                GetGuidFromDdl(collection, ddlJobTitles, out jobTypesListSelectedValue);
                GetGuidFromDdl(collection, ddlDepartments, out departemntsListSelectedValue);

                if ( Guid.TryParse(jobTypesListSelectedValue, out jobTitle)
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
                    _repository.UpdateUser(model);
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

        private static void GetGuidFromDdl(IFormCollection collection, string ddlName, out string output)
        {
            output = collection.FirstOrDefault(x => x.Key == ddlName).ToString();
            output = GetDdlValue(output);
        }

        private static string GetDdlValue(string input)
        {
            string output = input
                .Substring(input.IndexOf(',') + 1,
                            input.IndexOf(']') - input.Substring(0, input.IndexOf(',')).Length - 1).Trim();
            return output;
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
                _repository.DeleteUser(id);
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

            if (departmentText == null && departmentValue == null)
            {
                departmentsList.Add(new SelectListItem
                {
                    Text = "---Select Department---",
                    Value = "-1"
                });
            }
            else
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

            if (jobTitleText == null && jobTitleValue == null)
            {
                jobTitlesList.Add(new SelectListItem
                {
                    Text = "---Select Job Title---",
                    Value = "-1"
                });
            }
            else
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
            return View();
        }
    }
}

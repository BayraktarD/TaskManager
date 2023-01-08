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
    public class UserController : Controller
    {
        private Repository.UserRepository _repository;
        private Repository.DepartmentRepository _departmentRepository;
        private Repository.JobTitleRepository _jobTitlesRepository;
        private Repository.UserTypeRepository _userTypeRepository;




        public UserController(ApplicationDbContext dbContext)
        {
            _repository = new Repository.UserRepository(dbContext);
            _departmentRepository = new Repository.DepartmentRepository(dbContext);
            _jobTitlesRepository = new Repository.JobTitleRepository(dbContext);
            _userTypeRepository = new Repository.UserTypeRepository(dbContext);

        }



        // GET: UserController
        public ActionResult Index()
        {
            var users = _repository.GetAllUsers();
            return View("Index", users);
        }

        // GET: UserController/Details/5
        public ActionResult Details(Guid id)
        {
            var user = _repository.GetUserById(id);
            return View("UserDetails", user);
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            SelectCategory(null, null, null, null, null, null);
            return View("UserCreate");
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                Models.UserModel model = new Models.UserModel();

                Guid userType;
                Guid jobTitle;
                Guid department;

                string userTypesListSelectedValue, jobTypesListSelectedValue, departemntsListSelectedValue;

                string ddlUserTypes, ddlJobTitles, ddlDepartments;

                ddlUserTypes = "UserTypesList";
                ddlJobTitles = "JobTitlesList";
                ddlDepartments = "DepartmentsList";


                GetGuidFromDdl(collection, ddlUserTypes, out userTypesListSelectedValue);
                GetGuidFromDdl(collection, ddlJobTitles, out jobTypesListSelectedValue);
                GetGuidFromDdl(collection, ddlDepartments, out departemntsListSelectedValue);


                if (Guid.TryParse(userTypesListSelectedValue, out userType)
                    && Guid.TryParse(jobTypesListSelectedValue, out jobTitle)
                    && Guid.TryParse(departemntsListSelectedValue, out department)
                    )
                {
                    model.UserType = userType;
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
                    return RedirectToAction("UserCreate");
                };

            }
            catch
            {
                return RedirectToAction("UserCreate");
            }
        }




        // GET: UserController/Edit/5
        public ActionResult Edit(Guid id)
        {
            Models.UserModel model = _repository.GetUserById(id);
            SelectCategory(model.DepartmentString, model.Department.ToString(), model.JobTitleString, model.JobTitle.ToString(), model.UserTypeString, model.UserType.ToString());
            return View("UserEdit", model);
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, IFormCollection collection)
        {
            try
            {
                Models.UserModel model = new Models.UserModel();
                Guid userType, jobTitle, department;
                string userTypesListSelectedValue, jobTypesListSelectedValue, departemntsListSelectedValue;

                string ddlUserTypes, ddlJobTitles, ddlDepartments;

                ddlUserTypes = "UserTypesList";
                ddlJobTitles = "JobTitlesList";
                ddlDepartments = "DepartmentsList";


                GetGuidFromDdl(collection, ddlUserTypes, out userTypesListSelectedValue);
                GetGuidFromDdl(collection, ddlJobTitles, out jobTypesListSelectedValue);
                GetGuidFromDdl(collection, ddlDepartments, out departemntsListSelectedValue);

                if (Guid.TryParse(userTypesListSelectedValue, out userType)
                    && Guid.TryParse(jobTypesListSelectedValue, out jobTitle)
                    && Guid.TryParse(departemntsListSelectedValue, out department)
                    )
                {
                    model.UserType = userType;
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

        // GET: UserController/Delete/5
        public ActionResult Delete(Guid id)
        {
            var model = _repository.GetUserById(id);
            return View("UserDelete", model);
        }

        // POST: UserController/Delete/5
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
                return RedirectToAction("UserDelete", id);
            }
        }

        public ActionResult SelectCategory(string departmentText, string departmentValue, string jobTitleText, string jobTitleValue, string userTypeText, string userTypeValue)
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


            //Populate user types ddl
            List<SelectListItem> userTypesList = new List<SelectListItem>();

            if (userTypeText == null && userTypeValue == null)
            {

                userTypesList.Add(new SelectListItem
                {
                    Text = "---Select User Type---",
                    Value = "-1"
                });
            }
            else
            {
                userTypesList.Add(new SelectListItem
                {
                    Text = userTypeText,
                    Value = userTypeValue
                });
            };

            foreach (var item in _userTypeRepository.GetUserTypes().Where(x => x.IdUserType.ToString() != userTypeValue).OrderBy(x => x.UserType1))
            {
                userTypesList.Add(new SelectListItem
                {
                    Text = item.UserType1.ToString(),
                    Value = item.IdUserType.ToString()
                });
            }

            ViewBag.UserTypesList = userTypesList;

            return View();
        }
    }
}

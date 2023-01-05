using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol.Core.Types;
using TaskManager.Data;
using TaskManager.Models;
using TaskManager.Repository;

namespace TaskManager.Controllers
{
    public class UserController : Controller
    {
        private Repository.UserRepository _repository;
        private Repository.DepartmentRepository _departmentRepository;
        private Repository.JobTitleRepository _jobTitlesRepository;



        public UserController(ApplicationDbContext dbContext)
        {
            _repository = new Repository.UserRepository(dbContext);
            _departmentRepository = new Repository.DepartmentRepository(dbContext);
            _jobTitlesRepository = new Repository.JobTitleRepository(dbContext);
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
            SelectCategory();
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
                var task = TryUpdateModelAsync(model);
                task.Wait();
                if (task.Result)
                {
                    _repository.InsertUser(model);
                }
                return RedirectToAction("Index");
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

        public ActionResult SelectCategory()
        {

            //Populate departments ddl
            List<SelectListItem> departmentsList = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "---Select Department---",
                    Value = "-1"
                }
            };

            foreach (var item in _departmentRepository.GetAllDepartments())
            {
                departmentsList.Add(new SelectListItem
                {
                    Text = item.Department1.ToString(),
                    Value = item.IdDepartment.ToString()
                });
            }

            ViewBag.DepartmentsList = departmentsList;

            //Populate job titles ddl
            List<SelectListItem> jobTitlesList = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "---Select Job Title---",
                    Value = "-1"
                }
            };

            foreach (var item in _jobTitlesRepository.GetAllJobTitles())
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

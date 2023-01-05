using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManager.Data;
using TaskManager.Models;
using TaskManager.Repository;

namespace TaskManager.Controllers
{
    public class TaskController : Controller
    {
        private Repository.TaskRepository _repository;

        private Repository.UserRepository _userRepository;


        public TaskController(ApplicationDbContext dbContext)
        {
            _repository = new Repository.TaskRepository(dbContext);

            _userRepository = new Repository.UserRepository(dbContext);
        }


        // GET: TaskController
        public ActionResult Index()
        {
            var taskList = _repository.GetAllTasks();
            return View("Index", taskList);
        }

        // GET: TaskController/Details/5
        public ActionResult Details(Guid id)
        {
            var task = _repository.GetTasksById(id);
            return View("TaskDetails",task);
        }

        // GET: TaskController/Create
        public ActionResult Create()
        {
            SelectCategory();
            return View("TaskCreate");
        }

        // POST: TaskController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                Models.TaskModel taskModel = new Models.TaskModel();

                var task = TryUpdateModelAsync(taskModel);
                task.Wait();

                if (task.Result)
                {
                    _repository.InsertTask(taskModel);
                }

                return View("Index");
            }
            catch
            {
                return View("TaskCreate");

            }
        }

        // GET: TaskController/Edit/5
        public ActionResult Edit(Guid id)
        {
            var model = _repository.GetTasksById(id);
            return View("TaskEdit", model);
        }

        // POST: TaskController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                Models.TaskModel model = new Models.TaskModel();
                var task = TryUpdateModelAsync(model);
                task.Wait();
                if (task.Result)
                {
                    _repository.UpdateTask(model);
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

        // GET: TaskController/Delete/5
        public ActionResult Delete(Guid id)
        {
            var model = _repository.GetTasksById(id);
            return View("TaskDelete",model);
        }

        // POST: TaskController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, IFormCollection collection)
        {
            try
            {
                _repository.DeleteTask(id);
                return RedirectToAction("Index");
            }
            catch
            {
            return View("TaskDelete",id);
            }
        }

        public ActionResult SelectCategory()
        {

            List<SelectListItem> userList = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "---Select User---",
                    Value = "-1"
                }
            };

            foreach (var item in _userRepository.GetAllUsers())
            {
                userList.Add(new SelectListItem
                {
                    Text = item.IdUser.ToString(),
                    Value = item.Username
                });
            }

            ViewBag.UsersList = userList;
            return View();
        }
    }
}

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

        private Repository.EmployeeRepository _employeeRepository;


        public TaskController(ApplicationDbContext dbContext)
        {
            _repository = new Repository.TaskRepository(dbContext);

            _employeeRepository = new Repository.EmployeeRepository(dbContext);
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
            return View("TaskDetails", task);
        }

        // GET: TaskController/Create
        public ActionResult Create()
        {
            SelectCategory(null, null);
            return View("TaskCreate");
        }

        // POST: TaskController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                Models.TaskModel model = new Models.TaskModel();

                Guid assignedTo;

                var loggedUser = User.Claims.Select(x => x.Value).ToArray()[0];

                if (Guid.TryParse(loggedUser, out Guid userId))
                {
                    model.CreatedById = _employeeRepository.GetEmployeeByUserId(userId).IdEmployee;


                    string userListAssignedToSelectedValue = Request.Form["UsersList"].ToString();

                    if (Guid.TryParse(userListAssignedToSelectedValue, out assignedTo))
                    {
                        model.AssignedToId = assignedTo;



                        model.CreationDate = DateTime.Now.Date;

                        var task = TryUpdateModelAsync(model);
                        task.Wait();

                        if (task.Result)
                        {
                            _repository.InsertTask(model);
                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        SelectCategory(null, null);
                        return View("TaskCreate");

                    }

                }
                else
                {
                    SelectCategory(null, null);
                    return View("TaskCreate");
                }

            }
            catch
            {
                SelectCategory(null, null);
                return View("TaskCreate");

            }
        }

        // GET: TaskController/Edit/5
        public ActionResult Edit(Guid id)
        {
            var model = _repository.GetTasksById(id);
            SelectCategory(model.AssignedToString, model.AssignedToId.ToString());
            return View("TaskEdit", model);
        }

        // POST: TaskController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, IFormCollection collection)
        {
            try
            {
                Models.TaskModel model = new Models.TaskModel();

                var loggedUser = User.Claims.Select(x => x.Value).ToArray()[0];

                string userListAssignedToSelectedValue;

                Guid assignedTo;

                userListAssignedToSelectedValue = Request.Form["UsersList"].ToString();

                if (Guid.TryParse(userListAssignedToSelectedValue, out assignedTo) && Guid.TryParse(loggedUser, out Guid userId))
                {
                    model.ModificationDate = DateTime.Now;
                    model.AssignedToId = assignedTo;
                    model.ModifiedById = _employeeRepository.GetEmployeeByUserId(userId).IdEmployee;

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

        // GET: TaskController/Delete/5
        public ActionResult Delete(Guid id)
        {
            var model = _repository.GetTasksById(id);
            return View("TaskDelete", model);
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
                return View("TaskDelete", id);
            }
        }

        public ActionResult SelectCategory(string employeeText, string employeeValue)
        {

            List<SelectListItem> userList = new List<SelectListItem>();
            if (employeeText == null && employeeValue == null)
            {
                userList.Add(new SelectListItem
                {
                    Text = "---Select User---",
                    Value = "-1"
                });
            }
            else
            {
                userList.Add(new SelectListItem
                {
                    Text = employeeText,
                    Value = employeeValue
                });
            }

            foreach (var item in _employeeRepository.GetAllEmployees().Where(x => x.IdEmployee.ToString() != employeeValue))
            {
                userList.Add(new SelectListItem
                {
                    Text = item.Name.ToString() + " " + item.Surname.ToString(),
                    Value = item.IdEmployee.ToString()
                });
            }

            ViewBag.UsersList = userList;
            return View();
        }
    }
}

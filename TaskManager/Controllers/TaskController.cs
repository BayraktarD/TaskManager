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


                // trebuie dezvoltat dupa ce se creaza login-ul
                Guid.TryParse("3f52c669-1282-4276-926d-a5624133069d", out Guid hardCodeCreatedById);

                model.CreatedById = hardCodeCreatedById;
                //------------------------------------------------------------------------------------


                string userListAssignedToSelectedValue;
                string ddlUserListName = "UsersList";

                GetGuidFromDdl(collection, ddlUserListName, out userListAssignedToSelectedValue);


                if (Guid.TryParse(userListAssignedToSelectedValue, out assignedTo))
                {
                    model.AssignedToId = assignedTo;

                    //model.AssignedToString = "";
                    //model.ModificationDate = model.EndDate;
                    //model.ModifiedById = assignedTo;

                    model.CreationDate = DateTime.Now.Date;

                    var task = TryUpdateModelAsync(model);
                    task.Wait();

                    if (task.Result)
                    {
                        _repository.InsertTask(model);
                    }
                    return View("Index");

                }
                else
                {
                    return View("TaskCreate");
                }

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

                Guid assignedTo;

                string userListAssignedToSelectedValue;
                string ddlUserListName = "UsersList";

                GetGuidFromDdl(collection, ddlUserListName, out userListAssignedToSelectedValue);


                if (Guid.TryParse(userListAssignedToSelectedValue, out assignedTo))
                {
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

        public ActionResult SelectCategory(string userText, string userValue)
        {

            List<SelectListItem> userList = new List<SelectListItem>();
            if (userText == null && userValue == null)
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
                    Text = userText,
                    Value = userValue
                });
            }

            foreach (var item in _userRepository.GetAllUsers().Where(x => x.IdUser.ToString() != userValue))
            {
                userList.Add(new SelectListItem
                {
                    Text = item.Username.ToString(),
                    Value = item.IdUser.ToString()
                });
            }

            ViewBag.UsersList = userList;
            return View();
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManager.Data;
using TaskManager.Models;
using TaskManager.Repository;
using System.Web;
using TaskManager.Models.DBObjects;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace TaskManager.Controllers
{
    public class TaskController : Controller
    {
        private Repository.TaskRepository _repository;

        private Repository.EmployeeRepository _employeeRepository;

        private Repository.TaskAttachmentRepository _taskAttachmentsRepository;



        public TaskController(ApplicationDbContext dbContext)
        {
            _repository = new Repository.TaskRepository(dbContext);

            _employeeRepository = new Repository.EmployeeRepository(dbContext);

            _taskAttachmentsRepository = new Repository.TaskAttachmentRepository(dbContext);

        }

        public EmployeeModel LoggedEmployee { get; set; }


        private EmployeeModel GetLoggedEmployee()
        {
            var loggedUser = User.Claims.Select(x => x.Value).ToArray()[0];
            Guid.TryParse(loggedUser, out Guid userId);
            LoggedEmployee = _employeeRepository.GetEmployeeByUserId(userId);

            return LoggedEmployee;

        }

        // GET: TaskController
        public ActionResult Index()
        {
            GetLoggedEmployee();
            GetPermissions();

            var tasks = _repository.GetAllEmployeeTasks(LoggedEmployee.IdEmployee);
            ViewBag.EmployeeId = LoggedEmployee.IdEmployee;
            return View("Index", tasks);
        }

        private void GetPermissions()
        {
            ViewBag.CanCreate = LoggedEmployee.CanCreateTasks;
            ViewBag.CanEdit = LoggedEmployee.CanModifyTasks;
            ViewBag.CanDelete = LoggedEmployee.CanDeleteTasks;
        }


        // GET: TaskController/TaskDetails/5
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
        public ActionResult Create(IFormCollection collection, List<IFormFile> files)
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
                    }
                    else
                    {
                        model.AssignedToId = model.CreatedById;
                    }

                    model.CreationDate = DateTime.Now.Date;

                    var task = TryUpdateModelAsync(model);
                    task.Wait();

                    if (task.Result)
                    {
                        Guid idTask = _repository.InsertTask(model);

                        foreach (var file in files)
                        {
                            Models.TaskAttachmentModel taskAttachmentModel = new Models.TaskAttachmentModel();
                            var output = Upload(file);

                            taskAttachmentModel.IdTask = idTask;
                            taskAttachmentModel.Attachment = output;
                            taskAttachmentModel.AttachmentName = file.FileName;

                            task = TryUpdateModelAsync(taskAttachmentModel);
                            task.Wait();
                            if (task.Result)
                            {
                                _taskAttachmentsRepository.InsertTaskAttachment(taskAttachmentModel);
                            }
                            else
                            {
                                return RedirectToAction("Index");

                            }
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
        public ActionResult Edit(Guid id, IFormCollection collection, List<IFormFile> files)
        {
            try
            {
                Models.TaskModel model = new Models.TaskModel();

                var loggedUser = User.Claims.Select(x => x.Value).ToArray()[0];

                string userListAssignedToSelectedValue;

                Guid assignedTo;

                userListAssignedToSelectedValue = Request.Form["UsersList"].ToString();

                if (Guid.TryParse(loggedUser, out Guid userId))
                {

                    model.ModifiedById = _employeeRepository.GetEmployeeByUserId(userId).IdEmployee;
                    model.ModificationDate = DateTime.Now;


                    if (Guid.TryParse(userListAssignedToSelectedValue, out assignedTo))
                    {
                        model.AssignedToId = assignedTo;
                    }
                    else
                    {
                        model.AssignedToId = _employeeRepository.GetEmployeeByUserId(userId).IdEmployee;
                    }

                    var task = TryUpdateModelAsync(model);
                    task.Wait();
                    if (task.Result)
                    {
                        Guid idTask = _repository.UpdateTask(model);

                        foreach (var file in files)
                        {
                            Models.TaskAttachmentModel taskAttachmentModel = new Models.TaskAttachmentModel();
                            var output = Upload(file);

                            taskAttachmentModel.IdTask = idTask;
                            taskAttachmentModel.Attachment = output;
                            taskAttachmentModel.AttachmentName = file.FileName;

                            task = TryUpdateModelAsync(taskAttachmentModel);
                            task.Wait();
                            if (task.Result)
                            {
                                _taskAttachmentsRepository.InsertTaskAttachment(taskAttachmentModel);
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                return RedirectToAction("Index", id);

                            }
                        }
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

        // POST: TaskController/Finish
        public ActionResult Finish(Guid id)
        {
            var model = _repository.GetTasksById(id);
            SelectCategory(model.AssignedToString, model.AssignedToId.ToString());
            return View("FinishTask", model);
        }

        // POST: TaskController/Finish
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Finish(Guid id, IFormCollection collection, List<IFormFile> files)
        {
            try
            {
                Models.TaskModel model = _repository.GetTasksById(id);

                model.FinishedDate = DateTime.Now;
                model.IsActive = false;
                if (files.Count > 0)
                    model.HasAttachments = true;

                var task = TryUpdateModelAsync(model);
                task.Wait();
                if (task.Result)
                {
                    var idTask = _repository.FinishTask(model);

                    foreach (var file in files)
                    {
                        Models.TaskAttachmentModel taskAttachmentModel = new Models.TaskAttachmentModel();
                        var output = Upload(file);

                        taskAttachmentModel.IdTask = idTask;
                        taskAttachmentModel.Attachment = output;
                        taskAttachmentModel.AttachmentName = file.FileName;

                        task = TryUpdateModelAsync(taskAttachmentModel);
                        task.Wait();
                        if (task.Result)
                        {
                            _taskAttachmentsRepository.InsertTaskAttachment(taskAttachmentModel);
                        }
                        else
                        {
                            return RedirectToAction("Index", id);

                        }

                    }
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


        public byte[] Upload(IFormFile file)
        {

            using (var memoryStream = new MemoryStream())
            {
                file.CopyToAsync(memoryStream);
                byte[] fileBytes = memoryStream.ToArray();

                //store the file in the database
                //using (var db = new YourDbContext())
                //{
                //    var fileModel = new FileModel
                //    {
                //        FileName = file.FileName,
                //        ContentType = file.ContentType,
                //        Data = fileBytes
                //    };
                //    db.Files.Add(fileModel);
                //    db.SaveChanges();
                //}
                return fileBytes;
            }
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

            foreach (var item in _employeeRepository.GetAllEmployees(GetLoggedEmployee().IdEmployee)
                .Where(x => x.IdEmployee.ToString() != employeeValue))
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

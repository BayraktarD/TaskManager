using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManager.Data;
using TaskManager.Models;
using TaskManager.Repository;
using System.Web;
using TaskManager.Models.DBObjects;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.IO.Compression;

namespace TaskManager.Controllers
{
    public class TaskController : Controller
    {
        private Repository.TaskRepository _repository;

        private Repository.EmployeeRepository _employeeRepository;

        private Repository.TaskAttachmentRepository _taskAttachmentsRepository;
        private readonly IConfiguration _configuration;



        public TaskController(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _repository = new Repository.TaskRepository(dbContext);

            _employeeRepository = new Repository.EmployeeRepository(dbContext, _configuration);

            _taskAttachmentsRepository = new Repository.TaskAttachmentRepository(dbContext);
            _configuration = configuration;
        }

        public EmployeeModel LoggedEmployee { get; set; }


        private EmployeeModel GetLoggedEmployee()
        {
            var loggedUser = User.Claims.Select(x => x.Value).ToArray()[0];
            Guid.TryParse(loggedUser, out Guid userId);
            LoggedEmployee = _employeeRepository.GetEmployeeByUserId(userId);

            return LoggedEmployee;

        }

        private List<TaskModel> GetListTaskModel()
        {
            GetLoggedEmployee();

            ViewBag.EmployeeId = LoggedEmployee.IdEmployee;

            return _repository.GetAllEmployeeTasks(LoggedEmployee.IdEmployee);
        }

        // GET: TaskController
        public ActionResult Index()
        {
            return View("Index");
        }


        public ActionResult CreatedTasksAssigned()
        {
            var tasks = GetListTaskModel();
            GetPermissions();

            return View("CreatedTasksAssigned", tasks);
        }

        public ActionResult CreatedTasksInProgress()
        {
            var tasks = GetListTaskModel();
            GetPermissions();

            return View("CreatedTasksInProgress", tasks);
        }

        public ActionResult CreatedTasksFinished()
        {
            var tasks = GetListTaskModel();
            GetPermissions();

            return View("CreatedTasksFinished", tasks);
        }

        public ActionResult CreatedTasksMissed()
        {
            var tasks = GetListTaskModel();
            GetPermissions();

            return View("CreatedTasksMissed", tasks);
        }

        public ActionResult MyTasksMissed()
        {
            var tasks = GetListTaskModel();
            GetPermissions();

            return View("MyTasksMissed", tasks);
        }

        public ActionResult MyTasksFinished()
        {
            var tasks = GetListTaskModel();
            GetPermissions();

            return View("MyTasksFinished", tasks);
        }

        public ActionResult MyTasksToAccomplish()
        {
            var tasks = GetListTaskModel();
            GetPermissions();

            return View("MyTasksToAccomplish", tasks);
        }

        private void GetPermissions()
        {
            GetLoggedEmployee();
            ViewBag.CanCreate = LoggedEmployee.CanCreateTasks;
            ViewBag.CanEdit = LoggedEmployee.CanModifyTasks;
            ViewBag.CanDelete = LoggedEmployee.CanDeleteTasks;
            ViewBag.CanAssign = LoggedEmployee.CanAssignTasks;
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

                    string userListAssignedToSelectedValue = Request.Form["ddlUsersList"].ToString();

                    model.IsActive = model.StartDate >= DateTime.Now ? true : false;

                    model.HasAttachments = files.Count > 0 ? true : false;

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
                            taskAttachmentModel.AttachmentName = "Task_" + file.FileName;

                            task = TryUpdateModelAsync(taskAttachmentModel);
                            task.Wait();
                            if (task.Result)
                            {
                                _taskAttachmentsRepository.InsertTaskAttachment(taskAttachmentModel);
                                
                            }
                        }
                        return RedirectToAction("CreatedTasksAssigned");

                    }
                    else
                    {
                        SelectCategory(null, null);

                        return View("CreatedTasksAssigned");
                    }

                }
                else
                {
                    SelectCategory(null, null);
                    return View("CreatedTasksAssigned");
                }
            }
            catch
            {
                SelectCategory(null, null);
                return View("CreatedTasksAssigned");

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

                userListAssignedToSelectedValue = Request.Form["ddlUsersList"].ToString();

                if (Guid.TryParse(loggedUser, out Guid userId))
                {

                    model.ModifiedById = _employeeRepository.GetEmployeeByUserId(userId).IdEmployee;

                    model.ModificationDate = DateTime.Now;

                    model.IsActive = model.StartDate >= DateTime.Now ? true : false;

                    model.HasAttachments = files.Count > 0 ? true : false;

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
                            taskAttachmentModel.AttachmentName = "Task_"+file.FileName;

                            task = TryUpdateModelAsync(taskAttachmentModel);
                            task.Wait();
                            if (task.Result)
                            {
                                _taskAttachmentsRepository.InsertTaskAttachment(taskAttachmentModel);
                            }
                            else
                            {
                                return RedirectToAction("CreatedTasksAssigned");

                            }
                        }
                        return RedirectToAction("CreatedTasksAssigned");
                    }
                    else
                    {
                        return RedirectToAction("CreatedTasksAssigned");
                    }

                }
                else
                {

                    return RedirectToAction("CreatedTasksAssigned");
                }
            }

            catch
            {
                return RedirectToAction("CreatedTasksAssigned");
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

                model.HasAttachments = files.Count > 0 ? true : false;

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
                        taskAttachmentModel.AttachmentName = "Solution_"+file.FileName;

                        task = TryUpdateModelAsync(taskAttachmentModel);
                        task.Wait();
                        if (task.Result)
                        {
                            _taskAttachmentsRepository.InsertTaskAttachment(taskAttachmentModel);
                        }
                        else
                        {
                            GetPermissions();

                            return RedirectToAction("Index", id);

                        }

                    }
                    GetPermissions();

                    return RedirectToAction("MyTasksFinished");

                }
                else
                {
                    GetPermissions();

                    return RedirectToAction("Index", id);
                }

            }

            catch
            {
                GetPermissions();

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
                GetPermissions();

                return RedirectToAction("Index");
            }
            catch
            {

                return View("TaskDelete", id);
            }
        }

        public async Task<IActionResult> SelectCategory(string employeeText, string employeeValue)
        {

            List<SelectListItem> userList = new List<SelectListItem>();

            foreach (var item in await _employeeRepository.GetAllEmployees(GetLoggedEmployee().IdEmployee))
            {
                userList.Add(new SelectListItem
                {
                    Text = item.Name.ToString() + " " + item.Surname.ToString(),
                    Value = item.IdEmployee.ToString()
                });
            }

            ViewBag.UsersList = userList;

            if (employeeText != null && employeeValue != null)
            {
                var selectedEmployeeIndex = userList.IndexOf(userList.Where(x => x.Value == employeeValue).FirstOrDefault());
                ViewBag.SelectedEmployeeIndex = selectedEmployeeIndex + 1;
            }
            GetPermissions();

            return Ok();

        }

        public ActionResult DownloadAttachments(Guid id)
        {
            var taskAttachments = _taskAttachmentsRepository.GetAllTaskAttachments(id);

            if (taskAttachments.Count > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        foreach (var taskAttachment in taskAttachments)
                        {
                            var zipEntry = archive.CreateEntry(taskAttachment.AttachmentName);
                            using (var entryStream = zipEntry.Open())
                            using (var fileStream = new MemoryStream(taskAttachment.Attachment))
                            {
                                fileStream.CopyTo(entryStream);
                            }
                        }
                    }
                    memoryStream.Position = 0;
                    var result = new MemoryStream();
                    memoryStream.CopyTo(result);
                    result.Position = 0;
                    return File(result, "application/zip", "DownloadedFiles.zip");
                }
            }
            else
            {
                var task = _repository.GetTasksById(id);
                TempData["alertMsg"] = "This task has no attachments!";
                return View("TaskDetails", task);
            }
        }

        private void Download(Guid taskAttachmentsId)
        {

        }

        private string GetContentType(string fileName)
        {
            // Get the file's content type based on its file extension
            var extension = Path.GetExtension(fileName).ToLower();
            switch (extension)
            {
                case ".jpg": return "image/jpeg";
                case ".jpeg": return "image/jpeg";
                case ".png": return "image/png";
                case ".pdf": return "application/pdf";
                case ".doc": return "application/msword";
                case ".docx": return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                default: return "application/octet-stream";
            }
        }
    }
}

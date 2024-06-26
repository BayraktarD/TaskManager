﻿
using System.Globalization;
using System.Threading.Tasks;
using TaskManager.Controllers;
using TaskManager.Data;
using TaskManager.EmailManagementApi.Entities;
using TaskManager.Models;
using TaskManager.Models.DBObjects;
using TaskManager.Services.API;
using TaskManager.Usings;

namespace TaskManager.Repository
{
    public class TaskRepository
    {
        private ApplicationDbContext dbContext;

        private TaskAttachmentRepository _taskAttachmentsRepository;

        public TaskRepository()
        {
            this.dbContext = new ApplicationDbContext();
        }

        public TaskRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            this._taskAttachmentsRepository = new TaskAttachmentRepository(dbContext);

        }

        public void RefershTaskIsActiveState()
        {
            foreach (var item in dbContext.Tasks.Where(x => x.StartDate <= DateTime.Now.Date && x.FinishedDate == null && x.IsActive == false))
            {
                item.IsActive = true;
                dbContext.Update(item);
            }
            dbContext.SaveChanges();
        }

        public List<TaskModel> GetAllEmployeeTasks(Guid idEmployee)
        {
            RefershTaskIsActiveState();
            List<TaskModel> listTask = new List<TaskModel>();

            foreach (var task in dbContext.Tasks.Where(x => x.AssignedToId == idEmployee || x.CreatedById == idEmployee))
            {
                listTask.Add(MapDbObjectToModel(task));
            }

            return listTask;
        }


        public TaskModel GetTasksById(Guid id)
        {
            TaskModel task = new TaskModel();

            task = MapDbObjectToModel(dbContext.Tasks.FirstOrDefault(x => x.IdTask == id));

            return task;
        }

        public List<TaskModel> GetAllTasksCreatedById(Guid id)
        {
            List<TaskModel> listTask = new List<TaskModel>();

            foreach (var task in dbContext.Tasks.Where(x => x.CreatedById == id))
            {
                listTask.Add(MapDbObjectToModel(task));
            }

            return listTask;
        }

        public List<TaskModel> GetAllTasksAssignetToId(Guid id)
        {
            List<TaskModel> listTask = new List<TaskModel>();

            foreach (var task in dbContext.Tasks.Where(x => x.AssignedToId == id))
            {
                listTask.Add(MapDbObjectToModel(task));
            }

            return listTask;
        }

        public List<TaskModel> GetAllTasksModifiedById(Guid id)
        {
            List<TaskModel> listTask = new List<TaskModel>();

            foreach (var task in dbContext.Tasks.Where(x => x.AssignedToId == id))
            {
                listTask.Add(MapDbObjectToModel(task));
            }

            return listTask;
        }

        public TaskModel MapDbObjectToModel(Models.DBObjects.Task dbTask)
        {
            var listTask = new TaskModel();
            if (dbTask != null)
            {
                listTask.IdTask = dbTask.IdTask;
                listTask.CreatedById = dbTask.CreatedById;
                listTask.CreatedByString = dbContext.Employees.FirstOrDefault(x => x.IdEmployee == dbTask.CreatedById).Name + " " + dbContext.Employees.FirstOrDefault(x => x.IdEmployee == dbTask.CreatedById).Surname;
                listTask.CreationDate = dbTask.CreationDate;
                listTask.StartDate = dbTask.StartDate;
                listTask.EditableStartDate = dbTask.EditableStartDate;
                listTask.EndDate = dbTask.EndDate;
                listTask.EditableEndDate = dbTask.EditableEndDate;
                listTask.AssignedToId = dbTask.AssignedToId;
                listTask.AssignedToString = dbContext.Employees.FirstOrDefault(x => x.IdEmployee == dbTask.AssignedToId).Name + " " + dbContext.Employees.FirstOrDefault(x => x.IdEmployee == dbTask.AssignedToId).Surname;
                listTask.ModificationDate = dbTask.ModificationDate;
                listTask.ModifiedById = dbTask.ModifiedById;
                if (listTask.ModifiedById != null)
                    listTask.ModifiedByString = dbContext.Employees.FirstOrDefault(x => x.IdEmployee == dbTask.ModifiedById).Name + " " + dbContext.Employees.FirstOrDefault(x => x.IdEmployee == dbTask.ModifiedById).Surname;
                listTask.FinishedDate = dbTask.FinishedDate;
                listTask.IsActive = dbTask.IsActive;
                listTask.TaskDetails = dbTask.TaskDetails;
                listTask.HasAttachments = dbTask.HasAttachments;
                listTask.TaskName = dbTask.TaskName;
            }
            return listTask;
        }

        public Guid InsertTask(TaskModel taskModel)
        {
            taskModel.IdTask = Guid.NewGuid();
            dbContext.Add(MapModelToDbObject(taskModel));
            dbContext.SaveChanges();

            var assignedByEmployee = dbContext.Employees.FirstOrDefault(x => x.IdEmployee == taskModel.CreatedById);
            taskModel.CreatedByString = assignedByEmployee.Surname + " " + assignedByEmployee.Name;

            var assignedToEmployee = dbContext.Employees.FirstOrDefault(x => x.IdEmployee == taskModel.AssignedToId);
            taskModel.AssignedToString = assignedToEmployee.Surname + " " + assignedToEmployee.Name;


            var result = System.Threading.Tasks.Task.Run(async () => await SendEmail(EmailSubjectTypes.CreateTask, taskModel)).GetAwaiter().GetResult();

            return taskModel.IdTask;
        }

        async Task<bool> SendEmail(EmailSubjectTypes emailSubjectTypes, TaskModel task)
        {
            string endpoint = "api/EmailManagement/SendEmailAsync";

            //EmployeeRepository employeeRepository = new EmployeeRepository();
            //var employee = employeeRepository.GetEmployeeById(task.AssignedToId);

            ClientAppEmail clientAppEmail = new ClientAppEmail();
            switch (emailSubjectTypes)
            {
                case EmailSubjectTypes.CreateTask:
                    clientAppEmail.Body = GetCreateTaskEmailTempalte(task);
                    clientAppEmail.Subject = "New Task Assignment";
                    break;
                case EmailSubjectTypes.UpdateTask:
                    clientAppEmail.Body = GetUpdateTaskEmailTempalte(task);
                    clientAppEmail.Subject = "Task Update Notification";
                    break;
                case EmailSubjectTypes.DeleteTask:
                    clientAppEmail.Body = GetDeleteTaskEmailTempalte(task);
                    clientAppEmail.Subject = "Task Deletion Notification";
                    break;
            }

            clientAppEmail.EmailManagementApiKey = "cf13f65f-4ee6-4726-9e30-aa1992f93815";
            clientAppEmail.ConfirmUrl = "ConfirmUrl";
            clientAppEmail.Recipients = new List<EmailRecipient>
            {
                new EmailRecipient
                {
                    EmailAddress = "bayraktar.dorin@gmail.com",
                    FullName = string.Join(' ', task.AssignedToString)
                }
            };

            EmailManagementApiClient emailManagementApiClient = new EmailManagementApiClient();
            var result = emailManagementApiClient.SendEmailThroughEmailManagementApi(clientAppEmail);

            return true;

        }

        string GetCreateTaskEmailTempalte(TaskModel task)
        {
            var template = System.IO.File.ReadAllText("./EmailManagementApi/EmailTemplates/CreateTask.html");
            string emailBody = template
            .Replace("{{TaskName}}", task.TaskName)
           .Replace("{{CreatedByString}}", task.CreatedByString ?? "Unknown")
           .Replace("{{CreationDate}}", task.CreationDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture))
           .Replace("{{StartDate}}", task.StartDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture))
            .Replace("{{EndDate}}", task.EndDate.HasValue ? task.EndDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "Not specified")
            .Replace("{{TaskDetails}}", task.TaskDetails)
           .Replace("{{AssignedToString}}", task.AssignedToString ?? "User");

            return emailBody;
        }

        string GetUpdateTaskEmailTempalte(TaskModel task)
        {

            var template = System.IO.File.ReadAllText("./EmailManagementApi/EmailTemplates/UpdateTask.html");
            string emailBody = template
            .Replace("{{TaskName}}", task.TaskName)
            .Replace("{{CreatedByString}}", task.CreatedByString ?? "Unknown")
            .Replace("{{CreationDate}}", task.CreationDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture))
            .Replace("{{StartDate}}", task.StartDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture))
            .Replace("{{EndDate}}", task.EndDate.HasValue ? task.EndDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "Not specified")
            .Replace("{{ModifiedByString}}", task.ModifiedByString ?? "Unknown")
            .Replace("{{ModificationDate}}", task.ModificationDate.HasValue ? task.ModificationDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "Not specified")
            .Replace("{{TaskDetails}}", task.TaskDetails)
            .Replace("{{SolutionDetails}}", task.SolutionDetails ?? "Not specified")
            .Replace("{{HasAttachmentsText}}", task.HasAttachments ? "Yes" : "No")
            .Replace("{{IsActiveText}}", task.IsActive ? "Yes" : "No")
            .Replace("{{FinishedDate}}", task.FinishedDate.HasValue ? task.FinishedDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "Not specified")
            .Replace("{{AssignedToString}}", task.AssignedToString ?? "User");

            return emailBody;
        }

        string GetDeleteTaskEmailTempalte(TaskModel task)
        {
            var template = System.IO.File.ReadAllText("./EmailManagementApi/EmailTemplates/DeleteTask.html");
            string emailBody = template
            .Replace("{{TaskName}}", task.TaskName)
            .Replace("{{ModifiedByString}}", task.ModifiedByString ?? "Unknown")
            .Replace("{{ModificationDate}}", task.ModificationDate.HasValue ? task.ModificationDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "Not specified")
            .Replace("{{AssignedToString}}", task.AssignedToString ?? "User");

            return emailBody;
        }



        public Guid UpdateTask(TaskModel taskModel)
        {
            Models.DBObjects.Task dbTask = dbContext.Tasks.FirstOrDefault(x => x.IdTask == taskModel.IdTask);

            if (dbTask != null)
            {
                if (dbTask.EditableStartDate)
                    dbTask.StartDate = taskModel.StartDate;
                if (dbTask.EditableEndDate)
                    dbTask.EndDate = taskModel.EndDate;
                dbTask.AssignedToId = taskModel.AssignedToId;
                dbTask.ModificationDate = taskModel.ModificationDate;
                dbTask.ModifiedById = taskModel.ModifiedById;
                dbTask.IsActive = taskModel.IsActive;
                dbTask.TaskDetails = taskModel.TaskDetails;
                dbTask.HasAttachments = taskModel.HasAttachments;
                dbTask.TaskName = taskModel.TaskName;


                dbContext.Tasks.Update(dbTask);
                dbContext.SaveChanges();
                taskModel = MapDbObjectToModel(dbTask);
                var createdByEmployee = dbContext.Employees.FirstOrDefault(X => X.IdEmployee == taskModel.CreatedById);
                taskModel.CreatedByString = createdByEmployee.Surname + " " + createdByEmployee.Name;
                var modifiedByEmployee = dbContext.Employees.FirstOrDefault(X => X.IdEmployee == taskModel.ModifiedById);
                taskModel.ModifiedByString = modifiedByEmployee.Surname + " " + modifiedByEmployee.Name;
                var assignedToEmployee = dbContext.Employees.FirstOrDefault(x => x.IdEmployee == taskModel.AssignedToId);
                taskModel.AssignedToString = assignedToEmployee.Surname + " " + assignedToEmployee.Name;
                taskModel.ModificationDate = DateTime.Now;

                var result = System.Threading.Tasks.Task.Run(async () => await SendEmail(EmailSubjectTypes.UpdateTask, taskModel)).GetAwaiter().GetResult();
            }
            return taskModel.IdTask;

        }

        public Models.DBObjects.Task MapModelToDbObject(TaskModel taskModel)
        {
            Models.DBObjects.Task task = new Models.DBObjects.Task();
            if (taskModel != null)
            {
                task.IdTask = taskModel.IdTask;
                task.CreatedById = taskModel.CreatedById;
                task.CreationDate = taskModel.CreationDate;
                task.StartDate = taskModel.StartDate;
                task.EditableStartDate = taskModel.EditableStartDate;
                task.EndDate = taskModel.EndDate;
                task.EditableEndDate = taskModel.EditableEndDate;
                task.AssignedToId = taskModel.AssignedToId;

                if (task.StartDate.Date <= DateTime.Now.Date)
                    task.IsActive = true;
                else
                    task.IsActive = false;
                task.TaskDetails = taskModel.TaskDetails;

                task.HasAttachments = taskModel.HasAttachments;
                task.TaskName = taskModel.TaskName;

            }
            return task;
        }


        public void DeleteTask(Guid idTask)
        {
            Models.DBObjects.Task task = dbContext.Tasks.Where(x => x.IdTask == idTask).FirstOrDefault();

            List<TaskAttachment> taskAttachments = dbContext.TaskAttachments.Select(x => x).Where(x => x.IdTask == idTask).ToList();

            foreach (var taskAttachment in taskAttachments)
            {
                _taskAttachmentsRepository.DeleteTaskAttachments(_taskAttachmentsRepository.MapDbObjectToModel(taskAttachment));
            }

            if (task != null)
            {
                var taskModel = MapDbObjectToModel(task);
                dbContext.Tasks.Remove(task);
                dbContext.SaveChanges();
                var modifiedByEmployee = dbContext.Employees.FirstOrDefault(X => X.IdEmployee == taskModel.ModifiedById);
                taskModel.ModifiedByString = modifiedByEmployee.Surname + " " + modifiedByEmployee.Name;
                var assignedToEmployee = dbContext.Employees.FirstOrDefault(x => x.IdEmployee == taskModel.AssignedToId);
                taskModel.AssignedToString = assignedToEmployee.Surname + " " + assignedToEmployee.Name;
                taskModel.ModificationDate = DateTime.Now;

                var result = System.Threading.Tasks.Task.Run(async () => await SendEmail(EmailSubjectTypes.DeleteTask, taskModel)).GetAwaiter().GetResult();
            }

        }



        public Guid FinishTask(TaskModel taskModel)
        {
            Models.DBObjects.Task task = dbContext.Tasks.Where(x => x.IdTask == taskModel.IdTask).FirstOrDefault();
            if (task != null)
            {
                task.FinishedDate = taskModel.FinishedDate;
                task.IsActive = taskModel.IsActive;
                task.HasAttachments = taskModel.HasAttachments;

                dbContext.Update(task);
                dbContext.SaveChanges();
            }
            return task.IdTask;
        }
    }
}


using TaskManager.Controllers;
using TaskManager.Data;
using TaskManager.Models;
using TaskManager.Models.DBObjects;

namespace TaskManager.Repository
{
    public class TaskRepository
    {
        private ApplicationDbContext dbContext;


        public TaskRepository()
        {
            this.dbContext = new ApplicationDbContext();
        }

        public TaskRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;

        }

        public List<TaskModel> GetAllEmployeeTasks(Guid idEmployee)
        {
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
            }
            return listTask;
        }

        public Guid InsertTask(TaskModel taskModel)
        {
            taskModel.IdTask = Guid.NewGuid();
            dbContext.Add(MapModelToDbObject(taskModel));
            dbContext.SaveChanges();
            return taskModel.IdTask;
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

                dbContext.Tasks.Update(dbTask);
                dbContext.SaveChanges();
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

                if (task.StartDate.Date == DateTime.Now.Date)
                    task.IsActive = true;
                else
                    task.IsActive &= taskModel.IsActive;
                task.TaskDetails = taskModel.TaskDetails;

                task.HasAttachments = taskModel.HasAttachments;
            }
            return task;
        }


        public void DeleteTask(Guid idTask)
        {
            Models.DBObjects.Task task = dbContext.Tasks.Where(x => x.IdTask == idTask).FirstOrDefault();

            if (task != null)
            {
                dbContext.Tasks.Remove(task);
                dbContext.SaveChanges();
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

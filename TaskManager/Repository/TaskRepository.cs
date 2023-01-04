
using TaskManager.Data;
using TaskManager.Models;

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

        public List<TaskModel> GetAllTasks()
        {
            List<TaskModel> listTask = new List<TaskModel>();

            foreach (var task in dbContext.Tasks)
            {
                listTask.Add(MapDbObjectToModel(task));
            }

            return listTask;
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
                listTask.CreationDate = dbTask.CreationDate;
                listTask.StartDate = dbTask.StartDate;
                listTask.EditableStartDate = dbTask.EditableStartDate;
                listTask.EndDate = dbTask.EndDate;
                listTask.EditableEndDate = dbTask.EditableEndDate;
                listTask.AssignedToId = dbTask.AssignedToId;
                listTask.ModificationDate = dbTask.ModificationDate;
                listTask.ModifiedById = dbTask.ModifiedById;
                listTask.FinishedDate = dbTask.FinishedDate;
                listTask.IsActive = dbTask.IsActive;
                listTask.Details = dbTask.Details;
                listTask.HasAttachments = dbTask.HasAttachments;
            }
            return listTask;
        }

        public void InsertTask(TaskModel taskModel)
        {
            taskModel.IdTask = Guid.NewGuid();
            dbContext.Add(MapModelToDbObject(taskModel));
            dbContext.SaveChanges();
        }

        public void UpdateTask(TaskModel taskModel)
        {
            Models.DBObjects.Task dbTask = dbContext.Tasks.FirstOrDefault(x => x.IdTask == taskModel.IdTask);

            if (dbTask != null)
            {
                dbTask.CreatedById = taskModel.CreatedById;
                dbTask.CreationDate = taskModel.CreationDate;
                dbTask.StartDate = taskModel.StartDate;
                dbTask.EditableStartDate = taskModel.EditableStartDate;
                dbTask.EndDate = taskModel.EndDate;
                dbTask.EditableEndDate = taskModel.EditableEndDate;
                dbTask.AssignedToId = taskModel.AssignedToId;
                dbTask.ModificationDate = taskModel.ModificationDate;
                dbTask.ModifiedById = taskModel.ModifiedById;
                dbTask.FinishedDate = taskModel.FinishedDate;
                dbTask.IsActive = taskModel.IsActive;
                dbTask.Details = taskModel.Details;
                dbTask.HasAttachments = taskModel.HasAttachments;

                dbContext.Tasks.Update(dbTask);
                dbContext.SaveChanges();
            }
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
                task.ModificationDate = taskModel.ModificationDate;
                task.ModifiedById = taskModel.ModifiedById;
                task.FinishedDate = taskModel.FinishedDate;
                task.IsActive = taskModel.IsActive;
                task.Details = taskModel.Details;
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
    }
}

using TaskManager.Data;
using TaskManager.Models;
using TaskManager.Models.DBObjects;

namespace TaskManager.Repository
{
    public class TaskAttachmentRepository
    {
        private ApplicationDbContext dbContext;

        public TaskAttachmentRepository()
        {
            this.dbContext = new ApplicationDbContext();
        }

        public TaskAttachmentRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<TaskAttachmentModel> GetAllTaskAttachments(Guid idTask)
        {
            List<TaskAttachmentModel> listTaskAttachments = new List<TaskAttachmentModel>();

            foreach (var taskAttachment in dbContext.TaskAttachments.Where(x => x.IdTask == idTask))
            {
                listTaskAttachments.Add(MapDbObjectToModel(taskAttachment));
            }

            return listTaskAttachments;
        }

        public TaskAttachmentModel MapDbObjectToModel(TaskAttachment taskAttachment)
        {
            TaskAttachmentModel taskAttachmentModel = new TaskAttachmentModel();
            if (taskAttachment != null)
            {
                taskAttachmentModel.IdTask = taskAttachment.IdTask;
                taskAttachmentModel.IdAttachment = taskAttachment.IdAttachment;
                taskAttachmentModel.Attachment = taskAttachment.Attachment;
                taskAttachmentModel.AttachmentType = taskAttachment.AttachmentType;
            }
            return taskAttachmentModel;
        }

        public void InsertTaskAttachment(TaskAttachmentModel taskAttachmentModel)
        {
            if (taskAttachmentModel != null)
            {
                taskAttachmentModel.IdAttachment = Guid.NewGuid();
                dbContext.Add(MapModelToDbObject(taskAttachmentModel));
                dbContext.SaveChanges();
            }
        }

        public void UpdateTaskAttachment(TaskAttachmentModel taskAttachmentModel)
        {
            TaskAttachment taskAttachment = dbContext.TaskAttachments
                .FirstOrDefault(x => x.IdTask == taskAttachmentModel.IdTask
                                && x.IdAttachment == taskAttachmentModel.IdAttachment);
            if (taskAttachment != null)
            {
                dbContext.Add(MapModelToDbObject(taskAttachmentModel));
                dbContext.SaveChanges();
            }
        }

        public TaskAttachment MapModelToDbObject(TaskAttachmentModel taskAttachmentModel)
        {
            TaskAttachment taskAttachment = new TaskAttachment();
            if (taskAttachmentModel != null)
            {
                taskAttachment.IdTask = taskAttachmentModel.IdTask;
                taskAttachment.IdAttachment = taskAttachmentModel.IdAttachment;
                taskAttachment.Attachment = taskAttachmentModel.Attachment;
                taskAttachment.AttachmentType = taskAttachmentModel.AttachmentType;
            }
            return taskAttachment;
        }

        public void DeleteTaskAttachments(TaskAttachmentModel taskAttachmentModel)
        {
            TaskAttachment taskAttachment = dbContext.TaskAttachments
                .FirstOrDefault(x => x.IdTask == taskAttachmentModel.IdTask
                                && x.IdAttachment == taskAttachmentModel.IdAttachment);
            if(taskAttachment != null)
            {
                dbContext.TaskAttachments.Remove(taskAttachment);
                dbContext.SaveChanges();
            }
        }
    }
}

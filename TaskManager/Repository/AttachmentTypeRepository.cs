using TaskManager.Data;
using TaskManager.Models;
using TaskManager.Models.DBObjects;

namespace TaskManager.Repository
{
    public class AttachmentTypeRepository
    {
        private ApplicationDbContext dbContext;

        public AttachmentTypeRepository()
        {
            this.dbContext = new ApplicationDbContext();
        }

        public AttachmentTypeRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<AttachmentTypeModel> GetAllAttachments()
        {
            List<AttachmentTypeModel> listAttachmentTypes = new List<AttachmentTypeModel>();

            foreach (var attachmentType in dbContext.AttachmentTypes.OrderBy(x=>x.Type))
            {
                listAttachmentTypes.Add(MapDbObjectToModel(attachmentType));
            }

            return listAttachmentTypes;
        }

        public AttachmentTypeModel GetAttachmentTypeById(Guid id)
        {
            AttachmentTypeModel attachmentTypeModel = new AttachmentTypeModel();

            attachmentTypeModel = MapDbObjectToModel(dbContext.AttachmentTypes.FirstOrDefault(x => x.IdAttachmentType == id));

            return attachmentTypeModel;
        }


        public AttachmentTypeModel MapDbObjectToModel(AttachmentType attachmentType)
        {
            var attachmentTypeModel = new AttachmentTypeModel();
            if (attachmentType != null)
            {
                attachmentTypeModel.IdAttachmentType = attachmentType.IdAttachmentType;
                attachmentTypeModel.Type = attachmentType.Type;
            }
            return attachmentTypeModel;
        }

        public void InsertAttachmentType(AttachmentTypeModel attachmentTypeModel)
        {
            if (attachmentTypeModel != null)
            {
                attachmentTypeModel.IdAttachmentType = Guid.NewGuid();
                dbContext.AttachmentTypes.Add(MapModelToDbObject(attachmentTypeModel));
                dbContext.SaveChanges();
            }
        }

        public AttachmentType MapModelToDbObject(AttachmentTypeModel attachmentTypeModel)
        {
            AttachmentType attachmentType = new AttachmentType();

            attachmentType.IdAttachmentType = attachmentTypeModel.IdAttachmentType;
            attachmentType.Type = attachmentTypeModel.Type;

            return attachmentType;
        }

        public void UpdateAttachmentType(AttachmentTypeModel attachmentTypeModel)
        {
            AttachmentType attachmentType = dbContext.AttachmentTypes.FirstOrDefault(x => x.IdAttachmentType == attachmentTypeModel.IdAttachmentType);
            if (attachmentType != null)
            {
                attachmentType.Type= attachmentTypeModel.Type;

                dbContext.Update(attachmentType);

                dbContext.SaveChanges();
            }
        }

        public void DeleteAttachmentType(Guid id)
        {
            AttachmentType attachmentType = dbContext.AttachmentTypes.FirstOrDefault(x => x.IdAttachmentType == id);
            if (attachmentType != null)
            {
                dbContext.AttachmentTypes.Remove(attachmentType);
                dbContext.SaveChanges();
            }
        }


    }
}

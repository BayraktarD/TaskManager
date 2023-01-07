using TaskManager.Data;
using TaskManager.Models;
using TaskManager.Models.DBObjects;

namespace TaskManager.Repository
{
    public class UserTypeRepository
    {
        private ApplicationDbContext dbContext;

        public UserTypeRepository()
        {
            this.dbContext = new ApplicationDbContext();
        }

        public UserTypeRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<UserTypeModel> GetUserTypes()
        {
            List<UserTypeModel> userTypes = new List<UserTypeModel>();
            foreach (var userType in dbContext.UserTypes)
            {
                userTypes.Add(MapDbObjectToModel(userType));
            }
            return userTypes;
        }

        public UserTypeModel GetUserTypeById(Guid id)
        {
            UserTypeModel userTypeModel = new UserTypeModel();

            userTypeModel = MapDbObjectToModel(dbContext.UserTypes.FirstOrDefault(x => x.IdUserType == id));

            return userTypeModel;
        }

        public UserTypeModel MapDbObjectToModel(UserType dbUserType)
        {
            UserTypeModel userTypeModel = new UserTypeModel();
            if (dbUserType != null)
            {
                userTypeModel.IdUserType = dbUserType.IdUserType;
                userTypeModel.UserType1 = dbUserType.UserType1;
            }
            return userTypeModel;
        }

        public void InsertUserType(UserTypeModel userTypeModel)
        {
            userTypeModel.IdUserType = Guid.NewGuid();
            dbContext.Add(MapModelToDbObject(userTypeModel));
            dbContext.SaveChanges();
        }

        public void UpdateUserType(UserTypeModel userTypeModel)
        {
            UserType userType = dbContext.UserTypes.FirstOrDefault(x => x.IdUserType == userTypeModel.IdUserType);
            if (userType != null)
            {
                userType.UserType1 = userTypeModel.UserType1;
                dbContext.Update(userType);
                dbContext.SaveChanges();
            }
        }

        public UserType MapModelToDbObject(UserTypeModel userTypeModel)
        {
            UserType userType = new UserType();
            if (userTypeModel != null)
            {
                userType.IdUserType = userTypeModel.IdUserType;
                userType.UserType1 = userTypeModel.UserType1;
            }
            return userType;
        }

        public void DeleteUserType(Guid id)
        {
            UserType userType = dbContext.UserTypes.FirstOrDefault(x => x.IdUserType == id);
            if (userType != null)
            {
                dbContext.UserTypes.Remove(userType);
                dbContext.SaveChanges();
            }
        }

    }
}

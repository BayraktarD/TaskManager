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

        public void AddUpdateUserType(UserTypeModel userType)
        {
            if (userType != null)
            {
                if (userType.IdUserType != null)
                {
                    dbContext.UserTypes.Update(MapModelToDbObject(userType));
                    dbContext.SaveChanges();
                }
                else
                {
                    dbContext.UserTypes.Add(MapModelToDbObject(userType));
                    dbContext.SaveChanges();
                }
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

    }
}

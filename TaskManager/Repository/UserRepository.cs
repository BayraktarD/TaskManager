using TaskManager.Data;
using TaskManager.Models;
using TaskManager.Models.DBObjects;

namespace TaskManager.Repository
{
    public class UserRepository
    {
        private ApplicationDbContext dbContext;

        public UserRepository()
        {
            this.dbContext = new ApplicationDbContext();
        }

        public UserRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<UserModel> GetAllUsers()
        {
            List<UserModel> users = new List<UserModel>();
            foreach (var dbUser in dbContext.Users)
            {
                users.Add(MapDbObjectToModel(dbUser));
            }
            return users;
        }

        public UserModel MapDbObjectToModel(User dbUser)
        {
            UserModel user = new UserModel();
            if (dbUser != null)
            {
                user.IdUser = dbUser.IdUser;
                user.Username = dbUser.Username;
                user.Password = dbUser.Password;
                user.UserType = dbUser.UserType;
                user.IsActive = dbUser.IsActive;
                user.Email = dbUser.Email;
                user.CanCreateTasks = dbUser.CanCreateTasks;
                user.CanAssignTasks = dbUser.CanAssignTasks;
                user.CanUnassignTasks = dbUser.CanUnassignTasks;
                user.CanDeleteTasks = dbUser.CanDeleteTasks;
                user.CanActivateProfiles = dbUser.CanActivateProfiles;
                user.CanDeactivateProfiles = dbUser.CanDeactivateProfiles;
                user.CanModifyTasks = dbUser.CanModifyTasks;
                user.JobTitle = dbUser.JobTitle;
                user.Department = dbUser.Department;
            }
            return user;
        }

        public void AddUpdateUser(UserModel userModel)
        {
            if (userModel != null)
            {
                if (userModel.IdUser != null)
                {
                    dbContext.Update(MapModelToDbObject(userModel));
                    dbContext.SaveChanges();
                }
                else
                {
                    dbContext.Add(MapModelToDbObject(userModel));
                    dbContext.SaveChanges();
                }
            }
        }

        public User MapModelToDbObject(UserModel userModel)
        {
            User dbUser = new User();
            if (userModel != null)
            {
                dbUser.IdUser = userModel.IdUser;
                dbUser.Username = userModel.Username;
                dbUser.Password = userModel.Password;
                dbUser.UserType = userModel.UserType;
                dbUser.IsActive = userModel.IsActive;
                dbUser.Email = userModel.Email;
                dbUser.CanCreateTasks = userModel.CanCreateTasks;
                dbUser.CanAssignTasks = userModel.CanAssignTasks;
                dbUser.CanUnassignTasks = userModel.CanUnassignTasks;
                dbUser.CanDeleteTasks = userModel.CanDeleteTasks;
                dbUser.CanActivateProfiles = userModel.CanActivateProfiles;
                dbUser.CanDeactivateProfiles = userModel.CanDeactivateProfiles;
                dbUser.CanModifyTasks = userModel.CanModifyTasks;
                dbUser.JobTitle = userModel.JobTitle;
                dbUser.Department = userModel.Department;
            }
            return dbUser;
        }

        public void DeactivateUser(Guid idUser)
        {
            User user = dbContext.Users.Where(x => x.IdUser == idUser).FirstOrDefault();
            if (user != null)
            {
                user.IsActive = false;
                dbContext.SaveChanges();
            }
        }
    }
}

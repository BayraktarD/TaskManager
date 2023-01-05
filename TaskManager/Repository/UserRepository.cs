using Microsoft.EntityFrameworkCore;
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

        public UserModel GetUserById(Guid id)
        {
            UserModel userModel = new UserModel();

            User user = dbContext.Users.FirstOrDefault(x => x.IdUser == id);
            if (user != null)
            {
                MapDbObjectToModel(user);
            }
            return userModel;
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

        public void InsertUser(UserModel userModel)
        {
            if (userModel != null)
            {
                userModel.IdUser = Guid.NewGuid();
                dbContext.Add(MapModelToDbObject(userModel));
                dbContext.SaveChanges();
            }
        }

        public void UpdateUser(UserModel userModel)
        {
            User user = dbContext.Users.FirstOrDefault(x => x.IdUser == userModel.IdUser);
            if (user != null)
            {
                user.Username = userModel.Username;
                user.Password = userModel.Password;
                user.UserType = userModel.UserType;
                user.IsActive = userModel.IsActive;
                user.Email = userModel.Email;
                user.CanCreateTasks = userModel.CanCreateTasks;
                user.CanAssignTasks = userModel.CanAssignTasks;
                user.CanUnassignTasks = userModel.CanUnassignTasks;
                user.CanDeleteTasks = userModel.CanDeleteTasks;
                user.CanActivateProfiles = userModel.CanActivateProfiles;
                user.CanDeactivateProfiles = userModel.CanDeactivateProfiles;
                user.CanModifyTasks = userModel.CanModifyTasks;
                user.JobTitle = userModel.JobTitle;
                user.Department = userModel.Department;

                dbContext.Update(user);
                dbContext.SaveChanges();
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
            User user = dbContext.Users.FirstOrDefault(x => x.IdUser == idUser);
            if (user != null)
            {
                user.IsActive = false;
                dbContext.SaveChanges();
            }
        }

        public void DeleteUser(Guid idUser)
        {
            User user = dbContext.Users.FirstOrDefault(x => x.IdUser == idUser);
            if (user != null)
            {
                dbContext.Users.Remove(user);
                dbContext.SaveChanges();
            }
        }
    }
}

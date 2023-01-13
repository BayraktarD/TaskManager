using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Models;
using TaskManager.Models.DBObjects;

namespace TaskManager.Repository
{
    public class EmployeeRepository
    {
        private ApplicationDbContext dbContext;

        public EmployeeRepository()
        {
            this.dbContext = new ApplicationDbContext();
        }

        public EmployeeRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<EmployeeModel> GetAllEmployees()
        {
            List<EmployeeModel> models = new List<EmployeeModel>();
            foreach (var dbModel in dbContext.Employees)
            {
                models.Add(MapDbObjectToModel(dbModel));
            }
            return models;
        }

        public EmployeeModel GetEmployeeById(Guid id)
        {
            EmployeeModel model = new EmployeeModel();

            model = MapDbObjectToModel(dbContext.Employees.FirstOrDefault(x => x.IdEmployee == id));

            return model;
        }

        public EmployeeModel GetEmployeeByUserId(Guid id)
        {
            EmployeeModel model = new EmployeeModel();

            var employee = dbContext.Employees.FirstOrDefault(x => x.UserId == id.ToString());
            model = MapDbObjectToModel(employee);

            return model;
        }

        public EmployeeModel MapDbObjectToModel(Employee dbObject)
        {
            EmployeeModel model = new EmployeeModel();
            if (dbObject != null)
            {
                model.IdEmployee = dbObject.IdEmployee;
                model.Name = dbObject.Name;
                model.Surname = dbObject.Surname;
                model.UserId = dbObject.UserId;
               // model.UserTypeString = dbContext.UserTypes.FirstOrDefault(x => x.IdUserType == dbObject.UserType).UserType1.ToString();
                model.IsActive = dbObject.IsActive;
                model.Email = dbObject.Email;
                model.CanCreateTasks = dbObject.CanCreateTasks;
                model.CanAssignTasks = dbObject.CanAssignTasks;
                model.CanUnassignTasks = dbObject.CanUnassignTasks;
                model.CanDeleteTasks = dbObject.CanDeleteTasks;
                model.CanActivateProfiles = dbObject.CanActivateProfiles;
                model.CanDeactivateProfiles = dbObject.CanDeactivateProfiles;
                model.CanModifyTasks = dbObject.CanModifyTasks;
                model.JobTitle = dbObject.JobTitle;
                model.JobTitleString = dbContext.JobTitles.FirstOrDefault(x => x.IdJobTitle == dbObject.JobTitle).JobTitle1.ToString();
                model.Department = dbObject.Department;
                model.DepartmentString = dbContext.Departments.FirstOrDefault(x=>x.IdDepartment == dbObject.Department).Department1.ToString();
            }
            return model;
        }

        public void InsertEmployee(EmployeeModel model)
        {
            if (model != null)
            {
                model.IdEmployee = Guid.NewGuid();
                dbContext.Add(MapModelToDbObject(model));
                dbContext.SaveChanges();
            }
        }

        public void UpdateEmployee(EmployeeModel model)
        {
            Employee dbObject = dbContext.Employees.FirstOrDefault(x => x.IdEmployee == model.IdEmployee);
            if (dbObject != null)
            {
                dbObject.Name = model.Name;
                dbObject.Surname = model.Surname;
                dbObject.UserId = model.UserId;
                dbObject.IsActive = model.IsActive;
                dbObject.Email = model.Email;
                dbObject.CanCreateTasks = model.CanCreateTasks;
                dbObject.CanAssignTasks = model.CanAssignTasks;
                dbObject.CanUnassignTasks = model.CanUnassignTasks;
                dbObject.CanDeleteTasks = model.CanDeleteTasks;
                dbObject.CanActivateProfiles = model.CanActivateProfiles;
                dbObject.CanDeactivateProfiles = model.CanDeactivateProfiles;
                dbObject.CanModifyTasks = model.CanModifyTasks;
                dbObject.JobTitle = model.JobTitle;
                dbObject.Department = model.Department;

                dbContext.Update(dbObject);
                dbContext.SaveChanges();
            }
        }

        public Employee MapModelToDbObject(EmployeeModel model)
        {
            Employee dbObject = new Employee();
            if (model != null)
            {
                dbObject.IdEmployee = model.IdEmployee;
                dbObject.Name = model.Name;
                dbObject.Surname = model.Surname;
                dbObject.UserId = model.UserId;
                dbObject.IsActive = model.IsActive;
                dbObject.Email = model.Email;
                dbObject.CanCreateTasks = model.CanCreateTasks;
                dbObject.CanAssignTasks = model.CanAssignTasks;
                dbObject.CanUnassignTasks = model.CanUnassignTasks;
                dbObject.CanDeleteTasks = model.CanDeleteTasks;
                dbObject.CanActivateProfiles = model.CanActivateProfiles;
                dbObject.CanDeactivateProfiles = model.CanDeactivateProfiles;
                dbObject.CanModifyTasks = model.CanModifyTasks;
                dbObject.JobTitle = model.JobTitle;
                dbObject.Department = model.Department;
            }
            return dbObject;
        }

        public void DeactivateEmployee(Guid id)
        {
            Employee employee = dbContext.Employees.FirstOrDefault(x => x.IdEmployee == id);
            if (employee != null)
            {
                employee.IsActive = false;
                dbContext.SaveChanges();
            }
        }

        public void DeleteEmployee(Guid id)
        {
            Employee employee = dbContext.Employees.FirstOrDefault(x => x.IdEmployee == id);
            if (employee != null)
            {
                dbContext.Employees.Remove(employee);
                dbContext.SaveChanges();
            }
        }
    }
}

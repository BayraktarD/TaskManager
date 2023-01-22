using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TaskManager.Data;
using TaskManager.Models;
using TaskManager.Models.DBObjects;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace TaskManager.Repository
{
    public class EmployeeRepository
    {

        private ApplicationDbContext dbContext;

        private readonly IConfiguration _configuration;

        public EmployeeRepository()
        {
            this.dbContext = new ApplicationDbContext();
        }

        public EmployeeRepository(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            this.dbContext = dbContext;
            _configuration= configuration;  
        }

        public List<EmployeeModel> GetAllEmployees(Guid log)
        {
            List<EmployeeModel> models = new List<EmployeeModel>();
            foreach (var dbModel in dbContext.Employees.Where(x => x.IdEmployee != log))
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
                if (model.UserId != null)
                    model.IsActive = dbObject.IsActive;
                model.Email = dbObject.Email;
                model.CanCreateTasks = dbObject.CanCreateTasks;
                model.CanAssignTasks = dbObject.CanAssignTasks;
                model.CanModifyProfiles = dbObject.CanModifyProfiles;
                model.CanDeleteTasks = dbObject.CanDeleteTasks;
                model.CanCreateProfiles = dbObject.CanCreateProfiles;
                model.CanDeleteProfiles = dbObject.CanDeleteProfiles;
                model.CanModifyTasks = dbObject.CanModifyTasks;
                model.JobTitle = dbObject.JobTitle;
                model.JobTitleString = dbContext.JobTitles.FirstOrDefault(x => x.IdJobTitle == dbObject.JobTitle).JobTitle1.ToString();
                model.Department = dbObject.Department;
                model.DepartmentString = dbContext.Departments.FirstOrDefault(x => x.IdDepartment == dbObject.Department).Department1.ToString();
            }
            return model;
        }

        public void InsertEmployee(EmployeeModel model)
        {
            if (model != null)
            {
                model.IdEmployee = Guid.NewGuid();

                model.UserId = InsertNewUser(model.Email, model.Password);
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
                if (model.UserId != null)
                    dbObject.IsActive = model.IsActive;
                dbObject.Email = model.Email;
                dbObject.CanCreateTasks = model.CanCreateTasks;
                dbObject.CanAssignTasks = model.CanAssignTasks;
                dbObject.CanModifyProfiles = model.CanModifyProfiles;
                dbObject.CanDeleteTasks = model.CanDeleteTasks;
                dbObject.CanCreateProfiles = model.CanCreateProfiles;
                dbObject.CanDeleteProfiles = model.CanDeleteProfiles;
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
                dbObject.CanModifyProfiles = model.CanModifyProfiles;
                dbObject.CanDeleteTasks = model.CanDeleteTasks;
                dbObject.CanCreateProfiles = model.CanCreateProfiles;
                dbObject.CanDeleteProfiles = model.CanDeleteProfiles;
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


        public string InsertNewUser(string email, string password)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                Guid userId = Guid.NewGuid();

                string userName = email;

                string insert = @"Insert into AspNetUsers (Id,
                                                            UserName,
                                                            NormalizedUserName,
                                                            Email,
                                                            NormalizedEmail,
                                                            EmailConfirmed,
                                                            PasswordHash,
                                                            PhoneNumberConfirmed,
                                                            TwoFactorEnabled,
                                                            LockoutEnabled,
                                                            AccessFailedCount
                                                            ) values("
                                                        +"'"+ userId.ToString() + "', "
                                                        + "'" + userName + "', "
                                                        + "'" + userName.ToUpper() + "', "
                                                        + "'" + email + "', "
                                                        + "'" + email.ToUpper() + "', "
                                                        + "1,"
                                                        + "'" + password.GetHashCode()+"', "
                                                        + "0" + ","
                                                        + "0" + ","
                                                        + "0" + ","
                                                        + "0"
                                                        + ")";

                SqlCommand cmd = new SqlCommand(insert, connection);


                var i = cmd.ExecuteNonQuery();


                if (i > 0)
                {
                    string getInserted = @"select Id from AspNetUsers where Email = '" + email+"'";
                    SqlCommand command = new SqlCommand(getInserted, connection);
                    var result = command.ExecuteScalar().ToString();

                    return result;
                }
            }

            return null;
        }
    }
}

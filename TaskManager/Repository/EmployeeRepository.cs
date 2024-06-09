using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System.Data;
using TaskManager.Data;
using TaskManager.EmailManagementApi.Entities;
using TaskManager.Models;
using TaskManager.Models.DBObjects;
using TaskManager.Services.API;
using TaskManager.Usings;
using PasswordVerificationResult = Microsoft.AspNetCore.Identity.PasswordVerificationResult;

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
            _configuration = configuration;
        }

        public async Task<List<EmployeeModel>> GetAllEmployees(Guid log)
        {
            List<EmployeeModel> models = new List<EmployeeModel>();

            var currentUser = dbContext.Employees.FirstOrDefault(x => x.IdEmployee == log);
            if (currentUser != null)
            {
                if (currentUser.Name.ToLower().Contains("demo") || currentUser.Surname.ToLower().Contains("demo") || currentUser.Email.ToLower().Contains("demo"))
                {
                    foreach (var dbModel in dbContext.Employees.Where(x => x.IdEmployee != log
                                                            && (
                                                                x.Name.ToLower().Contains("demo") == true
                                                                && x.Surname.ToLower().Contains("demo") == true
                                                                && x.Email.ToLower().Contains("demo") == true)
                                                                )
                        )
                    {
                        models.Add(MapDbObjectToModel(dbModel));
                    }
                }
                else
                {

                    foreach (var dbModel in dbContext.Employees.Where(x => x.IdEmployee != log
                                                            && (
                                                               x.Name.ToLower().Contains("demo") == false
                                                                || x.Surname.ToLower().Contains("demo") == false
                                                                || x.Email.ToLower().Contains("demo") == false)
                                                                )
                        )
                    {
                        models.Add(MapDbObjectToModel(dbModel));
                    }
                }
            }
            return models;
        }

        public async Task<List<EmployeeModel>> GetAllDemoEmployees()
        {
            try
            {
                List<EmployeeModel> models = new List<EmployeeModel>();

                foreach (var dbModel in dbContext.Employees.Where(x =>
                                                            x.Name.ToLower().Contains("demo") == true
                                                            && x.Surname.ToLower().Contains("demo") == true
                                                            && x.Email.ToLower().Contains("demo") == true)
                    )
                {
                    models.Add(MapDbObjectToModel(dbModel));
                }
                return models;

            }
            catch (Exception ex)
            {

                throw;
            }
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

        public Guid InsertEmployee(EmployeeModel model)
        {
            if (model != null)
            {
                model.IdEmployee = Guid.NewGuid();

                //var userId = InsertNewUser(model.Email, model.Password);
                //model.UserId = userId;
                dbContext.Employees.Add(MapModelToDbObject(model));
                dbContext.SaveChanges();

                var result = System.Threading.Tasks.Task.Run(async () => await SendEmail(EmailSubjectTypes.UserCreated, model.IdEmployee)).GetAwaiter().GetResult();

            }
            return model.IdEmployee;
        }

        public void UpdateEmployee(EmployeeModel model)
        {
            Employee dbObject = dbContext.Employees.FirstOrDefault(x => x.IdEmployee == model.IdEmployee);

            if (dbObject != null)
            {
                // UpdateUser(dbObject.UserId, model.Email, model.Password);

                dbObject.Name = model.Name;
                dbObject.Surname = model.Surname;
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

                var result = System.Threading.Tasks.Task.Run(async () => await SendEmail(EmailSubjectTypes.UserUpdated, model.IdEmployee)).GetAwaiter().GetResult();

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
                var result = System.Threading.Tasks.Task.Run(async () => await SendEmail(EmailSubjectTypes.UserDeleted, employee.IdEmployee)).GetAwaiter().GetResult();
                //DeleteUserFromDB(employee.UserId);
                dbContext.Employees.Remove(employee);
                dbContext.SaveChanges();


            }
        }


        public string InsertNewUser(string email, string password)
        {
            string connectionString = _configuration.GetConnectionString("TaskManagerDB");

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                Guid userId = Guid.NewGuid();
                string userName = email;

                IdentityUser user = GenerateNewUser(email, userId, userName);

                user = HashPassword(user, password);

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
                                                        + "'" + userId.ToString() + "', "
                                                        + "'" + userName + "', "
                                                        + "'" + userName.ToUpper() + "', "
                                                        + "'" + email + "', "
                                                        + "'" + email.ToUpper() + "', "
                                                        + "1,"
                                                        + "'" + user.PasswordHash + "', "
                                                        + "0" + ","
                                                        + "0" + ","
                                                        + "0" + ","
                                                        + "0"
                                                        + ")";

                SqlCommand cmd = new SqlCommand(insert, connection);


                var i = cmd.ExecuteNonQuery();

                connection.Close();
                connection.Dispose();

                if (i > 0)
                {
                    var employeeIdString = GetInsertedUserId(email);
                    return employeeIdString;
                }


            }

            return null;
        }

        private string GetInsertedUserId(string email)
        {
            string connectionString = _configuration.GetConnectionString("TaskManagerDB");

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                string getInserted = @"select Id from AspNetUsers where Email = '" + email + "'";
                SqlCommand command = new SqlCommand(getInserted, connection);
                var result = command.ExecuteScalar();

                connection.Close();
                connection.Dispose();
                return result.ToString();
            }
        }




        private static IdentityUser GenerateNewUser(string email, Guid userId, string userName)
        {
            return new IdentityUser()
            {
                Id = userId.ToString(),
                UserName = userName,
                NormalizedUserName = userName.ToUpper(),
                Email = email,
                NormalizedEmail = email.ToUpper(),
                EmailConfirmed = true,
                PasswordHash = null,
                SecurityStamp = null,
                ConcurrencyStamp = null,
                PhoneNumber = null,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnd = null,
                LockoutEnabled = false,
                AccessFailedCount = 0,
            };
        }

        public string UpdateUser(string userId, string email, string password)
        {
            string connectionString = _configuration.GetConnectionString("TaskManagerDB");

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                DataTable dt = GetUserFromDb(userId);

                var user = MapDbUserToUserModel(dt, userId);

                user = HashPassword(user, password);

                string hashedPassword = user.PasswordHash;
                string securityStamp = user.SecurityStamp;

                string userName = email;

                string update = @"update AspNetUsers
                                set PasswordHash = '" + hashedPassword + "',"
                                + "SecurityStamp = '" + securityStamp + "',"
                                + "UserName = '" + userName + "',"
                                + "NormalizedUserName = '" + email.ToUpper() + "',"
                                + "Email = '" + email + "',"
                                + "NormalizedEmail = '" + email.ToUpper() + "'"
                                + "where Id = '" + userId + "'";

                SqlCommand cmd = new SqlCommand(update, connection);

                var i = cmd.ExecuteNonQuery();

                if (i > 0)
                {
                    string getUpdated = @"select Id from AspNetUsers where Email = '" + email + "'";
                    SqlCommand command = new SqlCommand(getUpdated, connection);
                    var result = command.ExecuteScalar().ToString();

                    connection.Close();

                    return result;
                }

                return null;
            }
        }

        public IdentityUser MapDbUserToUserModel(DataTable dt, string userId)
        {
            var users = dt.AsEnumerable().Select(x =>
            new IdentityUser
            {
                Id = x.Field<string>("Id"),
                UserName = x.Field<string>("UserName"),
                NormalizedUserName = x.Field<string>("NormalizedUserName"),
                Email = x.Field<string>("Email"),
                NormalizedEmail = x.Field<string>("NormalizedEmail"),
                EmailConfirmed = x.Field<bool>("EmailConfirmed"),
                PasswordHash = x.Field<string>("PasswordHash"),
                SecurityStamp = x.Field<string>("SecurityStamp"),
                ConcurrencyStamp = x.Field<string>("ConcurrencyStamp"),
                PhoneNumber = x.Field<string>("PhoneNumber"),
                PhoneNumberConfirmed = x.Field<bool>("PhoneNumberConfirmed"),
                TwoFactorEnabled = x.Field<bool>("TwoFactorEnabled"),
                LockoutEnd = x.Field<string>("LockoutEnd") != null ? x.Field<DateTime>("LockoutEnd") : null,
                LockoutEnabled = x.Field<bool>("LockoutEnabled"),
                AccessFailedCount = x.Field<int>("AccessFailedCount"),
            });

            var user = users.Where(x => x.Id == userId).FirstOrDefault();

            return user;
        }

        public DataTable GetUserFromDb(string userId)
        {
            string connectionString = _configuration.GetConnectionString("TaskManagerDB");
            using (SqlConnection connection = new SqlConnection())
            {

                connection.ConnectionString = connectionString;
                connection.Open();

                string getUser = @"select Id 
                                      , UserName 
                                      , NormalizedUserName 
                                      , Email 
                                      , NormalizedEmail 
                                      , EmailConfirmed 
                                      , PasswordHash 
                                      , SecurityStamp 
                                      , ConcurrencyStamp 
                                      , PhoneNumber 
                                      , PhoneNumberConfirmed 
                                      , TwoFactorEnabled 
                                      , LockoutEnd 
                                      , LockoutEnabled 
                                      , AccessFailedCount 
                                from AspNetUsers where Id = '" + userId + "'";

                SqlDataAdapter dap = new SqlDataAdapter(getUser, connection);

                DataTable dt = new DataTable();
                dap.Fill(dt);
                return dt;
            }

        }

        public IdentityUser HashPassword(IdentityUser user, string password)
        {
            var hasher = new PasswordHasher<IdentityUser>();

            var hashedPassword = hasher.HashPassword(user, password);

            if (VerifyHasher(hasher, user, hashedPassword, password))
            {
                user.SecurityStamp = UpdateSecurityStampAsync(user);
                user.PasswordHash = hashedPassword;
                return user;
            }
            else
            {
                return null;
            }

        }

        public string UpdateSecurityStampAsync(IdentityUser user)
        {
            var userManager = new Microsoft.AspNetCore.Identity.UserManager<IdentityUser>(
                                    new UserStore<IdentityUser>(dbContext),
                                    new OptionsWrapper<IdentityOptions>(new IdentityOptions()),
                                    new PasswordHasher<IdentityUser>(),
                                    new IUserValidator<IdentityUser>[0],
                                    new IPasswordValidator<IdentityUser>[0],
                                    new UpperInvariantLookupNormalizer(),
                                    new IdentityErrorDescriber(),
                                    null,
                                    new NullLogger<Microsoft.AspNetCore.Identity.UserManager<IdentityUser>>()
                                    );
            userManager.UpdateSecurityStampAsync(user);
            if (user.SecurityStamp != null)
            {
                return user.SecurityStamp;
            }
            else
            {
                return null;
            }
        }

        public bool VerifyHasher(PasswordHasher<IdentityUser> hasher, IdentityUser user, string hashedPassword, string providedPassword)
        {
            var result = hasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
            if (result == PasswordVerificationResult.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UserHasTasksToAccomplish(Guid idUser)
        {
            var userTasks = dbContext.Tasks.Where(x => x.AssignedToId == idUser && x.IsActive == false).Count();
            if (userTasks > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public void DeleteUserFromDB(string userId)
        {
            string connectionString = _configuration.GetConnectionString("TaskManagerDB");
            using (SqlConnection connection = new SqlConnection())
            {

                connection.ConnectionString = connectionString;
                connection.Open();

                string deleteUser = @"delete from AspNetUsers where Id = '" + userId + "'";

                SqlCommand command = new SqlCommand(deleteUser, connection);
                command.ExecuteScalar();
            }
        }

        async Task<bool> SendEmail(EmailSubjectTypes emailSubjectTypes, Guid employeeId)
        {
            string endpoint = "api/EmailManagement/SendEmailAsync";

            var employee = GetEmployeeById(employeeId);

            ClientAppEmail clientAppEmail = new ClientAppEmail();
            switch (emailSubjectTypes)
            {
                case EmailSubjectTypes.UserCreated:
                    clientAppEmail.Body = GetCreateUserEmailContent(employee);
                    clientAppEmail.Subject = "Welcome to TaskManager";
                    break;
                case EmailSubjectTypes.UserUpdated:
                    clientAppEmail.Body = GetUpdateUserEmailContent(employee);
                    clientAppEmail.Subject = "Account Update Notification";
                    break;
                case EmailSubjectTypes.UserDeleted:
                    clientAppEmail.Body = GetDeleteUserEmailContent(employee);
                    clientAppEmail.Subject = "Account Deletion Notification";
                    break;
            }

            clientAppEmail.EmailManagementApiKey = "cf13f65f-4ee6-4726-9e30-aa1992f93815";
            clientAppEmail.ConfirmUrl = "ConfirmUrl";
            clientAppEmail.Recipients = new List<EmailRecipient>
            {
                new EmailRecipient
                {
                    EmailAddress = "bayraktar.dorin@gmail.com",
                    FullName = string.Join(' ', employee.Name + " " + employee.Surname)
                }
            };

            EmailManagementApiClient emailManagementApiClient = new EmailManagementApiClient();
            var result = emailManagementApiClient.SendEmailThroughEmailManagementApi(clientAppEmail);

            return true;

        }

        string GetCreateUserEmailContent(EmployeeModel employee)
        {
            var template = System.IO.File.ReadAllText("./EmailManagementApi/EmailTemplates/UserCreated.html");

            template = template.Replace("{{Name}}", employee.Name)
                               .Replace("{{Surname}}", employee.Surname)
                               .Replace("{{Email}}", employee.Email)
                               .Replace("{{CanCreateTasks}}", employee.CanCreateTasks ? "Yes" : "No")
                               .Replace("{{CanAssignTasks}}", employee.CanAssignTasks ? "Yes" : "No")
                               .Replace("{{CanModifyTasks}}", employee.CanModifyTasks ? "Yes" : "No")
                               .Replace("{{CanDeleteTasks}}", employee.CanDeleteTasks ? "Yes" : "No")
                               .Replace("{{CanCreateProfiles}}", employee.CanCreateProfiles ? "Yes" : "No")
                               .Replace("{{CanModifyProfiles}}", employee.CanModifyProfiles ? "Yes" : "No")
                               .Replace("{{CanDeleteProfiles}}", employee.CanDeleteProfiles ? "Yes" : "No")
                               .Replace("{{JobTitleString}}", employee.JobTitleString ?? "N/A")
                               .Replace("{{DepartmentString}}", employee.DepartmentString ?? "N/A")
                               .Replace("{{CurrentYear}}", DateTime.Now.Year.ToString());

            return template;
        }

        string GetUpdateUserEmailContent(EmployeeModel employee)
        {
            var template = System.IO.File.ReadAllText("./EmailManagementApi/EmailTemplates/UpdateUser.html");

            template = template.Replace("{{Name}}", employee.Name)
                               .Replace("{{Surname}}", employee.Surname)
                               .Replace("{{Email}}", employee.Email)
                               .Replace("{{CanCreateTasks}}", employee.CanCreateTasks ? "Yes" : "No")
                               .Replace("{{CanAssignTasks}}", employee.CanAssignTasks ? "Yes" : "No")
                               .Replace("{{CanModifyTasks}}", employee.CanModifyTasks ? "Yes" : "No")
                               .Replace("{{CanDeleteTasks}}", employee.CanDeleteTasks ? "Yes" : "No")
                               .Replace("{{CanCreateProfiles}}", employee.CanCreateProfiles ? "Yes" : "No")
                               .Replace("{{CanModifyProfiles}}", employee.CanModifyProfiles ? "Yes" : "No")
                               .Replace("{{CanDeleteProfiles}}", employee.CanDeleteProfiles ? "Yes" : "No")
                               .Replace("{{JobTitleString}}", employee.JobTitleString ?? "N/A")
                               .Replace("{{DepartmentString}}", employee.DepartmentString ?? "N/A")
                               .Replace("{{CurrentYear}}", DateTime.Now.Year.ToString());

            return template;
        }


        public string GetDeleteUserEmailContent(EmployeeModel employee)
        {

            var template = System.IO.File.ReadAllText("./EmailManagementApi/EmailTemplates/DeleteUser.html");

            template = template.Replace("{{Name}}", employee.Name)
                               .Replace("{{Surname}}", employee.Surname)
                               .Replace("{{IdEmployee}}", employee.IdEmployee.ToString())
                               .Replace("{{Email}}", employee.Email)
                               .Replace("{{JobTitleString}}", employee.JobTitleString ?? "N/A")
                               .Replace("{{DepartmentString}}", employee.DepartmentString ?? "N/A")
                               .Replace("{{DeletionDate}}", DateTime.Now.ToString("MMMM dd, yyyy HH:mm"))
                               .Replace("{{CurrentYear}}", DateTime.Now.Year.ToString());

            return template;
        }
    }
}

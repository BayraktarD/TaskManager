using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TaskManager.Data;
using TaskManager.Models;
using TaskManager.Models.DBObjects;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Diagnostics.Eventing.Reader;
using PasswordVerificationResult = Microsoft.AspNetCore.Identity.PasswordVerificationResult;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

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
                UpdateUser(dbObject.UserId, model.Email, model.Password);

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

                string passwordEncrypted = HashPassword(password);

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
                                                        + "'" + userId.ToString() + "', "
                                                        + "'" + userName + "', "
                                                        + "'" + userName.ToUpper() + "', "
                                                        + "'" + email + "', "
                                                        + "'" + email.ToUpper() + "', "
                                                        + "1,"
                                                        + "'" + password.ToHashSet() + "', "
                                                        + "0" + ","
                                                        + "0" + ","
                                                        + "0" + ","
                                                        + "0"
                                                        + ")";

                SqlCommand cmd = new SqlCommand(insert, connection);


                var i = cmd.ExecuteNonQuery();


                if (i > 0)
                {
                    string getInserted = @"select Id from AspNetUsers where Email = '" + email + "'";
                    SqlCommand command = new SqlCommand(getInserted, connection);
                    var result = command.ExecuteScalar().ToString();

                    connection.Close();

                    return result;
                }
            }

            return null;
        }

        public string UpdateUser(string userId, string email, string password)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                var user = HashPassword(userId, password);

                string hashedPassword = user.PasswordHash;
                string securityStamp = user.SecurityStamp;

                string userName = email;


                string update = @"update AspNetUsers
                                set PasswordHash = '" + hashedPassword + "',"
                                + "SecurityStamp = '"+securityStamp+"',"
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

        public IdentityUser HashPassword(string userId, string password)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

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

                var hasher = new PasswordHasher<IdentityUser>();

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

                var hashedPassword = hasher.HashPassword(user, password);

                user.SecurityStamp = UpdateSecurityStampAsync(user);
                user.PasswordHash = hashedPassword;

                return user;
            }

        }

        public string UpdateSecurityStampAsync(IdentityUser user)
        {
            var userManager = new UserManager<IdentityUser>(
                                    new UserStore<IdentityUser>(dbContext),
                                    new OptionsWrapper<IdentityOptions>(new IdentityOptions()),
                                    new PasswordHasher<IdentityUser>(),
                                    new IUserValidator<IdentityUser>[0],
                                    new IPasswordValidator<IdentityUser>[0],
                                    new UpperInvariantLookupNormalizer(),
                                    new IdentityErrorDescriber(),
                                    null,
                                    new NullLogger<UserManager<IdentityUser>>()
                                    );
            userManager.UpdateSecurityStampAsync(user);
            return user.SecurityStamp;
        }

        public bool VerifyHasher(PasswordHasher<IdentityUser> hasher, IdentityUser user, string hashedPassword)
        {
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, hashedPassword);
            if (result == PasswordVerificationResult.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
            Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: password!,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8));
            return hashed;
        }

        public string Criptare(string text)
        {
            char[] s = text.ToCharArray();
            for (int i = 0; i < s.Length; i++)
            {
                if (i % 2 != 0)
                    s[i] += (char)1;
                else
                    s[i] -= (char)1;
            }
            text = new string(s);
            return text;
        }
    }
}

using TaskManager.Data;
using TaskManager.Models;
using TaskManager.Models.DBObjects;

namespace TaskManager.Repository
{
    public class DepartmentRepository
    {
        private ApplicationDbContext dbContext;

        public DepartmentRepository()
        {
            this.dbContext = new ApplicationDbContext();
        }

        public DepartmentRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<DepartmentModel> GetAllDepartments()
        {
            List<DepartmentModel> listDepartments = new List<DepartmentModel>();

            foreach (var department in dbContext.Departments)
            {
                listDepartments.Add(MapDbObjectToModel(department));
            }

            return listDepartments;
        }

        public DepartmentModel GetDepartmentById(Guid id)
        {
            DepartmentModel departmentModel= MapDbObjectToModel(dbContext.Departments.FirstOrDefault(x=>x.IdDepartment == id));
            return departmentModel;
        }

        public DepartmentModel MapDbObjectToModel(Department dbDepartment)
        {
            var departmentModel = new DepartmentModel();
            if (dbDepartment != null)
            {
                departmentModel.IdDepartment = dbDepartment.IdDepartment;
                departmentModel.Department1 = dbDepartment.Department1;
            }
            return departmentModel;
        }

        public void InsertDepartment(DepartmentModel departmentModel)
        {
            if (departmentModel != null)
            {
                departmentModel.IdDepartment = Guid.NewGuid();
                dbContext.Departments.Add(MapModelToDbObject(departmentModel));
                dbContext.SaveChanges();
            }
        }

        public Department MapModelToDbObject(DepartmentModel departmentModel)
        {
            var department = new Department();
            department.IdDepartment = departmentModel.IdDepartment;
            department.Department1 = departmentModel.Department1;
            return department;
        }

        public void UpdateDepartment(DepartmentModel departmentModel)
        {
            Department department = dbContext.Departments.SingleOrDefault(x => x.IdDepartment == departmentModel.IdDepartment);
            if (department != null)
            {
                department.Department1 = departmentModel.Department1;
                dbContext.Update(department);
                dbContext.SaveChanges();
            }
        }

        public void DeleteDepartment(Guid idDepartment)
        {
            Department department = dbContext.Departments.SingleOrDefault(x => x.IdDepartment == idDepartment);
            if(department != null)
            {
                dbContext.Departments.Remove(department);
                dbContext.SaveChanges();
            }
        }

    }
}

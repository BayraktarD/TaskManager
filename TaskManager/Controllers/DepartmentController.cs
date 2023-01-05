using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Data;

namespace TaskManager.Controllers
{
    public class DepartmentController : Controller
    {

        private Repository.DepartmentRepository _repository;

        public DepartmentController(ApplicationDbContext dbContext)
        {
            _repository = new Repository.DepartmentRepository(dbContext);
        }

        // GET: DepartmentController
        public ActionResult Index()
        {
            var departments = _repository.GetAllDepartments();
            return View("Index", departments);
        }

        // GET: DepartmentController/Details/5
        public ActionResult Details(Guid id)
        {
            var model = _repository.GetDepartmentById(id);
            return View("DepartmentDetails", model);
        }

        // GET: DepartmentController/Create
        public ActionResult Create()
        {
            return View("DepartmentCreate");
        }

        // POST: DepartmentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                Models.DepartmentModel departmentModel = new Models.DepartmentModel();

                var task = TryUpdateModelAsync(departmentModel);
                task.Wait();

                if (task.Result)
                {
                    _repository.InsertDepartment(departmentModel);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View("DepartmentCreate");
            }
        }

        // GET: DepartmentController/Edit/5
        public ActionResult Edit(Guid id)
        {
            var model = _repository.GetDepartmentById(id);
            return View("DepartmentEdit",model);
        }

        // POST: DepartmentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, IFormCollection collection)
        {
            try
            {
                Models.DepartmentModel model = new Models.DepartmentModel();

                var task = TryUpdateModelAsync(model);
                task.Wait();
                if (task.Result)
                {
                    _repository.UpdateDepartment(model);
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index", id);
                }
            }
            catch
            {
                return RedirectToAction("Index", id);
            }
        }

        // GET: DepartmentController/Delete/5
        public ActionResult Delete(Guid id)
        {
            var model = _repository.GetDepartmentById(id);
            return View("DepartmentDelete", model);
        }

        // POST: DepartmentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, IFormCollection collection)
        {
            try
            {
                _repository.DeleteDepartment(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("DepartmentDelete", id);
            }
        }
    }
}

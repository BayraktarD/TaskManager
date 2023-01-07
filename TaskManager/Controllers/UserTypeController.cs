using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Data;

namespace TaskManager.Controllers
{
    public class UserTypeController : Controller
    {
        private Repository.UserTypeRepository _repository;

        public UserTypeController(ApplicationDbContext dbContext)
        {
            _repository = new Repository.UserTypeRepository(dbContext);
        }

        // GET: UserTypeController
        public ActionResult Index()
        {
            var userTypes = _repository.GetUserTypes();
            return View("Index", userTypes);
        }

        // GET: UserTypeController/Details/5
        public ActionResult Details(Guid id)
        {
            Models.UserTypeModel userType = _repository.GetUserTypeById(id);
            return View("UserTypeDetails", userType);
        }

        // GET: UserTypeController/Create
        public ActionResult Create()
        {
            return View("UserTypeCreate");
        }

        // POST: UserTypeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                Models.UserTypeModel model = new Models.UserTypeModel();

                var task = TryUpdateModelAsync(model);
                task.Wait();
                if (task.Result)
                {
                    _repository.InsertUserType(model);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("UserTypeCreate");
            }
        }

        // GET: UserTypeController/Edit/5
        public ActionResult Edit(Guid id)
        {
            Models.UserTypeModel model = _repository.GetUserTypeById(id);
            return View("UserTypeEdit",model);
        }

        // POST: UserTypeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, IFormCollection collection)
        {
            try
            {
                Models.UserTypeModel model = new Models.UserTypeModel();
                var task = TryUpdateModelAsync(model);
                task.Wait();
                if (task.Result)
                {
                    _repository.UpdateUserType(model);
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

        // GET: UserTypeController/Delete/5
        public ActionResult Delete(Guid id)
        {
            Models.UserTypeModel model = _repository.GetUserTypeById(id);
            return View("UserTypeDelete",model);
        }

        // POST: UserTypeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, IFormCollection collection)
        {
            try
            {
                _repository.DeleteUserType(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("UserTypeDelete", id);
            }
        }
    }
}

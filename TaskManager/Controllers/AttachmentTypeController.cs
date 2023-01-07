using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Data;

namespace TaskManager.Controllers
{
    public class AttachmentTypeController : Controller
    {
        private Repository.AttachmentTypeRepository _repository;

        public AttachmentTypeController(ApplicationDbContext dbContext)
        {
            _repository = new Repository.AttachmentTypeRepository(dbContext);
        }
        // GET: AttachmentTypeController
        public ActionResult Index()
        {
            var attachmentTypes = _repository.GetAllAttachments();
            return View("Index", attachmentTypes);
        }

        // GET: AttachmentTypeController/Details/5
        public ActionResult Details(Guid id)
        {
            Models.AttachmentTypeModel model = _repository.GetAttachmentTypeById(id);
            return View("AttachmentTypeDetails",model);
        }

        // GET: AttachmentTypeController/Create
        public ActionResult Create()
        {
            return View("AttachmentTypeCreate");
        }

        // POST: AttachmentTypeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                Models.AttachmentTypeModel model = new Models.AttachmentTypeModel();
                var task = TryUpdateModelAsync(model);
                task.Wait();
                if (task.Result)
                {
                    _repository.InsertAttachmentType(model);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View("AttachmentTypeCreate");
            }
        }

        // GET: AttachmentTypeController/Edit/5
        public ActionResult Edit(Guid id)
        {
            Models.AttachmentTypeModel model = _repository.GetAttachmentTypeById(id);
            return View("AttachmentTypeEdit", model);
        }

        // POST: AttachmentTypeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, IFormCollection collection)
        {
            try
            {
                Models.AttachmentTypeModel model = new Models.AttachmentTypeModel();
                var task = TryUpdateModelAsync(model);
                task.Wait();
                if (task.Result)
                {
                    _repository.UpdateAttachmentType(model);
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

        // GET: AttachmentTypeController/Delete/5
        public ActionResult Delete(Guid id)
        {
            Models.AttachmentTypeModel model = _repository.GetAttachmentTypeById(id);
            return View("AttachmentTypeDelete", model);
        }

        // POST: AttachmentTypeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, IFormCollection collection)
        {
            try
            {
                _repository.DeleteAttachmentType(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View("AttachmentTypeDelete", id);

            }
        }
    }
}

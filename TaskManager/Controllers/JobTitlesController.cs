using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Data;

namespace TaskManager.Controllers
{
    public class JobTitlesController : Controller
    {

        private Repository.JobTitleRepository _repository;

        public JobTitlesController(ApplicationDbContext dbContext)
        {
            _repository = new Repository.JobTitleRepository(dbContext);
        }

        // GET: JobTitlesController
        public ActionResult Index()
        {
            var jobTitles = _repository.GetAllJobTitles();
            return View("Index", jobTitles);
        }

        // GET: JobTitlesController/Details/5
        public ActionResult Details(Guid id)
        {
            var jobTitle = _repository.GetJobTitleById(id);
            return View("JobTitleDetails",jobTitle);
        }

        // GET: JobTitlesController/Create
        public ActionResult Create()
        {
            return View("JobTitleCreate");
        }

        // POST: JobTitlesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                Models.JobTitlesModel jobTitlesModel = new Models.JobTitlesModel();

                var task = TryUpdateModelAsync(jobTitlesModel);
                task.Wait();

                if (task.Result)
                {
                    _repository.InsertJobTitle(jobTitlesModel);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View("JobTitleCreate");
            }
        }

        // GET: JobTitlesController/Edit/5
        public ActionResult Edit(Guid id)
        {
            var model = _repository.GetJobTitleById(id);
            return View("JobTitleEdit", model);
        }

        // POST: JobTitlesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, IFormCollection collection)
        {
            try
            {
                Models.JobTitlesModel model = new Models.JobTitlesModel();
                var task = TryUpdateModelAsync(model);
                task.Wait();
                if (task.Result)
                {
                    _repository.UpdateJobTitle(model);
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

        // GET: JobTitlesController/Delete/5
        public ActionResult Delete(Guid id)
        {
            var model = _repository.GetJobTitleById(id);
            return View("JobTitleDelete", model);
        }

        // POST: JobTitlesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, IFormCollection collection)
        {
            try
            {
                _repository.DeleteJobTitle(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("JobTitleDelete", id);
            }
        }
    }
}

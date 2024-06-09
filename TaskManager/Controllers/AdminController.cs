using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using TaskManager.EmailManagementApi.Entities;
using TaskManager.Services.API;
using static Humanizer.On;

namespace TaskManager.Controllers
{
    public class AdminController : Controller
    {
        // GET: AdminController
        public ActionResult Index()
        {
            return View("Index");
        }

        // GET: AdminController/TaskDetails/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminController/Create
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> AddClientApp()
        {
            IncomingClientApp incomingClientApp = new IncomingClientApp
            {
                ClientAppName = "TaskManager",
                ConfirmUrl = "ConfirmUrl",
                ClientRsaPublicKey = "ClientRsaPublicKey",
                EmailAddress = "dorin.bayraktar@ulbsibiu.ro",
                EmailManagementApiKey = ""
            };

            string json = JsonConvert.SerializeObject(incomingClientApp);

            EmailManagementApiClient emailManagementApiClient = new EmailManagementApiClient();

            var responseJson = await emailManagementApiClient.PostAsync(json, "https://localhost:7120/" + "api/ClientApp/AddClientApp");
            Dictionary<bool, string> response = JsonConvert.DeserializeObject<Dictionary<bool, string>>(responseJson);
            if (response.Keys.First() == true)
            {
                return Json(new { success = true, message = "Client app added successfully!" });

            }

            //// Assuming EmailManagementApiClient is a service you've implemented or injected
            //var result = await EmailManagementApiClient.PostAsync<Task<IActionResult>>("api/ClientApp/AddClientApp", incomingClientApp);

            return Json(new { success = false, message = "Client app  wasn't added!" });
        }

      

        // POST: AdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

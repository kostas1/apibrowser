using APIBrowser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace APIBrowser.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View(ApiHelper.GetAllKeys());
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(Api api)
        {
            ApiHelper.SaveApi(api);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Call()
        {
            string api = Request.Params["api"];
            if (!string.IsNullOrWhiteSpace(api))
            {
                ApiHelper.Persist(api, Request.Params);
                return RedirectToAction("Index");
            }
            else
            {
                return HttpNotFound("Non-existent API or wrong parameters provided");
            }
        }

        [HttpGet]
        public ActionResult Browse(string key)
        {
            return View(ApiHelper.Get(key));
        }
    }
}

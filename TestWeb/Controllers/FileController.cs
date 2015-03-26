using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestWeb.Controllers
{
    public class FileController : Controller
    {
        public ActionResult Index(System.Web.HttpPostedFileBase file)
        {
            using(var reader = new System.IO.StreamReader(file.InputStream))
            {
                var content = reader.ReadToEnd();
                return Content(Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    content = content,
                    filename = file.FileName,
                }));
            }
        }

        public ActionResult Index2(System.Web.HttpPostedFileBase file, string strs)
        {
            using (var reader = new System.IO.StreamReader(file.InputStream))
            {
                var content = reader.ReadToEnd();
                return Content(Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    content = content,
                    filename = file.FileName,
                    strs = strs,
                }));
            }
        }
    }
}

using EntityFrameworkTutorial.DAL;
using EntityFrameworkTutorial.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EntityFrameworkTutorial.Controllers
{
   public class HomeController : Controller
   {
      private SchoolContext db = new SchoolContext();
      public ActionResult Index()
      {
         return View();
      }

      public ActionResult About()
      {
         IQueryable<EnrollmentDateGroup> enrollmentDates 
               = db.Students
               .GroupBy(s => s.EnrollmentDate)
               .Select(s => new EnrollmentDateGroup
               {
                  EnrollmentDate = s.Key,
                  StudentCount = s.Count()
               });
         
         return View(enrollmentDates);
      }

      public ActionResult Contact()
      {
         ViewBag.Message = "Your contact page.";

         return View();
      }
      protected override void Dispose(bool disposing)
      {
         db.Dispose();
         base.Dispose(disposing);
      }
   }
}
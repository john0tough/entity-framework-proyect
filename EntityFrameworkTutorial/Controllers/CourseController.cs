using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EntityFrameworkTutorial.DAL;
using EntityFrameworkTutorial.Models;

namespace EntityFrameworkTutorial.Controllers
{
   public class CourseController : Controller
   {
      private SchoolContext db = new SchoolContext();

      private void PopulateDepartmentDropDownList(int? DepartmentID = null) {
         var departmentQuery = db.Departments.OrderBy(d => d.Name);
         ViewBag.DepartmentID = new SelectList(departmentQuery, "DepartmentID", "Name", DepartmentID);
      }
      // GET: Course
      public ActionResult Index()
      {
         var courses = db.Courses.Include(c => c.Departament);
         return View(courses.ToList());
      }

      // GET: Course/Details/5
      public ActionResult Details(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         Course course = db.Courses.Find(id);
         if (course == null)
         {
            return HttpNotFound();
         }
         return View(course);
      }

      // GET: Course/Create
      public ActionResult Create()
      {
         PopulateDepartmentDropDownList();
         return View();
      }

      // POST: Course/Create
      // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
      // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Create([Bind(Include = "CourseID,Title,Credits,DepartmentID")] Course course)
      {
         try
         {
            if (ModelState.IsValid)
            {
               db.Courses.Add(course);
               db.SaveChanges();
               return RedirectToAction("Index");
            }
         }
         catch (RetryLimitExceededException /* dex */)
         {
            //Log the error (uncomment dex variable name and add a line here to write a log.)
            ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
         }
         PopulateDepartmentDropDownList(course.DepartmentID);
         return View(course);
      }

      // GET: Course/Edit/5
      public ActionResult Edit(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         Course course = db.Courses.Find(id);
         if (course == null)
         {
            return HttpNotFound();
         }

         PopulateDepartmentDropDownList(course.DepartmentID);
         return View(course);
      }

      // POST: Course/Edit/5
      // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
      // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost, ActionName("Edit")]
      [ValidateAntiForgeryToken]
      public ActionResult EditPost(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         var courseToUpdate = db.Courses.Find(id);
         if (TryUpdateModel(courseToUpdate, "", new string[] { "Title", "Credits", "DepartmentID"})) {
            try
            {
               db.SaveChanges();

               return RedirectToAction("Index");
            }
            catch (RetryLimitExceededException /* dex */)
            {
               //Log the error (uncomment dex variable name and add a line here to write a log.
               ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
         }
         PopulateDepartmentDropDownList(courseToUpdate.DepartmentID);
         return View(courseToUpdate);
      }

      // GET: Course/Delete/5
      public ActionResult Delete(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         Course course = db.Courses.Find(id);
         if (course == null)
         {
            return HttpNotFound();
         }
         return View(course);
      }

      // POST: Course/Delete/5
      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public ActionResult DeleteConfirmed(int id)
      {
         Course course = db.Courses.Find(id);
         db.Courses.Remove(course);
         db.SaveChanges();
         return RedirectToAction("Index");
      }

      protected override void Dispose(bool disposing)
      {
         if (disposing)
         {
            db.Dispose();
         }
         base.Dispose(disposing);
      }
   }
}

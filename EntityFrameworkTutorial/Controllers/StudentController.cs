using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EntityFrameworkTutorial.DAL;
using EntityFrameworkTutorial.Models;
using PagedList;

namespace EntityFrameworkTutorial.Controllers
{
   [RoutePrefix("Student")]
   [Route("{action=index}")]
   public class StudentController : Controller
   {
      private SchoolContext db = new SchoolContext();

      // GET: Student
      public ActionResult Index(string sortOrder, string filterString, string currentFilter, int page = 1)
      {
         ViewBag.CurrentSort = sortOrder;
         ViewBag.NameSortParam = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
         ViewBag.DateSortParam = sortOrder == "Date" ? "date_desc" : "Date";

         if (filterString == null)
         {
            filterString = currentFilter;
         }
         ViewBag.CurrentFilter = filterString;
         var students = db.Students.Select(s => s);
         if (!string.IsNullOrEmpty(filterString))
         {
            students = students.Where(s => s.LastName.Contains(filterString)
            || s.FirstMidName.Contains(filterString));
         }
         switch (sortOrder)
         {
            case "name_desc":
               students = students.OrderByDescending(s => s.LastName);
               break;
            case "Date":
               students = students.OrderBy(s => s.EnrollmentDate);
               break;
            case "date_desc":
               students = students.OrderByDescending(s => s.EnrollmentDate);
               break;
            default:
               students = students.OrderBy(s => s.LastName);
               break;
         }
         int pageSize = 3;
         int pageNumber = page;
         return View(students.ToPagedList(page, pageSize));
      }

      // GET: Student/Details/5
      [Route("details/{id:int}")]
      public ActionResult Details(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         Student student = db.Students.Find(id);
         if (student == null)
         {
            return HttpNotFound();
         }
         return View(student);
      }

      // GET: Student/Create
      [Route("create")]
      public ActionResult Create()
      {
         return View();
      }

      // POST: Student/Create
      // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
      // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
      [Route("create")]
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Create([Bind(Include = "LastName,FirstMidName,EnrollmentDate")] Student student)
      {
         try
         {
            if (ModelState.IsValid)
            {
               db.Students.Add(student);
               db.SaveChanges();
               return RedirectToAction("Index");
            }
         }
         catch (DataException /* dex */)
         {
            //Log the error (uncomment dex variable name and add a line here to write a log.
            ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
         }

         return View(student);
      }

      // GET: Student/Edit/5
      public ActionResult Edit(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         Student student = db.Students.Find(id);
         if (student == null)
         {
            return HttpNotFound();
         }
         return View(student);
      }

      // POST: Student/Edit/5
      // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
      // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Edit(int id)
      {
         var studentToUpdate = db.Students.Find(id);
         if (TryUpdateModel(studentToUpdate, "",
            new string[] { "LastName", "FirstMidName", "EnrollmentDate" }))
         {

            try
            {
               db.SaveChanges();
               return RedirectToAction("Index");
            }
            catch (DataException /* dex */)
            {
               //Log the error (uncomment dex variable name and add a line here to write a log.
               ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }


         }


         return View(studentToUpdate);
      }

      // GET: Student/Delete/5
      public ActionResult Delete(int id, bool? errorOcurred = false)
      {
         if (errorOcurred.GetValueOrDefault())
         {
            ViewBag.ErrorMessage = "Delete Failed, Try Again";
         }
         Student student = db.Students.Find(id);
         if (student == null)
         {
            return HttpNotFound();
         }
         return View(student);
      }

      // POST: Student/Delete/5
      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public ActionResult DeleteConfirmed(int id)
      {
         try
         {
            Student studentToDelete = new Student() { ID = id }; // se cambia el find, para inicializar una entidad para evitar dos consultas a solo la requerida para eliminar la entidad
            db.Entry(studentToDelete).State = EntityState.Deleted;
            db.SaveChanges();
         }
         catch (DataException /*dex*/)
         {
            return RedirectToAction("Delete");
         }
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

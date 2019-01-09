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
using EntityFrameworkTutorial.ViewModel;

namespace EntityFrameworkTutorial.Controllers
{
   public class InstructorController : Controller
   {
      private SchoolContext db = new SchoolContext();

      // GET: Instructor
      public ActionResult Index(int? id, int? courseID)
      {
         var viewModel = new InstructorIndexData();

         var instructors = db.Instructors
         .Include(i => i.OfficeAssignment)
         .Include(i => i.Courses.Select(c => c.Departament))
         .OrderBy(i => i.LastName);

         viewModel.Instructors = instructors;

         if (id != null)
         {
            ViewBag.InstructorID = id.Value;
            viewModel.Courses = viewModel.Instructors.Single(i => i.ID == id.Value).Courses;
         }

         if (courseID != null)
         {
            //ViewBag.CourseID = courseID.Value;
            //viewModel.Enrollments = viewModel.Courses.Single(x => x.CourseID == courseID).Enrollments;
            var selectedCourse = viewModel.Courses.SingleOrDefault(c => c.CourseID == courseID);
            db.Entry(selectedCourse).Collection(sc => sc.Enrollments).Load(); // si carga una coleccion de entidades se usa collection
            foreach (var enrollment in selectedCourse.Enrollments)
            {
               db.Entry(enrollment).Reference(e => e.Student).Load(); // si carga 1 sola entidadse usa reference
            }
            viewModel.Enrollments = selectedCourse.Enrollments;
         }
         return View(viewModel);
      }

      // GET: Instructor/Details/5
      public ActionResult Details(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         Instructor instructor = db.Instructors.Find(id);
         if (instructor == null)
         {
            return HttpNotFound();
         }
         return View(instructor);
      }

      // GET: Instructor/Create
      public ActionResult Create()
      {
         ViewBag.ID = new SelectList(db.OfficeAssignments, "InstructorID", "Location");
         return View();
      }

      // POST: Instructor/Create
      // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
      // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Create([Bind(Include = "ID,LastName,FirstMidName,HireDate")] Instructor instructor)
      {
         if (ModelState.IsValid)
         {
            db.Instructors.Add(instructor);
            db.SaveChanges();
            return RedirectToAction("Index");
         }

         ViewBag.ID = new SelectList(db.OfficeAssignments, "InstructorID", "Location", instructor.ID);
         return View(instructor);
      }

      // GET: Instructor/Edit/5
      public ActionResult Edit(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         Instructor instructor = db.Instructors.Find(id);
         if (instructor == null)
         {
            return HttpNotFound();
         }
         ViewBag.ID = new SelectList(db.OfficeAssignments, "InstructorID", "Location", instructor.ID);
         return View(instructor);
      }

      // POST: Instructor/Edit/5
      // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
      // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Edit([Bind(Include = "ID,LastName,FirstMidName,HireDate")] Instructor instructor)
      {
         if (ModelState.IsValid)
         {
            db.Entry(instructor).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
         }
         ViewBag.ID = new SelectList(db.OfficeAssignments, "InstructorID", "Location", instructor.ID);
         return View(instructor);
      }

      // GET: Instructor/Delete/5
      public ActionResult Delete(int? id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         Instructor instructor = db.Instructors.Find(id);
         if (instructor == null)
         {
            return HttpNotFound();
         }
         return View(instructor);
      }

      // POST: Instructor/Delete/5
      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public ActionResult DeleteConfirmed(int id)
      {
         Instructor instructor = db.Instructors.Find(id);
         db.Instructors.Remove(instructor);
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

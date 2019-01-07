using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFrameworkTutorial.Models
{
   public class Course
   {
      [DatabaseGenerated(DatabaseGeneratedOption.None), Display(Name = "Number")]
      public int CourseID { get; set; }
      [StringLength(50, MinimumLength = 3)]
      public string Title { get; set; }
      [Range(0,5)]
      public int Credits { get; set; }

      public int DepartamentID { get; set; }
      public virtual Department Departament { get; set; }
      public virtual ICollection<Enrollment> Enrollments { get; set; }
      public virtual ICollection<Instructor> Instructors { get; set; }
   }
}
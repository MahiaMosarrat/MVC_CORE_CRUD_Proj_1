using CoreFinal.Models;
using CoreFinal.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreFinal.Controllers
{
    public class StudentsController : Controller
    {
        private readonly AppDbContext db;
        private readonly IWebHostEnvironment web;
        public StudentsController(AppDbContext _db, IWebHostEnvironment _web)
        {
            db = _db;
            web = _web;
        }
        public IActionResult Index()
        {
            var students = db.Students.Include(s => s.Course).Include(s => s.Modules).ToList();
            return View(students);
        }
        public IActionResult Delete(int id)
        {
            var student = db.Students.Find(id);
            if(student != null)
            {
                db.Students.Remove(student);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult CreatePartial()
        {
            StudentViewModel student = new StudentViewModel();
            student.Courses = db.Courses.ToList();
            student.Modules.Add(new Module { ModuleId = 1 });
            return View("_CreateStudentPartial",student);
        }
        [HttpPost]
        public IActionResult CreateStudent(StudentViewModel vObj)
        {
            Student student = new Student
            {
                StudentName=vObj.StudentName,
                MobileNo = vObj.MobileNo,
                IsEnrolled = vObj.IsEnrolled,
                AdmissionDate = vObj.AdmissionDate,
                CourseId = vObj.CourseId,
                Modules = vObj.Modules,
                RegistrationFee = vObj.RegistrationFee,
            };
            if(vObj.Picture != null)
            {
                string fileName = GetFileName(vObj.Picture);
                student.ImageUrl = fileName;
            }
            else
            {
                student.ImageUrl = "noimage.jpg";
            }
            db.Students.Add(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private string GetFileName(IFormFile file)
        {
            string fileName = "";
            if (file != null)
            {
                fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string uploadFolder = Path.Combine(web.WebRootPath, "images");
                string path = Path.Combine(uploadFolder, fileName);
                using(var fileStream=new FileStream(path, FileMode.Create))
                {
                    file.CopyToAsync(fileStream);
                }
            }
            return fileName;
        }
        [HttpGet]
        public IActionResult EditPartial(int id)
        {
            var student= db.Students.Include(s => s.Modules).FirstOrDefault(s=>s.StudentId==id);
            var obj = new StudentViewModel
            {
                StudentId = student.StudentId,

                StudentName = student.StudentName,
                MobileNo = student.MobileNo,
                IsEnrolled = student.IsEnrolled,
                AdmissionDate = student.AdmissionDate,
                CourseId = student.CourseId,
                ImageUrl=student.ImageUrl,
                Modules = student.Modules.ToList(),
                RegistrationFee = student.RegistrationFee,
                Courses = db.Courses.ToList()
            };
          
            return View("_EditStudentPartial", obj);
        }
        [HttpPost]
        public IActionResult EditStudent(StudentViewModel vObj,string OldImageUrl)
        {
            var obj = db.Students.Find(vObj.StudentId);
            if(obj!=null)
            {
                obj.StudentName = vObj.StudentName;
                obj.MobileNo = vObj.MobileNo;
                obj.IsEnrolled = vObj.IsEnrolled;
                obj.AdmissionDate = vObj.AdmissionDate;
                obj.CourseId = vObj.CourseId;

                obj.RegistrationFee = vObj.RegistrationFee;
                if (vObj.Picture != null)
                {
                    string fileName = GetFileName(vObj.Picture);
                    obj.ImageUrl = fileName;
                }
                else
                {
                    obj.ImageUrl = OldImageUrl;
                }
                var modules = db.Modules.Where(s => s.StudentId == obj.StudentId).ToList();
                if (modules != null)
                {
                    db.Modules.RemoveRange(modules);
                }
                if(vObj.Modules != null)
                {
                    foreach (var m in vObj.Modules)
                    {
                        m.StudentId = vObj.StudentId;
                        m.ModuleName = m.ModuleName;
                        m.Duration = m.Duration;
                        db.Modules.Add(m);
                    }
                }
                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            vObj.Courses = db.Courses.ToList();
            return View(vObj);
           
           
            
        }
    }
}

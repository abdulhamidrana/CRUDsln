using MvcCRUD.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcCRUD.Controllers
{
    public class StudentController : Controller
    {
        MvcJquryAjaxCrudDBEntities db = new MvcJquryAjaxCrudDBEntities();
        public ActionResult Index()
        {
            List<tblDepartment> list = db.tblDepartments.ToList();
            ViewBag.DDL = new SelectList(list, "DepartmentId", "DepartmentName");
            return View();
        }

        public JsonResult GetStudents()
        {
            List<StudentViewModel> students = db.tblStudents.Where(s => s.IsDeleted == false).Select(x => new StudentViewModel
            {
                StudentId = x.StudentId,
                StudentName = x.StudentName,
                Email = x.Email,
                DepartmentName = x.tblDepartment.DepartmentName
            }).ToList();

            return Json(students, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStudentById(int StudentId)
        {
            tblStudent studentFromDB = db.tblStudents.Where(m => m.IsDeleted == false && m.StudentId == StudentId).FirstOrDefault();
            string value = string.Empty;
            value = JsonConvert.SerializeObject(studentFromDB, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Upsert(StudentViewModel model)
        {
            var result = false;
            try
            {
                if (model.StudentId > 0)
                {
                    tblStudent studentFromDB = db.tblStudents.Where(m => m.IsDeleted == false && m.StudentId == model.StudentId).FirstOrDefault();
                    studentFromDB.StudentName = model.StudentName;
                    studentFromDB.Email = model.Email;
                    studentFromDB.DepartmentId = model.DepartmentId;
                    db.SaveChanges();
                    result = true;
                }
                else
                {
                    tblStudent obj = new tblStudent();
                    obj.StudentName = model.StudentName;
                    obj.Email = model.Email;
                    obj.DepartmentId = model.DepartmentId;
                    obj.IsDeleted = false;

                    db.tblStudents.Add(obj);
                    db.SaveChanges();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult DeleteStudent(int StudentId)
        {
            bool result = false;
            tblStudent studentFromDB = db.tblStudents.Where(m => m.StudentId == StudentId).FirstOrDefault();
            if (studentFromDB != null)
            {
                studentFromDB.IsDeleted = true;
                db.SaveChanges();
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}
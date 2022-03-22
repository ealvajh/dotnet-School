using Schooltt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Schooltt.Controllers
{
    public class StudentController : ApiController
    {
        public IHttpActionResult GetAllStudents(bool includeAddress = false)
        {
            List<StudentModel> students = null;

            using (var db = new SchoolEntities())
            {
                students = db.Students
                  .Include("StudentAddresses")
                  .Select(s => new StudentModel()
                  {
                      StudentId = s.StudentId,
                      FirstName = s.FirstName,
                      LastName = s.LastName,
                      StandardId = s.StandardId,
                      Address = s.StudentAddress == null || includeAddress == false ? null : new AddressModel()
                      {
                          StudentId = s.StudentId,
                          Address1 = s.StudentAddress.Address1,
                          Address2 = s.StudentAddress.Address2,
                          City = s.StudentAddress.City,
                          State = s.StudentAddress.State
                      }
                  }).ToList<StudentModel>();
            }

            if (students.Count == 0)
            {
                return NotFound();
            }

            return Ok(students);

        }

        public IHttpActionResult GetStudentById(int id)
        {

            StudentModel student = null;

            using (var db = new SchoolEntities())
            {
                student = db.Students
                            .Include("StudentAddresses")
                            .Where(s => s.StudentId == id)
                            .Select(s => new StudentModel()
                            {
                                StudentId = s.StudentId,
                                FirstName = s.FirstName,
                                LastName = s.LastName

                            }).FirstOrDefault();
            }

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        public IHttpActionResult GetStudentsByName(string name)
        {
            List<StudentModel> students = null;

            using (var db = new SchoolEntities())
            {
                students = db.Students
                            .Include("StudentAddresses")
                            .Where(s => s.FirstName.ToLower() == name.ToLower())
                            .Select(s => new StudentModel()
                            {
                                StudentId = s.StudentId,
                                FirstName = s.FirstName,
                                LastName = s.LastName

                            }).ToList();
            }

            if (students.Count == 0)
            {
                return NotFound();
            }

            return Ok(students);
        }


        public IHttpActionResult GetAllStudentsInSameStandard(int standard)
        {
            List<StudentModel> students = null;

            using (var db = new SchoolEntities())
            {
                students = db.Students
                            .Include("StudentAddresses")
                            .Include("Standards")
                            .Where(s => s.StandardId == standard)
                            .Select(s => new StudentModel()
                            {
                                StudentId = s.StudentId,
                                FirstName = s.FirstName,
                                LastName = s.LastName,
                                Address = s.StudentAddress == null ? null : new AddressModel()
                                {
                                    StudentId = s.StudentId,
                                    Address1 = s.StudentAddress.Address1,
                                    Address2 = s.StudentAddress.Address2,
                                    City = s.StudentAddress.City,
                                    State = s.StudentAddress.State
                                },
                                Standard = s.Standard == null ? null : new StandardModel()
                                {
                                    Name = s.Standard.StandarName,
                                    Description = s.Standard.Description
                                }

                            }).ToList();
            }

            if (students.Count == 0)
            {
                return NotFound();
            }

            return Ok(students);
        }

        // POST method

        public IHttpActionResult PostNewStudent(StudentModel student)
        {

            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            using (var db = new SchoolEntities())
            {
                db.Students.Add(new Student()
                {
                    StudentId = student.StudentId,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    StandardId = student.StandardId

                });
                db.SaveChanges();
            }
            return Ok();
        }

        // PUT method

        public IHttpActionResult PutStudent(StudentModel student)
        {

            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            using (var db = new SchoolEntities())
            {
                var studentToModify = db.Students
                                        .Where(s => s.StudentId == student.StudentId).FirstOrDefault();

                if (studentToModify != null)
                {
                    studentToModify.FirstName = student.FirstName;
                    studentToModify.LastName = student.LastName;

                    db.SaveChanges();
                }
                else
                {
                    return NotFound();
                }

            }
            return Ok();
        }

        // DELETE method

        public IHttpActionResult DeleteStudent(int id)
        {
            if (id <= 0)
                return BadRequest(" Not a valid student id.");

            using (var db = new SchoolEntities())
            {
                var student = db.Students.Where(s => s.StudentId == id).FirstOrDefault();

                if (student != null)
                {
                    db.Entry(student).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
            }

            return Ok();
        }
    }
}
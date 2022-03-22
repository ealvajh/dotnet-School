using Schooltt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace Schooltt.Controllers
{
    public class EstudianteController : Controller
    {
        // GET: Estudiante
        public ActionResult Index()
        {
            IEnumerable<StudentModel> students = null;

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("https://localhost:44335/api/");
                
                var responseTask = client.GetAsync("Student");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<StudentModel>>();
                    readTask.Wait();

                    students = readTask.Result;
                }
                else
                {
                    students = Enumerable.Empty<StudentModel>();
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrador.");
                }
            }
            return View(students);
        }


        // POST: Estudiante

        public ActionResult Create() 
        { 

            return View(); 
        
        }

        [HttpPost]
        public ActionResult Create(StudentModel student)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44335/api/Student");

                //HTTP POST
                var postTask = client.PostAsJsonAsync<StudentModel>("student", student);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return View(student);

        }


        // PUT methods

        public ActionResult Edit(int id)
        {

            StudentModel student = null;

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("https://localhost:44335/api/");

                var responseTask = client.GetAsync("Student?id=" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<StudentModel>();
                    readTask.Wait();

                    student = readTask.Result;
                }
         
            }
            return View(student);
        }


        [HttpPost]
        public ActionResult Edit(StudentModel student)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44335/api/Student");

                //HTTP POST
                var putTask = client.PutAsJsonAsync<StudentModel>("student", student);
                putTask.Wait();

                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("Index");
                }
            }
            return View(student);
        }


        public ActionResult Delete(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44335/api/");

                //HTTP DELETE
                var deleteTask = client.DeleteAsync("student/" + id.ToString());
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }
    }
}
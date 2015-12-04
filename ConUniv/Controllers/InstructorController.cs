using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Needletail.DataAccess;
using System.Configuration;
using System.Web.Script.Serialization;
using ConUniv.Repositories;
using ConUniv.Models;
using ConUniv.ViewModels;


namespace ConUniv.Controllers
{

    //Basic Controller template, avoid adding bussiness logic code here, use the Repository instead
    public class InstructorController : Controller
    {
        public Dictionary<string, Func<Instructor, IComparable>> DynamicOrdering = new Dictionary<string, Func<Instructor, IComparable>>
        {
            {"Id",opt => opt.Id},
            {"LastName",opt => opt.LastName},
            {"FirstName",opt => opt.FirstName},
            {"HireDate",opt => opt.HireDate},
            {"Discriminator",opt => opt.Discriminator}
        };


        InstructorRepository instructorRepository;

        public InstructorController()
        {

            //Remove this line if you want to use dependency injection 

            instructorRepository = new InstructorRepository();
        }

        //
        // GET: /Instructor/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Instructor/Details/id

        public ActionResult Details(Guid id)
        {
            ViewBag.id = id;
            return View();
        }

        //
        // GET: /Instructor/Create

        public ActionResult Create()
        {
            var viewModel = new InstructorViewModel();
            viewModel.FillData(primaryKey: new Guid()); ;
            ViewBag.Instructor = new JavaScriptSerializer().Serialize(viewModel);
            return View();
        }

        //
        // POST: /Instructor/Create

        [HttpPost]
        public ActionResult Create(InstructorViewModel model)
        {

            model.Instructor.Id = Guid.NewGuid();
            model.Office = new OfficeAssignment() { InstructorID = model.Instructor.Id , Id = Guid.NewGuid() };

            model.Save();

            return Json(new { result = "Redirect", url = Url.Action("Index", "Instructor") });
        }

        //
        // GET: /Instructor/Edit/id

        public ActionResult Edit(Guid id)
        {
            ViewBag.id = id;
            return View();
        }

        //
        // POST: /Instructor/Edit/id

        [HttpPost]
        public ActionResult Edit(InstructorViewModel model)
        {
            model.Save();

            return Json(new { result = "Redirect", url = Url.Action("Index", "Instructor") });
        }


        //
        // POST: /Instructor/Delete/5

        [HttpPost, ActionName("Delete")]
        public bool Delete(Guid id)
        {
            var result = instructorRepository.Delete(where: new { Id = id });
            return result;
        }

        #region instructor data access

        [HttpPost]
        public JsonResult GetInstructors()
        {
            var instructors = instructorRepository.GetAll();

            var result = new
            {
                instructors = instructors
            };

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetInstructor(Guid id)
        {


            var instructor = new InstructorViewModel();
            instructor.FillData(primaryKey: id);

            var result = new
            {
                instructor = instructor
            };

            return Json(result);
        }


        [HttpPost]
        public JsonResult AddCourse(Guid instructorId, Guid courseId)
        { 
            using (var icRepo = new CourseInstructorRepository())
            {
                icRepo.Insert(new CourseInstructor { Id = Guid.NewGuid(), InstructorID = instructorId, CourseID = courseId});
            }
            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult DeleteCourse(Guid ciId)
        {
            using (var icRepo = new CourseInstructorRepository())
            {
                icRepo.Delete(where: new { CourseId = ciId });
            }
            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult GetAllInstructor(int page_num, int rows_per_page)
        {
            var list = new List<dynamic>();
            var sortField = this.Request.Form["sorting[0][field]"];
            var sort = this.Request.Form["sorting[0][order]"];

            var totalRows = instructorRepository.ExecuteScalar<int>("SELECT Count(Id) FROM instructor", new Dictionary<string, object>());
            var registers = instructorRepository.GetMany(
                new { },
                new { },
                Needletail.DataAccess.Engines.FilterType.AND,
                page_num - 1,
                rows_per_page
            );

            registers = (sort == "descending") ?
                registers.OrderByDescending(DynamicOrdering[sortField]) :
                registers.OrderBy(DynamicOrdering[sortField]);

            foreach (var row in registers)
            {
                var objId = row.Id.ToString();
                list.Add(new
                {
                    Id = row.Id,
                    LastName = row.LastName,
                    FirstName = row.FirstName,
                    HireDate = row.HireDate,
                    Discriminator = row.Discriminator,
                    edit = string.Format("<a href = \"/instructor/edit/{0}\">Edit</a>", objId),
                    details = string.Format("<a href = \"/instructor/Details/{0}\">Details</a>", objId),
                    delete = string.Format("<a href = \"/instructor/delete/{0}\">Delete</a>", objId)
                });
            }

            return Json(new { total_rows = totalRows, page_data = list }); ;
        }

        //By default the Id is a Guid, change it to whatever you need
        public JsonResult GetTextForAutocomplete(Guid id, string idField, string showField)
        {
            var found = instructorRepository.GetMany(
                where: string.Format("{0} = @id", idField),
                orderBy: "",
                args: new Dictionary<string, object> { { "id", id } },
                topN: null).FirstOrDefault();
            if (found == null)
                return Json(new { name = "" }, JsonRequestBehavior.AllowGet);

            var show = found.GetType().GetProperties().FirstOrDefault(p => p.Name.ToLower() == showField.ToLower());
            return Json(new { name = show.GetValue(found) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Search(string query, string searchField, string idField, string showField, string order)
        {
            query = query.Trim();
            if (!query.StartsWith("%"))
                query = "%" + query;
            if (!query.EndsWith("%"))
                query += "%";
            var found = instructorRepository.GetMany(
                where: string.Format("{0} like @query", searchField),
                orderBy: order + " DESC",
                args: new Dictionary<string, object> { { "query", query } },
                topN: null);

            if (found == null || found.Count() == 0)
                return Json(new List<object> { new { id = "00000000-0000-0000-0000-000000000000", name = string.Format("{0}: No results found", query.Replace("%", "")) } }, JsonRequestBehavior.AllowGet);

            //create the object to return
            var allFound = new List<object>();
            var id = found.First().GetType().GetProperties().FirstOrDefault(p => p.Name.ToLower() == idField.ToLower());
            var show = found.First().GetType().GetProperties().FirstOrDefault(p => p.Name.ToLower() == showField.ToLower());
            if (id == null || show == null)
                throw new Exception("Id field or Show fields doest not belog to the entity");
            foreach (var f in found)
            {
                allFound.Add(new { id = id.GetValue(f), name = f.FirstName + " " + f.LastName });
            }

            return Json(allFound, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}

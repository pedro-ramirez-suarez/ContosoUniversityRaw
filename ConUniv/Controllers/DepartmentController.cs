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
    public class DepartmentController : Controller
    {
        public Dictionary<string, Func<Department, IComparable>> DynamicOrdering = new Dictionary<string, Func<Department, IComparable>>
        {
            {"Id",opt => opt.Id},
            {"Name",opt => opt.Name},
            {"Budget",opt => opt.Budget},
            {"StartDate",opt => opt.StartDate},
            {"InstructorID",opt => opt.InstructorID}
        };

        
        DepartmentRepository departmentRepository;

        public DepartmentController() {

            //Remove this line if you want to use dependency injection 
            
        departmentRepository = new DepartmentRepository();
        }

        //
        // GET: /Department/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Department/Details/id

        public ActionResult Details(Guid id)
        {
            ViewBag.id = id;
            return View();
        }

        //
        // GET: /Department/Create

        public ActionResult Create()
        {
            var Department = new Department();
            ViewBag.Department = new JavaScriptSerializer().Serialize(Department);
            return View();
        }

        //
        // POST: /Department/Create

        [HttpPost]
        public ActionResult Create(DepartmentViewModel model)
        {
            model.Department.Id = Guid.NewGuid();
            model.Save();

            return Json(new { result = "Redirect", url = Url.Action("Index", "Department") });
        }

        //
        // GET: /Department/Edit/id

        public ActionResult Edit(Guid id)
        {
            ViewBag.id = id;
            return View();
        }

        //
        // POST: /Department/Edit/id

        [HttpPost]
        public ActionResult Edit(DepartmentViewModel model)
        {
            model.Save();

            return Json(new { result = "Redirect", url = Url.Action("Index", "Department") });
        }


        //
        // POST: /Department/Delete/5

        [HttpPost, ActionName("Delete")]
        public bool Delete(Guid id)
        {
            var result = departmentRepository.Delete(where: new {Id = id});
            return result;
        }

        #region department data access

        [HttpPost]
        public JsonResult GetDepartments()
        {
            var departments = departmentRepository.GetAll();

            var result = new {
                departments = departments
            };

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetDepartment(Guid id)
        {

            var department = new DepartmentViewModel();
            department.FillData(id);
            

            var result = new
            {
                department = department
            };

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetAllDepartment(int page_num, int rows_per_page)
        {
            var list = new List<dynamic>();
            var sortField = this.Request.Form["sorting[0][field]"];
            var sort = this.Request.Form["sorting[0][order]"];
            
            var totalRows = departmentRepository.ExecuteScalar<int>("SELECT Count(Id) FROM department", new Dictionary<string, object>());
            var registers = departmentRepository.GetMany(
                new {},
                new {},
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
                    Name = row.Name,
                    Budget = row.Budget,
                    StartDate = row.StartDate,
                    InstructorID = row.InstructorID,
                    edit = string.Format("<a href = \"/department/edit/{0}\">Edit</a>", objId),
                    details = string.Format("<a href = \"/department/Details/{0}\">Details</a>", objId),
                    delete = string.Format("<a href = \"/department/delete/{0}\">Delete</a>", objId)
                });
            }
 
            return Json(new { total_rows = totalRows, page_data = list });;
        }

        //By default the Id is a Guid, change it to whatever you need
        public JsonResult GetTextForAutocomplete(Guid id, string idField, string showField)
        {
            var found = departmentRepository.GetMany(
                where: string.Format("{0} = @id", idField),
                orderBy: "",
                args: new Dictionary<string, object> { { "id", id } },
                topN: null).FirstOrDefault();
            if(found == null)
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
            var found = departmentRepository.GetMany(
                where: string.Format("{0} like @query", searchField),
                orderBy: order + " DESC",
                args: new Dictionary<string, object> { { "query", query } },
                topN: null);

            if (found == null || found.Count() == 0)
                return Json(new List<object> { new { id = "00000000-0000-0000-0000-000000000000", name = string.Format("{0}: No results found", query.Replace("%","")) } }, JsonRequestBehavior.AllowGet);

            //create the object to return
            var allFound = new List<object>();
            var id = found.First().GetType().GetProperties().FirstOrDefault(p => p.Name.ToLower() == idField.ToLower());
            var show = found.First().GetType().GetProperties().FirstOrDefault(p => p.Name.ToLower() == showField.ToLower());
            if (id == null || show == null)
                throw new Exception("Id field or Show fields doest not belog to the entity");
            foreach (var f in found)
            { 
                allFound.Add(new { id = id.GetValue(f), name = show.GetValue(f) });
            }

            return Json(allFound, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}

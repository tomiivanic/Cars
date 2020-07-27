using Cars.WebApps.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace Cars.WebApps.Controllers
{
    public class HomeController : Controller
    {
        private CarsDBEntities _db = new CarsDBEntities();

        // GET: Home
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.ProizvođačSortParm = sortOrder == "proizvođač" ? "proizvođač_desc" : "proizvođač";
            ViewBag.ModelSortParm = sortOrder == "model" ? "model_desc" : "model";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var cars = from s in _db.Cars
                           select s;

            if (!String.IsNullOrEmpty(searchString))   
            {
                cars = cars.Where(s => s.Proizvođač.Contains(searchString));
                               //        || s.Model.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "proizvođač":
                    cars = _db.Cars.OrderBy(s => s.Proizvođač);
                    break;
                case "model":
                    cars = _db.Cars.OrderBy(s => s.Model);
                    break;
                case "proizvođač_desc":
                    cars = _db.Cars.OrderByDescending(s => s.Proizvođač);
                    break;
                case "model_desc":
                    cars = _db.Cars.OrderByDescending(s => s.Model);
                    break;
                default:
                    cars = _db.Cars.OrderBy(s => s.Id);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(cars.ToPagedList(pageNumber, pageSize));
           // return View(cars.ToList());
        }
        //public ActionResult Index()
        //{
         //   return View(_db.Cars.ToList());
        //}

        // GET: Home/Details/5
        public ActionResult Details(int id)
        {
            var carToDetails = (from n in _db.Cars
                             where n.Id == id
                             select n).First();
            return View(carToDetails);
            //return View(_db.Cars.Where(x => x.Id == id).FirstOrDefault());
        }

        // GET: Home/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Home/Create
        [HttpPost]
        public ActionResult Create([Bind(Exclude="Id")] Car carToCreate)
        {
            if (!ModelState.IsValid)
                return View();
            _db.Cars.Add(carToCreate);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Home/Edit/5
        public ActionResult Edit(int id)
        {
            var carToEdit = (from m in _db.Cars
                             where m.Id == id
                             select m).First();  
            return View(carToEdit);
        }

        // POST: Home/Edit/5
        [HttpPost]
        public ActionResult Edit(Car carToEdit)
        {
            var originalCar = (from m in _db.Cars
                               where m.Id == carToEdit.Id
                               select m).First();

            if (!ModelState.IsValid)
                return View(originalCar);

            _db.Entry(originalCar).CurrentValues.SetValues(carToEdit);
            _db.SaveChanges();

            return RedirectToAction("Index");
            
        }

        // GET: Home/Delete/5
        public ActionResult Delete(int id)
        {
            var carToDel = (from m in _db.Cars
                             where m.Id == id
                             select m).First();
            return View(carToDel);
        }

        // POST: Home/Delete/5
        [HttpPost]
        public ActionResult Delete(Car carToDel)
        {
            var delCar = (from m in _db.Cars
                               where m.Id == carToDel.Id
                               select m).First();

            if (!ModelState.IsValid)
                return View(delCar);

            _db.Cars.Remove(delCar);
            _db.SaveChanges();

            return RedirectToAction("Index");

        }
    }
}

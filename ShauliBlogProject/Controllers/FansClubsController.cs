using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShauliBlogProject.DAL;
using ShauliBlogProject.Models;

namespace ShauliBlogProject.Controllers
{
    public class FansClubsController : Controller
    {
        private ShauliBlogContext db = new ShauliBlogContext();

        // GET: FansClubs
        public ActionResult Index()
        {
            return View(db.FansClub.ToList());
        }

        [HttpPost]
        public ActionResult Index(string nameSearch, string genderSearch)
        {
            var fans = (from f in db.FansClub select f);
            if (!String.IsNullOrEmpty(nameSearch))
            {
                fans = fans.Where(s => s.LastName.Contains(nameSearch) || s.FirstName.Contains(nameSearch));
            }
            if (!String.IsNullOrEmpty(genderSearch))
            {
                fans = fans.Where(s => s.Gender.Contains(genderSearch));
            }

            return View(fans);
        }

        // GET: FansClubs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FansClub fansClub = db.FansClub.Find(id);
            if (fansClub == null)
            {
                return HttpNotFound();
            }
            return View(fansClub);
        }

        // GET: FansClubs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FansClubs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FirstName,LastName,Gender,BirthDate,Seniority,City,Country,Address,Email")] FansClub fansClub)
        {
            if (ModelState.IsValid)
            {
                if (fansClub.Gender.ToLower() == "male" || fansClub.Gender.ToLower() == "female")
                {
                    db.FansClub.Add(fansClub);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(fansClub);
        }

        // GET: FansClubs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FansClub fansClub = db.FansClub.Find(id);
            if (fansClub == null)
            {
                return HttpNotFound();
            }
            return View(fansClub);
        }

        // POST: FansClubs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FirstName,LastName,Gender,BirthDate,Seniority,City,Country,Address,Email")] FansClub fansClub)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fansClub).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fansClub);
        }

        // GET: FansClubs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FansClub fansClub = db.FansClub.Find(id);
            if (fansClub == null)
            {
                return HttpNotFound();
            }
            return View(fansClub);
        }

        // POST: FansClubs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FansClub fansClub = db.FansClub.Find(id);
            db.FansClub.Remove(fansClub);
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

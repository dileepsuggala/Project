using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VendorRegistration.Models;

namespace VendorRegistration.Controllers
{
    
    public class NewspapersController : Controller
    {
        private ProjectDBEntities db = new ProjectDBEntities();

        // GET: Newspapers
      
            
        public ActionResult Index()
        {
            return View(db.Newspapers.ToList());
        }

        // GET: Newspapers/Details/5
        
            

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Newspaper newspaper = db.Newspapers.Find(id);
            if (newspaper == null)
            {
                return HttpNotFound();
            }
            return View(newspaper);
        }

        // GET: Newspapers/Create
        
          
        public ActionResult Create()
        {
            return View();
        }

        // POST: Newspapers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Create([Bind(Include = "NewspaperId,NewspaperName,Price")] Newspaper newspaper)
        {
            if (ModelState.IsValid)
            {
                db.Newspapers.Add(newspaper);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(newspaper);
        }

        // GET: Newspapers/Edit/5
      
         
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Newspaper newspaper = db.Newspapers.Find(id);
            if (newspaper == null)
            {
                return HttpNotFound();
            }
            return View(newspaper);
        }

        // POST: Newspapers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
       

        public ActionResult Edit([Bind(Include = "NewspaperId,NewspaperName,Price")] Newspaper newspaper)
        {
            if (ModelState.IsValid)
            {
                db.Entry(newspaper).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(newspaper);
        }

        // GET: Newspapers/Delete/5
      
           
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Newspaper newspaper = db.Newspapers.Find(id);
            if (newspaper == null)
            {
                return HttpNotFound();
            }
            return View(newspaper);
        }

        // POST: Newspapers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Newspaper newspaper = db.Newspapers.Find(id);
            db.Newspapers.Remove(newspaper);
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

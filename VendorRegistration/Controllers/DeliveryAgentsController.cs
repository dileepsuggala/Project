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
   
    public class DeliveryAgentsController : Controller
    {
        private vendorEntities1 db = new vendorEntities1();

        // GET: DeliveryAgents
        [Authorize]
        public ActionResult Index()
        {
            return View(db.DeliveryAgents.ToList());
        }

        // GET: DeliveryAgents/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryAgent deliveryAgent = db.DeliveryAgents.Find(id);
            if (deliveryAgent == null)
            {
                return HttpNotFound();
            }
            return View(deliveryAgent);
        }

        // GET: DeliveryAgents/Create
        

        public ActionResult Create()
        {
            return View();
        }

        // POST: DeliveryAgents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Id,AgentName,Mobile")] DeliveryAgent deliveryAgent)
        {
            if (ModelState.IsValid)
            {
                db.DeliveryAgents.Add(deliveryAgent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(deliveryAgent);
        }

        // GET: DeliveryAgents/Edit/5
        [Authorize]

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryAgent deliveryAgent = db.DeliveryAgents.Find(id);
            if (deliveryAgent == null)
            {
                return HttpNotFound();
            }
            return View(deliveryAgent);
        }

        // POST: DeliveryAgents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AgentName,Mobile")] DeliveryAgent deliveryAgent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deliveryAgent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(deliveryAgent);
        }

        // GET: DeliveryAgents/Delete/5
        [Authorize]

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryAgent deliveryAgent = db.DeliveryAgents.Find(id);
            if (deliveryAgent == null)
            {
                return HttpNotFound();
            }
            return View(deliveryAgent);
        }

        // POST: DeliveryAgents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DeliveryAgent deliveryAgent = db.DeliveryAgents.Find(id);
            db.DeliveryAgents.Remove(deliveryAgent);
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

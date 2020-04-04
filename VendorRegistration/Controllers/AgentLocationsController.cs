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
    public class AgentLocationsController : Controller
    {
        private vendorEntities2 db = new vendorEntities2();

        // GET: AgentLocations
        [Authorize]
        public ActionResult Index()
        {
            var agentLocations = db.AgentLocations.Include(a => a.DeliveryAgent);
            return View(agentLocations.ToList());
        }

        // GET: AgentLocations/Details/5
        [Authorize]

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgentLocation agentLocation = db.AgentLocations.Find(id);
            if (agentLocation == null)
            {
                return HttpNotFound();
            }
            return View(agentLocation);
        }

        // GET: AgentLocations/Create
        [Authorize]

        public ActionResult Create()
        {
            ViewBag.AgentId = new SelectList(db.DeliveryAgents, "Id", "AgentName");
            return View();
        }

        // POST: AgentLocations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Id,AssignLocation,AgentId")] AgentLocation agentLocation)
        {
            if (ModelState.IsValid)
            {
                db.AgentLocations.Add(agentLocation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AgentId = new SelectList(db.DeliveryAgents, "Id", "AgentName", agentLocation.AgentId);
            return View(agentLocation);
        }

        [Authorize]

        // GET: AgentLocations/Edit/5
        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgentLocation agentLocation = db.AgentLocations.Find(id);
            if (agentLocation == null)
            {
                return HttpNotFound();
            }
            ViewBag.AgentId = new SelectList(db.DeliveryAgents, "Id", "AgentName", agentLocation.AgentId);
            return View(agentLocation);
        }

        // POST: AgentLocations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AssignLocation,AgentId")] AgentLocation agentLocation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(agentLocation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AgentId = new SelectList(db.DeliveryAgents, "Id", "AgentName", agentLocation.AgentId);
            return View(agentLocation);
        }

        // GET: AgentLocations/Delete/5
        [Authorize]

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgentLocation agentLocation = db.AgentLocations.Find(id);
            if (agentLocation == null)
            {
                return HttpNotFound();
            }
            return View(agentLocation);
        }

        // POST: AgentLocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AgentLocation agentLocation = db.AgentLocations.Find(id);
            db.AgentLocations.Remove(agentLocation);
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

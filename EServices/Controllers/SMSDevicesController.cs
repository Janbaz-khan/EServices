using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EServices.Models;
using EServices.Models.API;

namespace EServices.Controllers
{
    public class SMSDevicesController : Controller
    {
        private DB db = new DB();

        // GET: SMSDevices
        public ActionResult Index()
        {
            ViewBag.ActiveDevicesCount = db.SMSDevices.Where(a => a.status).Count();
            return View(db.SMSDevices.ToList());
        }

        public ActionResult Activate(int id)
        {
            int count=db.SMSDevices.Where(a => a.status).Count();
                var s = db.SMSDevices.Find(id);
            if (count>0&&!s.status)
            {
                TempData["alert-message"] = "One Device already activated,checkout in the list";
            }
            else
            {
                if (s.status)
                {
                    s.status = false;
                }
                else
                {
                    s.status = true;
                }
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        // GET: SMSDevices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SMSDevice sMSDevice = db.SMSDevices.Find(id);
            if (sMSDevice == null)
            {
                return HttpNotFound();
            }
            return View(sMSDevice);
        }

        // GET: SMSDevices/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SMSDevices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Device_Name,Device_Code,status")] SMSDevice sMSDevice)
        {
            if (ModelState.IsValid)
            {
                db.SMSDevices.Add(sMSDevice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sMSDevice);
        }

        // GET: SMSDevices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SMSDevice sMSDevice = db.SMSDevices.Find(id);
            if (sMSDevice == null)
            {
                return HttpNotFound();
            }
            return View(sMSDevice);
        }

        // POST: SMSDevices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Device_Name,Device_Code,status")] SMSDevice sMSDevice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sMSDevice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sMSDevice);
        }

        // GET: SMSDevices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SMSDevice sMSDevice = db.SMSDevices.Find(id);
            if (sMSDevice == null)
            {
                return HttpNotFound();
            }
            return View(sMSDevice);
        }

        // POST: SMSDevices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SMSDevice sMSDevice = db.SMSDevices.Find(id);
            db.SMSDevices.Remove(sMSDevice);
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

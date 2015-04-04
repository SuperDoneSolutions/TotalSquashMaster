using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TotalSquashNext.Models;

namespace TotalSquashNext.Controllers
{

    public class LadderRuleController : Controller
    {

        private PrimarySquashDBContext db = new PrimarySquashDBContext();


        // GET: LadderRule
        public ActionResult Index()
        {
            if (Session["currentUser"] == null || Session["currentUser"]=="")
            {
                TempData["message"] = "Please login to continue.";
                return RedirectToAction("VerifyLogin");
            }
            return View(db.LadderRules.ToList());
        }

        // GET: LadderRule/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["currentUser"] == null || Session["currentUser"] == "")
            {
                TempData["message"] = "Please login to continue.";
                return RedirectToAction("VerifyLogin");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LadderRule ladderRule = db.LadderRules.Find(id);
            if (ladderRule == null)
            {
                return HttpNotFound();
            }
            return View(ladderRule);
        }

        // GET: LadderRule/Create
        public ActionResult Create()
        {
            if (Session["currentUser"] == null || Session["currentUser"] == "")
            {
                TempData["message"] = "Please login to continue.";
                return RedirectToAction("VerifyLogin");
            }
            return View();
        }

        // POST: LadderRule/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LadderRuleId,challengeRange,challengeLower,numLadders")] LadderRule ladderRule)
        {
            if (Session["currentUser"] == null || Session["currentUser"] == "")
            {
                TempData["message"] = "Please login to continue.";
                return RedirectToAction("VerifyLogin");
            }
            if (ModelState.IsValid)
            {
                db.LadderRules.Add(ladderRule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ladderRule);
        }

        // GET: LadderRule/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["currentUser"] == null || Session["currentUser"] == "")
            {
                TempData["message"] = "Please login to continue.";
                return RedirectToAction("VerifyLogin");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LadderRule ladderRule = db.LadderRules.Find(id);
            if (ladderRule == null)
            {
                return HttpNotFound();
            }
            return View(ladderRule);
        }

        // POST: LadderRule/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LadderRuleId,challengeRange,challengeLower,numLadders")] LadderRule ladderRule)
        {
            if (Session["currentUser"] == null || Session["currentUser"] == "")
            {
                TempData["message"] = "Please login to continue.";
                return RedirectToAction("VerifyLogin");
            }
            if (ModelState.IsValid)
            {
                db.Entry(ladderRule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ladderRule);
        }

        // GET: LadderRule/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["currentUser"] == null || Session["currentUser"] == "")
            {
                TempData["message"] = "Please login to continue.";
                return RedirectToAction("VerifyLogin");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LadderRule ladderRule = db.LadderRules.Find(id);
            if (ladderRule == null)
            {
                return HttpNotFound();
            }
            return View(ladderRule);
        }

        // POST: LadderRule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["currentUser"] == null || Session["currentUser"] == "")
            {
                TempData["message"] = "Please login to continue.";
                return RedirectToAction("VerifyLogin");
            }
            LadderRule ladderRule = db.LadderRules.Find(id);
            db.LadderRules.Remove(ladderRule);
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

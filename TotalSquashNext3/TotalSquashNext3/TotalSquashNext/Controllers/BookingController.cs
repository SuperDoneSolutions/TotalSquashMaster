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
    public class BookingController : Controller
    {
        private PrimarySquashDBContext db = new PrimarySquashDBContext();
        
        
        // GET: Booking
        //Administrator - view all bookings
        public ActionResult Index()
        {
            var bookings = db.Bookings.Include(b => b.BookingType).Include(b => b.BookingRule).Include(b => b.Court);
            return View(bookings.ToList());
        }

        // GET: Booking/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // GET: Booking/Create
        public ActionResult Create()
        {
            if (Session["currentUser"] == null)
            {
                TempData["message"] = "Please login to continue.";
                return RedirectToAction("VerifyLogin");
            }
            ViewBag.bookingCode = new SelectList(db.BookingTypes, "bookingCode", "description");
            ViewBag.bookingRulesId = new SelectList(db.BookingRules, "bookingRuleId", "bookingRuleId");
            ViewBag.courtId = new SelectList(db.Courts, "courtId", "courtDescription");
            return View();
        }

        // POST: Booking/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "courtId,bookingNumber,bookingDate,bookingCode,userId,date,bookingRulesId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                booking.userId = (((TotalSquashNext.Models.User)Session["currentUser"]).id);
                booking.bookingDate = DateTime.Now;

                var dateHolder = (from x in db.Bookings
                                  where x.bookingDate == (DateTime)Session["datePicked"]
                                  select x.date).Single();
                

                var dateRules = (from x in db.BookingRules
                                     where x.bookingRuleId == 1
                                     select x.daysInAdvance).Single();

                var dayStart = (from x in db.BookingRules
                                     where x.bookingRuleId == 1
                                     select x.dayStart).Single();

                int numBookings = (from x in db.Bookings
                                   where x.userId == ((TotalSquashNext.Models.User)Session["currentUser"]).id
                                   select x.bookingNumber).Count();

                var numBookAllowed = (from x in db.BookingRules
                                      where x.bookingRuleId == 1
                                      select x.numOfBookings).Single();

                var timeSpanRule = (from x in db.BookingRules
                                    where x.bookingRuleId == 1
                                    select x.bookingLength).Single();

                TimeSpan bookingLength = new TimeSpan(0, timeSpanRule, 0);

                booking.bookingRulesId = 1;

                DateTime currentDate = DateTime.Now;
                DateTime datePicked = (DateTime)Session["datePicked"];
                
                

                if (dateHolder == null)
                {
                    if (datePicked.AddDays((double)dateRules) <= currentDate && datePicked.TimeOfDay >= dayStart && numBookings <= numBookAllowed)
                    {
                        db.Bookings.Add(booking);
                        db.SaveChanges();
                        return RedirectToAction("BookACourt", "BookACourt");
                    }
                    else
                    {
                        if (datePicked.AddDays((double)dateRules) > currentDate)
                        {
                            TempData["Message"] = "Sorry friend. You cannot book more than " + dateRules.ToString() + " in advance!";
                            return RedirectToAction("BookACourt", "BookACourt");
                        }
                        else if (datePicked.TimeOfDay < dayStart)
                        {
                            TempData["Message"] = "Sorry friend. You cannot book a court earlier than " + dayStart.ToString() + " am!";
                            return RedirectToAction("BookACourt", "BookACourt");
                        }
                        else if (numBookings > numBookAllowed)
                        {
                            TempData["Message"] = "Sorry friend. You cannot have more than " + numBookAllowed.ToString() + " bookings!";
                            return RedirectToAction("BookACourt", "BookACourt");
                        }
                    }
                }
                else
                {
                    List<DateTime> alternateBooking = new List<DateTime>();
                    do
                    {
                        var alternateDateHolder = (from x in db.Bookings
                                                   where x.bookingDate == ((DateTime)Session["datePicked"] + bookingLength)
                                                   select x.date).Single();

                        if (alternateDateHolder == null)
                        {
                            alternateBooking.Add(alternateDateHolder);
                        }

                    } while (alternateBooking.Count() != 3);
                    do
                    {
                        var alternateDateHolder = (from x in db.Bookings
                                                   where x.bookingDate == ((DateTime)Session["datePicked"] - bookingLength)
                                                   select x.date).Single();
                        if (alternateDateHolder == null)
                        {
                            alternateBooking.Add(alternateDateHolder);
                        }
                    } while (alternateBooking.Count() != 6);

                    
                    return RedirectToAction("BookACourt", "BookACourt"); // <----- Should work and should have a list of times available earlier and later than the desired time if unavailable BUT how do we call this shit??
                }


            }

            ViewBag.bookingCode = new SelectList(db.BookingTypes, "bookingCode", "description", booking.bookingCode);
            ViewBag.bookingRulesId = new SelectList(db.BookingRules, "bookingRuleId", "bookingRuleId", booking.bookingRulesId);
            ViewBag.courtId = new SelectList(db.Courts, "courtId", "courtDescription", booking.courtId);
            return View(booking);
        }

        // GET: Booking/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            ViewBag.bookingCode = new SelectList(db.BookingTypes, "bookingCode", "description", booking.bookingCode);
            ViewBag.bookingRulesId = new SelectList(db.BookingRules, "bookingRuleId", "bookingRuleId", booking.bookingRulesId);
            ViewBag.courtId = new SelectList(db.Courts, "courtId", "courtDescription", booking.courtId);
            return View(booking);
        }

        // POST: Booking/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "courtId,bookingNumber,bookingDate,bookingCode,userId,date,bookingRulesId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.bookingCode = new SelectList(db.BookingTypes, "bookingCode", "description", booking.bookingCode);
            ViewBag.bookingRulesId = new SelectList(db.BookingRules, "bookingRuleId", "bookingRuleId", booking.bookingRulesId);
            ViewBag.courtId = new SelectList(db.Courts, "courtId", "courtDescription", booking.courtId);
            return View(booking);
        }

        // GET: Booking/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: Booking/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Booking booking = db.Bookings.Find(id);
            db.Bookings.Remove(booking);
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

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ReservationSystem.DATA.EF;

namespace ReservationSystem.UI.MVC.Controllers
{
    public class ReservationsController : Controller
    {
        private ReservationEntities db = new ReservationEntities();

        // GET: Reservations
        public ActionResult Index()
        {
            string user = User.Identity.GetUserId();

            if (User.IsInRole("Customer"))
            {
                var reservations = db.Reservations.Include(r => r.Room).Include(r => r.UserFamilyMember).Where(r => r.UserFamilyMember.UserId == user);
                return View(reservations.ToList());

            } else
            {
                var reservations = db.Reservations.Include(r => r.Room).Include(r => r.UserFamilyMember);
                return View(reservations.ToList());
            }

        }

        // GET: Reservations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // GET: Reservations/Create
        public ActionResult Create(int? id)
        {
            if(Request.IsAuthenticated){
                string user = User.Identity.GetUserId();

                if (User.IsInRole("Customer"))
                {
                    var members = db.UserFamilyMembers.Where(u => u.UserId == user).ToList();
                    ViewBag.CustomerId = new SelectList(members, "UserFamilyMemberId", "MemberName");

                }
                else
                {
                    ViewBag.CustomerId = new SelectList(db.UserFamilyMembers, "UserFamilyMemberId", "MemberName");
                }

                if (id != null)
                {
                    var room = db.Rooms.Where(r => r.RoomId == id).ToList();
                    ViewBag.RoomName = room.FirstOrDefault().RoomType;
                    ViewBag.RoomId = new SelectList(room, "RoomId", "RoomType");
                }
                else
                {
                    ViewBag.RoomName = "River";
                    ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "RoomType");
                }

                return View();
            }
            return RedirectToAction("Login", "Account");
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReservationId,CustomerId,RoomId,NumberOfGuests,ReservationStartDate,ReservationEndDate")] Reservation reservation)
        {
            var existingReservations = db.Reservations;
            var room = db.Rooms.Where(r => r.RoomId == reservation.RoomId).FirstOrDefault();
            int roomsReserved = 0;
            bool canReserve = true;

            if (ModelState.IsValid)
            {
                if (reservation.NumberOfGuests <= room.MaxOccupancy)
                {
                    if (User.IsInRole("Customer") || User.IsInRole(""))
                    {
                        foreach (var singleReservation in existingReservations)
                        {
                            bool overlapFalse = singleReservation.ReservationEndDate <= reservation.ReservationStartDate || singleReservation.ReservationStartDate >= reservation.ReservationEndDate;

                            if (!overlapFalse)
                            {
                                roomsReserved++;
                            }

                            if (roomsReserved >= room.RoomsAvailable)
                            {
                                canReserve = false;
                                break;
                            }
                        }

                        if (canReserve)
                        {
                            reservation.DateAdded = DateTime.Now;
                            reservation.IsActive = true;
                            db.Reservations.Add(reservation);
                            db.SaveChanges();
                            return RedirectToAction("Index");

                        }
                        else
                        {
                            ViewBag.Error = "There's already a max number of reservations for the dates you have chosen. Please choose a different date range.";
                            ViewBag.RoomName = room.RoomType;
                            ViewBag.CustomerId = new SelectList(db.UserFamilyMembers, "UserFamilyMemberId", "MemberName", reservation.CustomerId);
                            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "RoomType", reservation.RoomId);
                            ViewBag.NumberOfGuests = reservation.NumberOfGuests;
                            return View(reservation);
                        }
                    }

                    reservation.DateAdded = DateTime.Now;
                    reservation.IsActive = true;
                    db.Reservations.Add(reservation);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                } else
                {
                    ViewBag.Error = "Your Number of guests exceed the amount available for the room you are trying to reserve. Please change this. ";
                    ViewBag.RoomName = room.RoomType;
                    ViewBag.CustomerId = new SelectList(db.UserFamilyMembers, "UserFamilyMemberId", "MemberName", reservation.CustomerId);
                    ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "RoomType", reservation.RoomId);
                    ViewBag.NumberOfGuests = reservation.NumberOfGuests;
                    return View(reservation);
                }        
            }
            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "RoomType", reservation.RoomId);
            ViewBag.CustomerId = new SelectList(db.UserFamilyMembers, "UserFamilyMemberId", "MemberName", reservation.UserFamilyMember.UserId);
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "RoomDescription", reservation.RoomId);
            ViewBag.CustomerId = new SelectList(db.UserFamilyMembers, "UserFamilyMemberId", "MemberName", reservation.CustomerId);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReservationId,CustomerId,RoomId,NumberOfGuests,DateAdded,IsActive,SpecialRequests,ReservationStartDate,ReservationEndDate")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "RoomDescription", reservation.RoomId);
            ViewBag.CustomerId = new SelectList(db.UserFamilyMembers, "UserFamilyMemberId", "MemberName", reservation.CustomerId);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reservation reservation = db.Reservations.Find(id);
            db.Reservations.Remove(reservation);
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

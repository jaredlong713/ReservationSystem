using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ReservationSystem.DATA.EF;

namespace ReservationSystem.UI.MVC.Controllers
{
    public class RoomsController : Controller
    {
        private ReservationEntities db = new ReservationEntities();

        // GET: Rooms
        public ActionResult Index(int? id)
        {
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "LocationName");
           
            if (id != null)
            {
                var rooms = db.Rooms.Include(r => r.Location).Where(r => r.LocationId == id).ToList();
                ViewBag.LocationName = "The " + rooms.FirstOrDefault().Location.LocationName;
                return View(rooms);

            } else
            {
                ViewBag.LocationName = "The River";
                var rooms = db.Rooms.Include(r => r.Location).ToList();
                return View(rooms);
            }

        }

        // GET: Rooms
        public ActionResult locationIndex([Bind(Include = "LocationName, LocationId")] Location location)
        {
            return RedirectToAction("Index", new { id = location.LocationId });
        }

        // GET: Rooms/Details/5
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // GET: Rooms/Create
        public ActionResult Create()
        {
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "LocationName");
            return View();
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RoomId,LocationId,RoomType,RoomDescription,Price,MaxOccupancy,IsAvailable,RoomPhoto,RoomsAvailable,RoomsTaken")] Room room, HttpPostedFileBase roomPhoto)
        {
            if (ModelState.IsValid)
            {
                string image = string.Empty;

                if (roomPhoto != null)
                {
                    image = roomPhoto.FileName;
                    string ext = image.Substring(image.LastIndexOf("."));
                    string[] okExtentions = { ".jpg", ".jpeg", ".png" };

                    if (okExtentions.Contains(ext.ToLower()))
                    {
                        image = Guid.NewGuid() + ext;

                        roomPhoto.SaveAs
                            (Server.MapPath("~/Content/images/rooms/" + image));
                    }

                    room.RoomPhoto = image;
                }

                db.Rooms.Add(room);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "LocationName", room.LocationId);
            return View(room);
        }

        // GET: Rooms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "LocationName", room.LocationId);
            return View(room);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RoomId,LocationId,RoomType,RoomDescription,Price,MaxOccupancy,IsAvailable,RoomPhoto,RoomsAvailable,RoomsTaken")] Room room, HttpPostedFileBase roomPhoto)
        {
            if (ModelState.IsValid)
            {
                string image = string.Empty;

                if (roomPhoto != null)
                {
                    image = roomPhoto.FileName;
                    string ext = image.Substring(image.LastIndexOf("."));
                    string[] okExtentions = { ".jpg", ".jpeg", ".png" };

                    if (okExtentions.Contains(ext.ToLower()))
                    {
                        image = Guid.NewGuid() + ext;

                        roomPhoto.SaveAs
                            (Server.MapPath("~/Content/images/family/" + image));
                    }

                    room.RoomPhoto = image;

                }
                else
                {
                    var currentRoom = db.Rooms.Where(r => r.RoomId == room.RoomId).FirstOrDefault();
                    room.RoomPhoto = currentRoom.RoomPhoto;
                }
                //Having to use the AddOrUpdate vs the EntityState.Modified due to this error. 
                //Attaching an entity of type 'ReservationSystem.DATA.EF.Room' failed because another entity of 
                //the same type already has the same primary key value.
                db.Set<Room>().AddOrUpdate(room);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = room.LocationId });
            }
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "LocationName", room.LocationId);
            return View(room);
        }

        // GET: Rooms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            if (room.Reservations.Count > 0)
            {
                ViewBag.Error = "*Room currently has reservations. Please remove those before proceeding. ";
            }

            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Room room = db.Rooms.Find(id);
            db.Rooms.Remove(room);
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

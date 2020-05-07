using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ReservationSystem.DATA.EF;

namespace ReservationSystem.UI.MVC.Controllers
{
    public class UserFamilyMembersController : Controller
    {
        private ReservationEntities db = new ReservationEntities();

        // GET: UserFamilyMembers
        public ActionResult Index()
        {

            string user = User.Identity.GetUserId();

            if (User.IsInRole("Customer"))
            {
                var userFamilyMembers = db.UserFamilyMembers.Include(u => u.UserDetail).Where(u => u.UserId == user);
                return View(userFamilyMembers.ToList());
            }
            else
            {
                var userFamilyMembers = db.UserFamilyMembers.Include(u => u.UserDetail);
                return View(userFamilyMembers.ToList());
            }


        }

        // GET: UserFamilyMembers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserFamilyMember userFamilyMember = db.UserFamilyMembers.Find(id);
            if (userFamilyMember == null)
            {
                return HttpNotFound();
            }
            return View(userFamilyMember);
        }

        // GET: UserFamilyMembers/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.UserDetails, "UserId", "FirstName");
            return View();
        }

        // POST: UserFamilyMembers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserFamilyMemberId,MemberName,UserId,Photo,SpecialNotes")] UserFamilyMember userFamilyMember, HttpPostedFileBase memberPhoto)
        {
            string userId = User.Identity.GetUserId();

            if (ModelState.IsValid)
            {
                string image = string.Empty;

                if (memberPhoto != null)
                {
                    image = memberPhoto.FileName;
                    string ext = image.Substring(image.LastIndexOf("."));
                    string[] okExtentions = { ".jpg", ".jpeg", ".png" };

                    if (okExtentions.Contains(ext.ToLower()))
                    {
                        image = Guid.NewGuid() + ext;

                        memberPhoto.SaveAs
                            (Server.MapPath("~/Content/images/family/" + image));
                    }

                    userFamilyMember.Photo = image;
                    userFamilyMember.UserId = userId;
                }


                db.UserFamilyMembers.Add(userFamilyMember);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.UserDetails, "UserId", "FirstName", userFamilyMember.UserId);
            return View(userFamilyMember);
        }

        // GET: UserFamilyMembers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserFamilyMember userFamilyMember = db.UserFamilyMembers.Find(id);
            if (userFamilyMember == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.UserDetails, "UserId", "FirstName", userFamilyMember.UserId);
            return View(userFamilyMember);
        }

        // POST: UserFamilyMembers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserFamilyMemberId,MemberName,UserId,Photo,SpecialNotes")] UserFamilyMember userFamilyMember, HttpPostedFileBase memberPhoto)
        {
            string userId = User.Identity.GetUserId();

            if (ModelState.IsValid)
            {
                string image = string.Empty;

                if (memberPhoto != null)
                {
                    image = memberPhoto.FileName;
                    string ext = image.Substring(image.LastIndexOf("."));
                    string[] okExtentions = { ".jpg", ".jpeg", ".png" };

                    if (okExtentions.Contains(ext.ToLower()))
                    {
                        image = Guid.NewGuid() + ext;

                        memberPhoto.SaveAs
                            (Server.MapPath("~/Content/images/family/" + image));
                    }

                    userFamilyMember.Photo = image;

                } else
                {
                    var currentMember = db.UserFamilyMembers.Where(u => u.UserFamilyMemberId == userFamilyMember.UserFamilyMemberId).FirstOrDefault();
                    userFamilyMember.Photo = currentMember.Photo;
                }

                userFamilyMember.UserId = userId;

                db.Set<UserFamilyMember>().AddOrUpdate(userFamilyMember);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.UserDetails, "UserId", "FirstName", userFamilyMember.UserId);
            return View(userFamilyMember);
        }

        // GET: UserFamilyMembers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserFamilyMember userFamilyMember = db.UserFamilyMembers.Find(id);
            if (userFamilyMember == null)
            {
                return HttpNotFound();
            }
            return View(userFamilyMember);
        }

        // POST: UserFamilyMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserFamilyMember userFamilyMember = db.UserFamilyMembers.Find(id);
            db.UserFamilyMembers.Remove(userFamilyMember);
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

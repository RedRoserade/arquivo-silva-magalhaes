﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ArquivoSilvaMagalhaes.Models.SiteModels;
using ArquivoSilvaMagalhaes.Models;
using ArquivoSilvaMagalhaes.Areas.BackOffice.ViewModels;

namespace ArquivoSilvaMagalhaes.Areas.BackOffice.Controllers
{
    public class PortalController : Controller
    {
        private ArchiveDataContext db = new ArchiveDataContext();

        // GET: /Portal/
        public async Task<ActionResult> Index()
        {
            return View(await db.ArchiveSet.ToListAsync());
        }

        // GET: /Portal/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Archive archive = await db.ArchiveSet.FindAsync(id);
            if (archive == null)
            {
                return HttpNotFound();
            }
            return View(archive);
        }

        // GET: /Portal/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Portal/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PortalViewModels model)
        {
            if (ModelState.IsValid)
            {
                var archive = new Archive
                {
                    Title = model.Title
                };

                archive.ArchiveTexts.Add(
               new ArchiveText
               {
                   LanguageCode = model.LanguageCode,
                   ArchiveHistory = model.ArchiveHistory,
                   ArchiveMission = model.ArchiveMission
               });
                
                
                archive.Contacts.Add(
                new Contact
                {
                    Name = model.Name,
                    Address = model.Address,
                    Email = model.Email,
                    ContactDetails = model.ContactDetails,
                    Service = model.Service
                });

                db.ArchiveSet.Add(archive);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: /Portal/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Archive archive = await db.ArchiveSet.FindAsync(id);
            if (archive == null)
            {
                return HttpNotFound();
            }
            return View(archive);
        }

        // POST: /Portal/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit( PortalViewModels model)
        {
            if (ModelState.IsValid)
            {
                var portal = db.ArchiveSet.Find(model.Id);
                portal.Title = model.Title;

                var text = db.ArchiveTexts.Find(model.Id);
                text.ArchiveMission = model.ArchiveMission;
                text.ArchiveHistory = model.ArchiveHistory;

                var contact = db.ArchiveContacts.Find(model.Id);
                contact.Name = model.Name;
                contact.Email = model.Email;
                contact.ContactDetails = model.ContactDetails;
                contact.Address = model.Address;
                contact.Service = model.Service;
                
                db.Entry(model).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: /Portal/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Archive archive = await db.ArchiveSet.FindAsync(id);
            if (archive == null)
            {
                return HttpNotFound();
            }
            return View(archive);
        }

        // POST: /Portal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Archive archive = await db.ArchiveSet.FindAsync(id);
            db.ArchiveSet.Remove(archive);
            await db.SaveChangesAsync();
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

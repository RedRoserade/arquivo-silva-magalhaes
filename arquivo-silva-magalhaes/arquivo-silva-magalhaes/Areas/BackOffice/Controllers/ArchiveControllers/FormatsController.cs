﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ArquivoSilvaMagalhaes.Models;
using ArquivoSilvaMagalhaes.Models.ArchiveModels;

namespace ArquivoSilvaMagalhaes.Areas.BackOffice.Controllers
{
    public class FormatsController : Controller
    {
        private ArchiveDataContext db = new ArchiveDataContext();

        // GET: BackOffice/Formats
        public async Task<ActionResult> Index()
        {
            return View(await db.Formats.ToListAsync());
        }

        // GET: BackOffice/Formats/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Format format = await db.Formats.FindAsync(id);
            if (format == null)
            {
                return HttpNotFound();
            }
            return View(format);
        }

        // GET: BackOffice/Formats/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BackOffice/Formats/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,FormatDescription")] Format format)
        {
            if (ModelState.IsValid)
            {
                db.Formats.Add(format);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(format);
        }

        // GET: BackOffice/Formats/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Format format = await db.Formats.FindAsync(id);
            if (format == null)
            {
                return HttpNotFound();
            }
            return View(format);
        }

        // POST: BackOffice/Formats/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,FormatDescription")] Format format)
        {
            if (ModelState.IsValid)
            {
                db.Entry(format).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(format);
        }

        // GET: BackOffice/Formats/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Format format = await db.Formats.FindAsync(id);
            if (format == null)
            {
                return HttpNotFound();
            }
            return View(format);
        }

        // POST: BackOffice/Formats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Format format = await db.Formats.FindAsync(id);
            db.Formats.Remove(format);
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

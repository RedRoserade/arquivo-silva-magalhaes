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
using ArquivoSilvaMagalhaes.Utilitites;

namespace ArquivoSilvaMagalhaes.Areas.BackOffice.Controllers
{
    public class NewsController : Controller
    {
        private ArchiveDataContext db = new ArchiveDataContext();

        // GET: /News/
        public async Task<ActionResult> Index()
        {
            return View(await db.NewsSet.ToListAsync());
        }

        // GET: /News/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsItem newsitem = await db.NewsSet.FindAsync(id);
            if (newsitem == null)
            {
                return HttpNotFound();
            }
            ViewBag.AreLanguagesMissing = newsitem.ReferencedNewsText.Count <= LanguageDefinitions.Languages.Count;
            return View(newsitem);
        }

        // GET: /News/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /News/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(NewsItemViewModels model)
        {
            if (ModelState.IsValid)
            {
                var newsitem = new NewsItem
                {
                    Id = model.Id,
                    CreationDate = model.CreationDate,
                    ExpiryDate = model.ExpiryDate,
                    HideAfterExpiry = model.HideAfterExpiry,
                    LastModificationDate = model.LastModificationDate,
                    Links = model.Links,
                    PublishDate = model.PublishDate,
                };
                newsitem.ReferencedNewsText.Add(
                    new NewsTexts{
                       
                       Heading=model.Heading,
                       LanguageCode=model.LanguageCode,
                       Subtitle=model.Subtitle,
                       TextContent=model.TextContent,
                       Title=model.Title
                    }
                    );
            

                db.NewsSet.Add(newsitem);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: /News/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsItem newsitem = await db.NewsSet.FindAsync(id);
            if (newsitem == null)
            {
                return HttpNotFound();
            }
            return View(newsitem);
        }

        // POST: /News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="Id,PublishDate,ExpiryDate,HideAfterExpiry,CreationDate,LastModificationDate")] NewsItem newsitem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(newsitem).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(newsitem);
        }

        // GET: /News/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsItem newsitem = await db.NewsSet.FindAsync(id);
            if (newsitem == null)
            {
                return HttpNotFound();
            }
            return View(newsitem);
        }

        // POST: /News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            NewsItem newsitem = await db.NewsSet.FindAsync(id);
            db.NewsSet.Remove(newsitem);
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

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
using ArquivoSilvaMagalhaes.Utilitites;
using ArquivoSilvaMagalhaes.Areas.BackOffice.ViewModels;

namespace ArquivoSilvaMagalhaes.Areas.BackOffice.Controllers
{
    public class BannerPhotographController : Controller
    {
        private ArchiveDataContext db = new ArchiveDataContext();

        // GET: /BackOffice/BannerPhotograph/
        public async Task<ActionResult> Index()
        {
            return View(await db.BannerPhotographSet.ToListAsync());
        }

        // GET: /BackOffice/BannerPhotograph/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BannerPhotograph bannerphotograph = await db.BannerPhotographSet.FindAsync(id);
            if (bannerphotograph == null)
            {
                return HttpNotFound();
            }
            return View(bannerphotograph);
        }

        // GET: /BackOffice/BannerPhotograph/Create
        public ActionResult Create()
        {
            var model = new BannerPhotographViewModels
            {
                LanguageCode = LanguageDefinitions.DefaultLanguage
            };
            return View(model);
        }

        // POST: /BackOffice/BannerPhotograph/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(BannerPhotographViewModels model)
        {
            if (ModelState.IsValid)
            {
                var banner = new BannerPhotograph
                {
                    Id=model.Id,
                    ImageData=model.ImageData,
                    UriPath=model.UriPath,
                    //UriPath = model.UriPath.FileName,
                    PublicationDate = model.PublicationDate,
                    RemovalDate = model.RemovalDate,
                    IsVisible = model.IsVisible
                };
                banner.SubTitle.Add(
                    new ArquivoSilvaMagalhaes.Models.SiteModels.BannerPhotograph.BannerPhotographEditTexts
                    {
                        BannerPhotograph = banner,
                        BannerTexts=model.BannerTexts,

                        LanguageCode = LanguageDefinitions.DefaultLanguage
                    }); 
                    
                db.BannerPhotographSet.Add(banner);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: /BackOffice/BannerPhotograph/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BannerPhotograph bannerphotograph = await db.BannerPhotographSet.FindAsync(id);
            if (bannerphotograph == null)
            {
                return HttpNotFound();
            }
            return View(bannerphotograph);
        }

        // POST: /BackOffice/BannerPhotograph/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="Id,ImageData,UriPath,PublicationDate,RemovalDate,IsVisible")] BannerPhotograph bannerphotograph)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bannerphotograph).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(bannerphotograph);
        }

        // GET: /BackOffice/BannerPhotograph/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BannerPhotograph bannerphotograph = await db.BannerPhotographSet.FindAsync(id);
            if (bannerphotograph == null)
            {
                return HttpNotFound();
            }
            return View(bannerphotograph);
        }

        // POST: /BackOffice/BannerPhotograph/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            BannerPhotograph bannerphotograph = await db.BannerPhotographSet.FindAsync(id);
            db.BannerPhotographSet.Remove(bannerphotograph);
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

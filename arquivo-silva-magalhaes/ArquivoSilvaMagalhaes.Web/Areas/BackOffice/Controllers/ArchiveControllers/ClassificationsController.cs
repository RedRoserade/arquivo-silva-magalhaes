﻿using ArquivoSilvaMagalhaes.Common;
using ArquivoSilvaMagalhaes.Models;
using ArquivoSilvaMagalhaes.Models.ArchiveModels;
using ArquivoSilvaMagalhaes.Models.Translations;
using ArquivoSilvaMagalhaes.ViewModels;
using PagedList;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ArquivoSilvaMagalhaes.Areas.BackOffice.Controllers.ArchiveControllers
{
    public class ClassificationsController : ArchiveControllerBase
    {
        private ITranslateableRepository<Classification, ClassificationTranslation> db;

        public ClassificationsController()
            : this(new TranslateableRepository<Classification, ClassificationTranslation>())
        {
        }

        public ClassificationsController(ITranslateableRepository<Classification, ClassificationTranslation> db)
        {
            this.db = db;
        }

        // GET: BackOffice/Classifications
        public async Task<ActionResult> Index(int pageNumber = 1, string query = "")
        {
            var model = await db.Entities
                .Include(c => c.Translations)
                .Where(c => c.Translations.Any(t => t.Value.Contains(query)))
                .OrderBy(c => c.Id)
                .Select(c => new TranslatedViewModel<Classification, ClassificationTranslation>
                {
                    Entity = c
                })
                .ToPagedListAsync(pageNumber, 10);

            ViewBag.Query = query;

            if (Request.IsAjaxRequest())
            {
                return PartialView("_ListPartial", model);
            }

            return View(model);
        }

        // GET: BackOffice/Classifications/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var classification = await db.GetByIdAsync(id);

            if (classification == null)
            {
                return HttpNotFound();
            }

            classification.Translations = classification.Translations.ToList();

            return View(classification);
        }

        // GET: BackOffice/Classifications/Create
        public ActionResult Create()
        {
            var model = new ClassificationTranslation { LanguageCode = LanguageDefinitions.DefaultLanguage };

            return View(model);
        }

        // POST: BackOffice/Classifications/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ClassificationTranslation classification)
        {
            if (DoesClassificationExist(classification))
            {
                ModelState.AddModelError("Value", ClassificationStrings.ValError_AlreadyExists);
            }


            if (ModelState.IsValid)
            {
                var c = new Classification();

                c.Translations.Add(classification);

                db.Add(c);
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(classification);
        }

        // GET: BackOffice/Classifications/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Classification classification = await db.GetByIdAsync(id);

            if (classification == null)
            {
                return HttpNotFound();
            }

            return View(classification);
        }

        // POST: BackOffice/Classifications/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Classification classification)
        {
            for (var i = 0; i < classification.Translations.Count; i++)
            {
                var pt = classification.Translations[i];
                if (DoesClassificationExist(pt))
                {
                    ModelState.AddModelError("Translations[" + i + "].Value",
                        ClassificationStrings.ValError_AlreadyExists);
                }
            }

            if (ModelState.IsValid)
            {
                foreach (var t in classification.Translations)
                {
                    db.UpdateTranslation(t);
                }

                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return View(classification);
        }

        // GET: BackOffice/Classifications/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Classification classification = await db.GetByIdAsync(id);

            if (classification == null)
            {
                return HttpNotFound();
            }

            classification.Translations = classification.Translations.ToList();
            return View(classification);
        }

        // POST: BackOffice/Classifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await db.RemoveByIdAsync(id);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        #region Translation Actions
        public async Task<ActionResult> AddTranslation(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var c = await db.GetByIdAsync(id);

            if (c == null)
            {
                return HttpNotFound();
            }

            ViewBag.Languages = LanguageDefinitions
                .GenerateAvailableLanguageDDL(c.Translations.Select(t => t.LanguageCode).ToList());

            return View(new ClassificationTranslation
                {
                    ClassificationId = c.Id
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddTranslation(ClassificationTranslation translation)
        {
            if (DoesClassificationExist(translation))
            {
                ModelState.AddModelError("Value", ClassificationStrings.ValError_AlreadyExists);
            }

            if (ModelState.IsValid)
            {
                db.AddTranslation(translation);
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            var c = await db.GetByIdAsync(translation.ClassificationId);

            if (c != null)
            {
                ViewBag.Languages = LanguageDefinitions
                    .GenerateAvailableLanguageDDL(c.Translations.Select(t => t.LanguageCode).ToList());
            }

            return View(translation);
        }

        public async Task<ActionResult> DeleteTranslation(int? id, string languageCode)
        {
            if (id == null || string.IsNullOrEmpty(languageCode) || languageCode == LanguageDefinitions.DefaultLanguage)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var tr = await db.GetTranslationAsync(id.Value, languageCode);

            if (tr == null)
            {
                return HttpNotFound();
            }

            return View(tr);
        }

        [HttpPost]
        [ActionName("DeleteTranslation")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteTranslationConfirmed(int? id, string languageCode)
        {
            if (id == null || string.IsNullOrEmpty(languageCode))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var tr = await db.GetTranslationAsync(id.Value, languageCode);

            if (tr == null)
            {
                return HttpNotFound();
            }

            await db.RemoveTranslationByIdAsync(id, languageCode);

            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        #endregion

        private bool DoesClassificationExist(ClassificationTranslation p)
        {
            return db.Set<ClassificationTranslation>()
                .Any(t =>
                    t.LanguageCode == p.LanguageCode &&
                    t.Value == p.Value &&
                    t.ClassificationId != p.ClassificationId);
        }


        public ActionResult AuxAdd()
        {
            var model = new ClassificationTranslation { LanguageCode = LanguageDefinitions.DefaultLanguage };

            return PartialView("_ClassificationFields", model);
        }

        [HttpPost]
        public async Task<ActionResult> AuxAdd(ClassificationTranslation t)
        {
            var cl = db.Entities
                .FirstOrDefault(c => c.Translations.Any(ct =>
                    ct.LanguageCode == t.LanguageCode &&
                    ct.Value == t.Value &&
                    ct.ClassificationId != t.ClassificationId)
                );

            if (cl == null)
            {
                cl = new Classification();
                cl.Translations.Add(t);

                db.Add(cl);
                await db.SaveChangesAsync();
            }

            return Json((await db.Entities
                .OrderBy(ct => ct.Id)
                .ToListAsync())
                .Select(ct => new TranslatedViewModel<Classification, ClassificationTranslation>(ct))
                .Select(ct => new
                {
                    value = ct.Entity.Id.ToString(),
                    text = ct.Translation.Value,
                    selected = ct.Entity.Id == cl.Id
                })
                .ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && db != null)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
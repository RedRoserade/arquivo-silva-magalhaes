﻿using ArquivoSilvaMagalhaes.Areas.BackOffice.ViewModels.ArchiveViewModels;
using ArquivoSilvaMagalhaes.Common;
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
    public class DocumentsController : ArchiveControllerBase
    {
        private ITranslateableRepository<Document, DocumentTranslation> db;

        public DocumentsController()
            : this(new TranslateableRepository<Document, DocumentTranslation>()) { }

        public DocumentsController(TranslateableRepository<Document, DocumentTranslation> db)
        {
            this.db = db;
        }

        // GET: BackOffice/Documents
        public async Task<ActionResult> Index(int collectionId = 0, int pageNumber = 1, string query = "", int authorId = 0)
        {
            var model = await db.Entities
                .Include(doc => doc.Translations)
                .Where(doc =>
                    (collectionId == 0 || doc.CollectionId == collectionId) &&
                    (authorId == 0 || doc.AuthorId == authorId) &&
                    (query == "" || doc.CatalogCode.Contains(query) || doc.Title.Contains(query)))
                .OrderBy(doc => doc.Id)
                .Select(doc => new TranslatedViewModel<Document, DocumentTranslation>
                {
                    Entity = doc
                })
                .ToPagedListAsync(pageNumber, 10);

            ViewBag.AuthorId = authorId;
            ViewBag.CollectionId = collectionId;
            ViewBag.Query = query;

            if (Request.IsAjaxRequest())
            {
                return PartialView("_ListPartial", model);
            }

            return View(model);
        }

        // GET: BackOffice/Documents/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Document document = await db.GetByIdAsync(id);

            if (document == null)
            {
                return HttpNotFound();
            }

            document.Translations = document.Translations.ToList();

            return View(document);
        }

        // GET: BackOffice/Documents/Create
        public async Task<ActionResult> Create(int? collectionId, int? authorId)
        {
            var doc = new Document();

            doc.Translations.Add(new DocumentTranslation
            {
                LanguageCode = LanguageDefinitions.DefaultLanguage
            });

            // Check for a collection.
            if (collectionId != null && db.Set<Collection>().Any(c => c.Id == collectionId))
            {
                doc.CatalogCode = CodeGenerator.SuggestDocumentCode(collectionId.Value);
                doc.CollectionId = collectionId.Value;
            }

            // Check for an author.
            if (authorId != null && db.Set<Author>().Any(a => a.Id == authorId))
            {
                doc.AuthorId = authorId.Value;
            }

            var model = new DocumentEditViewModel(doc);
            await model.PopulateDropDownLists(db.Set<Author>(), db.Set<Collection>());

            return View(model);
        }

        // POST: BackOffice/Documents/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Document document)
        {
            if (DoesCodeAlreadyExist(document))
            {
                ModelState.AddModelError("CatalogCode", DocumentStrings.CodeAlreadyExists);
            }

            if (ModelState.IsValid)
            {
                db.Add(document);
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            var model = new DocumentEditViewModel(document);
            await model.PopulateDropDownLists(db.Set<Author>(), db.Set<Collection>());

            return View(model);
        }

        // GET: BackOffice/Documents/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Document document = await db.GetByIdAsync(id);

            if (document == null)
            {
                return HttpNotFound();
            }

            document.Translations = document.Translations.ToList();

            var model = new DocumentEditViewModel(document);
            await model.PopulateDropDownLists(db.Set<Author>(), db.Set<Collection>());

            return View(model);
        }

        // POST: BackOffice/Documents/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Document document)
        {
            if (DoesCodeAlreadyExist(document))
            {
                ModelState.AddModelError("CatalogCode", DocumentStrings.CodeAlreadyExists);
            }

            if (ModelState.IsValid)
            {
                foreach (var t in document.Translations)
                {
                    db.UpdateTranslation(t);
                }

                db.Update(document);

                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            var model = new DocumentEditViewModel(document);
            await model.PopulateDropDownLists(db.Set<Author>(), db.Set<Collection>());

            return View(model);
        }

        // GET: BackOffice/Documents/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = await db.GetByIdAsync(id);

            if (document == null)
            {
                return HttpNotFound();
            }

            document.Translations = document.Translations.ToList();

            return View(document);
        }

        // POST: BackOffice/Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await db.RemoveByIdAsync(id);
            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> AddTranslation(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var doc = await db.GetByIdAsync(id);

            if (doc == null)
            {
                return HttpNotFound();
            }

            var translations = doc.Translations
                                  .Select(dt => dt.LanguageCode)
                                  .ToList();

            if (translations.Count() == LanguageDefinitions.Languages.Count)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = new DocumentTranslation
            {
                DocumentId = doc.Id
            };

            ViewBag.Languages =
                LanguageDefinitions.GenerateAvailableLanguageDDL(doc.Translations.Select(t => t.LanguageCode).ToList());

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddTranslation(DocumentTranslation translation)
        {
            if (ModelState.IsValid)
            {
                db.AddTranslation(translation);

                await db.SaveChangesAsync();

                return RedirectToAction("Index");
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

        public async Task<ActionResult> SuggestCode(int parentId = 0, int entityId = 0)
        {
            if (parentId == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (entityId != 0)
            {
                var doc = await db.GetByIdAsync(entityId);

                if (doc.CollectionId == parentId)
                {
                    return Json(doc.CatalogCode, JsonRequestBehavior.AllowGet);
                }
            }

            var suggestedCode = CodeGenerator.SuggestDocumentCode(parentId);

            return Json(suggestedCode, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AuxList(string query = "", int pageNumber = 1)
        {
            return PartialView("_AuxList", db.Entities
                .Where(c => query == "" || c.CatalogCode.Contains(query) || c.Title.Contains(query))
                .OrderBy(c => c.Id)
                .Select(doc => new TranslatedViewModel<Document, DocumentTranslation>
                {
                    Entity = doc
                })
                .ToPagedList(pageNumber, 10));
        }

        private bool DoesCodeAlreadyExist(Document doc)
        {
            return db.Entities
                .Any(d => d.CatalogCode == doc.CatalogCode && d.Id != doc.Id);
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
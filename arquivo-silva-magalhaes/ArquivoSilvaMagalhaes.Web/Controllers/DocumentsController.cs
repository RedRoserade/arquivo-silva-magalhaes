﻿using ArquivoSilvaMagalhaes.Models;
using ArquivoSilvaMagalhaes.Models.ArchiveModels;
using ArquivoSilvaMagalhaes.ViewModels;
using PagedList;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ArquivoSilvaMagalhaes.Controllers
{
    public class DocumentsController : Controller
    {
        /// <summary>
        /// Associa Entidade Documents às traduções existentes
        /// </summary>
        private ITranslateableRepository<Document, DocumentTranslation> db;

        public DocumentsController()
            : this(new TranslateableRepository<Document, DocumentTranslation>()) { }

        public DocumentsController(ITranslateableRepository<Document, DocumentTranslation> db)
        {
            this.db = db;
        }

        /// <summary>
        /// Fornece uma lista paginada de documentos existentes, na coleção acessida
        /// previamente, ordenados pela sua data de criação
        /// </summary>
        /// <param name="collectionId"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public async Task<ActionResult> Index(int collectionId = 0, int pageNumber = 1, string query = "", int authorId = 0)
        {
            var model = (await db.Entities
                .Include(doc => doc.Translations)
                .Where(doc =>
                    (doc.Collection.IsVisible) &&
                    (collectionId == 0 || doc.CollectionId == collectionId) &&
                    (authorId == 0 || doc.AuthorId == authorId) &&
                    (query == "" || doc.Title.Contains(query)))
                .ToListAsync())
                .Select(doc => new TranslatedViewModel<Document, DocumentTranslation>(doc))
                .ToPagedList(pageNumber, 10);

            ViewBag.AuthorId = authorId;
            ViewBag.CollectionId = collectionId;
            ViewBag.Query = query;

            if (Request.IsAjaxRequest())
            {
                return PartialView("_DocumentList", model);
            }

            return View(model);
        }

        /// <summary>
        /// Fornece os detalhes de um determinado documento
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Details(int? id, int pageNumber = 1)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Document document = await db.GetByIdAsync(id);

            if (document == null || !document.Collection.IsVisible)
            {
                return HttpNotFound();
            }

            return View(new DocumentDetailsViewModel
                {
                    Collection = new TranslatedViewModel<Collection, CollectionTranslation>(document.Collection),
                    Document = new TranslatedViewModel<Document, DocumentTranslation>(document),
                    Images = document.Images
                                     .Where(i => i.IsVisible)
                                     .ToList()
                                     .Select(i => new TranslatedViewModel<Image, ImageTranslation>(i))
                                     .ToPagedList(pageNumber, 2)
                });
        }

        /// <summary>
        /// Actualização à base de dados
        /// </summary>
        /// <param name="disposing"></param>
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

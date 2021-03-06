﻿using ArquivoSilvaMagalhaes.Models;
using ArquivoSilvaMagalhaes.Models.ArchiveModels;
using ArquivoSilvaMagalhaes.Models.SiteModels;
using ArquivoSilvaMagalhaes.ViewModels;
using PagedList;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ArquivoSilvaMagalhaes.Controllers
{
    public class HomeController : Controller
    {
        private ArchiveDataContext db = new ArchiveDataContext();

        /// <summary>
        /// Página de about
        /// </summary>
        /// <returns></returns>
        public ActionResult About()
        {
            return View();
        }


        /// <summary>
        /// Fornece ViewModel que agrega atributos de BannerModel e SpotLightVideoModel
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            var model = new IndexViewModel
            {
                Banners = (await db.Banners.ToListAsync())
                                   .Select(b => new TranslatedViewModel<Banner, BannerTranslation>
                                   {
                                       Entity = b
                                   })
                                   .ToList(),

                VideoId = (await db.Configurations
                                   .FindAsync(AppConfiguration.VideoUrlKey))
                                   .Value
            };

            return View(model);
        }

        /// <summary>
        /// Forence os atributos listados contidos no model ArchiveTranslations
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> History()
        {
            ViewBag.VideoId = (await db.Configurations.FindAsync(AppConfiguration.VideoUrlKey)).Value;

            return View(new TranslatedViewModel<Archive, ArchiveTranslation>(await db.Archives.FirstOrDefaultAsync()));
        }


        public ActionResult Archive()
        {
            return View();
        }


        /// <summary>
        /// Loja ainda não existe (poderá ser desenvolvida futuramente)
        /// </summary>
        /// <returns></returns>
        //public ActionResult Shop()
        //{
        //    return View();
        //}

        /// <summary>
        /// Estabelece qual a lingua a ser usada no portal
        /// </summary>
        /// <param name="lang"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public ActionResult SetLanguage(string lang, string returnUrl)
        {
            Response.SetCookie(new HttpCookie("lang", lang));

            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Página de contactos estática
        /// </summary>
        /// <returns></returns>
        public ActionResult Contacts()
        {
            return View();
        }

        /// <summary>
        /// Forence a lista de parcerias
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Partnerships()
        {
            return View(await db.Partnerships.ToListAsync());
        }

        public async Task<ActionResult> References()
        {
            var model = new ReferencesViewModel
            {
                ReferencedLinks = await db.ReferencedLinks
                    .Select(l => new TranslatedViewModel<ReferencedLink, ReferencedLinkTranslation>
                    {
                        Entity = l
                    })
                    .ToListAsync(),
                TechnicalDocuments = await db.TechnicalDocuments.ToListAsync()
            };

            return View(model);
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
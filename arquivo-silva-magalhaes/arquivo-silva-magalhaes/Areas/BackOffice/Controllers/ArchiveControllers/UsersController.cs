﻿using ArquivoSilvaMagalhaes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArquivoSilvaMagalhaes.Areas.BackOffice.Controllers.ArchiveControllers
{
    public class UsersController : Controller
    {
        ApplicationDbContext _db = new ApplicationDbContext();

        // GET: BackOffice/Users
        public ActionResult Index()
        {
            //var model = _db.Users
            //    .OrderBy(u => u.Id)
            //    .Select(u => new )


            return View();
        }

        // GET: BackOffice/Users/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BackOffice/Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BackOffice/Users/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: BackOffice/Users/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BackOffice/Users/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: BackOffice/Users/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BackOffice/Users/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}

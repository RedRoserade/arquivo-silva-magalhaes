﻿using System.Web.Mvc;

namespace ArquivoSilvaMagalhaes.Areas.BackOffice
{
    public class BackOfficeAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "BackOffice";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "BackOffice_default",
                "BackOffice/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
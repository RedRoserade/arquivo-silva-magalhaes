﻿using ArquivoSilvaMagalhaes.Models.SiteModels;
using ArquivoSilvaMagalhaes.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ArquivoSilvaMagalhaes.Models.SiteViewModels
{
    public class TechnicalDocumentEditViewModel
    {
        public TechnicalDocument TechnicalDocument { get; set; }

        [Required]
        [Display(ResourceType = typeof(DataStrings), Name = "File")]
        public HttpPostedFileBase UploadedFile { get; set; }
    }
}
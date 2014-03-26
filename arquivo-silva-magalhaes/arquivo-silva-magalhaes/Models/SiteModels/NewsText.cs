﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArquivoSilvaMagalhaes.Models.SiteModels
{
    public class NewsText
    {
        public string CodLanguage { get; set; }
        public string CodNewsItem { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Heading { get; set; }
        public string TextContent { get; set; }


        public virtual ICollection<Event> NewsUsingNewsText { get; set; }

    }
}
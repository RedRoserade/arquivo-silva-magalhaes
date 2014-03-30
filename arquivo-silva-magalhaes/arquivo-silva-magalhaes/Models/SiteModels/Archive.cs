﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ArquivoSilvaMagalhaes.Models.SiteModels
{
    public class Archive
    {
        public Archive()
        {
            ArchiveTexts = new HashSet<ArchiveText>();
            Contacts = new HashSet<Contact>();
        }

        [Key]
        public int Id { get; set; }
        public ICollection<ArchiveText> ArchiveTexts;
        public ICollection<Contact> Contacts;
    }

    public class ArchiveText
    {
        public ArchiveText()
        {
            LanguageCode = "pt";
        }

        [Key, Column(Order = 0)]
        public int Id { get; set; }
        [Key, Column(Order = 1)]
        public string LanguageCode { get; set; }
        public string ArchiveHistory { get; set; }
        public string ArchiveMission { get; set; }

        public virtual Archive Archive { get; set; }
    }

    public class Contact
    {
        public int Id { get; set; }
        public string Name;
        public string Email;
        public string Address;
        public string ContactDetails;
        public string Service;

        public Archive Archive { get; set; }
    }
}
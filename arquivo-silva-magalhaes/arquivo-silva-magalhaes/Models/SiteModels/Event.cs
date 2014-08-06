﻿using ArquivoSilvaMagalhaes.Resources;
using ArquivoSilvaMagalhaes.Resources.ModelTranslations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArquivoSilvaMagalhaes.Models.SiteModels
{
    public enum EventType : byte
    {
        [Display(ResourceType = typeof(EventStrings), Name = "EventType_Expo")]
        Expo = 1,
        [Display(ResourceType = typeof(EventStrings), Name = "EventType_School")]
        School = 2,
        [Display(ResourceType = typeof(EventStrings), Name = "EventType_Workshop")]
        Workshop = 3,
        [Display(ResourceType = typeof(EventStrings), Name = "EventType_Other")]
        Other = 100
    }

    public class Event
    {
        public Event()
        {
            Translations = new List<EventTranslation>();
            Partnerships = new List<Partnership>();
            Collaborators = new List<Collaborator>();
            Links = new List<ReferencedLink>();
            AttachedDocuments = new List<Attachment>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(ResourceType = typeof(EventStrings), Name = "Place")]
        public string Place { get; set; }

        [Display(ResourceType = typeof(EventStrings), Name = "Coordinates")]
        public string Coordinates { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(EventStrings), Name = "VisitorInformation")]
        public string VisitorInformation { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(EventStrings), Name = "StartMoment")]
        public DateTime StartMoment { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(EventStrings), Name = "EndMoment")]
        public DateTime EndMoment { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(EventStrings), Name = "PublishDate")]
        public DateTime PublishDate { get; set; }

        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(EventStrings), Name = "ExpiryDate")]
        public DateTime? ExpiryDate { get; set; }

        [Display(ResourceType = typeof(EventStrings), Name = "HideAfterExpiry")]
        public bool HideAfterExpiry { get; set; }

        /// <summary>
        /// The type of this event.
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(EventStrings), Name = "EventType")]
        public EventType? EventType { get; set; }

        //public virtual IList<Event> ReferencedEvents { get; set; }
        public virtual IList<Partnership> Partnerships { get; set; }
        public virtual IList<Collaborator> Collaborators { get; set; }
        public virtual IList<ReferencedLink> Links { get; set; }
        public virtual IList<Attachment> AttachedDocuments { get; set; }
        public virtual IList<EventTranslation> Translations { get; set; }
       
    }

    public partial class EventTranslation
    {

        [Key, Column(Order = 0)]
        public int EventId { get; set; }
        [ForeignKey("EventId")]
        public Event Event { get; set; }

        [Key, Column(Order = 1)]
        public string LanguageCode { get; set; }

        [Required]
        [Display(ResourceType = typeof(EventStrings), Name = "Title")]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(EventStrings), Name = "Heading")]
        public string Heading { get; set; }

        [Required]
        [DataType(DataType.Html)]
        [AllowHtml]
        [Display(ResourceType = typeof(EventStrings), Name = "TextContent")]
        public string TextContent { get; set; }

    }
}
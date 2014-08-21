﻿using ArquivoSilvaMagalhaes.Models.Translations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArquivoSilvaMagalhaes.Models.ArchiveModels
{
    /// <summary>
    /// Defines a classification of a photographic specimen.
    /// </summary>
    public class Classification
    {
        public Classification()
        {
            this.Translations = new List<ClassificationTranslation>();
            this.Specimens = new List<Specimen>();
        }

        [Key]
        public int Id { get; set; }

        public virtual IList<ClassificationTranslation> Translations { get; set; }
        public virtual IList<Specimen> Specimens { get; set; }
    }

    /// <summary>
    /// Details about a classification.
    /// </summary>
    public class ClassificationTranslation
    {

        [Key, Column(Order = 0)]
        public int ClassificationId { get; set; }

        [Key, Column(Order = 1), Required]
        public string LanguageCode { get; set; }

        /// <summary>
        /// The classification details.
        /// </summary>
        [Required] 
        [Index(IsUnique = true)]
        [MaxLength(80)]
        [Display(ResourceType = typeof(ClassificationStrings), Name = "Value")]
        public string Value { get; set; }

        [ForeignKey("ClassificationId")]
        public virtual Classification Classification { get; set; }
    }
}
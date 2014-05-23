﻿using ArquivoSilvaMagalhaes.Resources;
using ArquivoSilvaMagalhaes.Utilitites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading;
using System.Web;

namespace ArquivoSilvaMagalhaes.Models.ArchiveModels
{
    /// <summary>
    /// Defines an author of a collection, or an author of a document.
    /// </summary>
    public partial class Author : IValidatableObject
    {
        public Author()
        {
            this.AuthorTexts = new HashSet<AuthorText>();
            this.Documents = new HashSet<Document>();
            this.Collections = new HashSet<Collection>();
        }

        [Key]
        public int Id { get; set; }

        /// <summary>
        /// The first name(s) of this author.
        /// </summary>
        [Required]
        [MaxLength(50)]
        [Display(ResourceType = typeof(DataStrings), Name = "FirstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// The last name(s) of this author.
        /// </summary>
        [Required]
        [MaxLength(30)]
        [Display(ResourceType = typeof(DataStrings), Name = "LastName")]
        public string LastName { get; set; }

        /// <summary>
        /// The date on which this author was born.
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(DataStrings), Name = "BirthDate")]
        public DateTime BirthDate { get; set; }
        /// <summary>
        /// The date on which this author died.
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(DataStrings), Name = "DeathDate")]
        public DateTime DeathDate { get; set; }

        /// <summary>
        /// Localized texts descibing the biography and other aspects of this
        /// author.
        /// </summary>
        public virtual ICollection<AuthorText> AuthorTexts { get; set; }
        /// <summary>
        /// Documents created by this author.
        /// </summary>
        public virtual ICollection<Document> Documents { get; set; }
        /// <summary>
        /// Collections created by this author.
        /// </summary>
        public virtual ICollection<Collection> Collections { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // The death date cannot be earlier than the birth date.
            if (DeathDate.CompareTo(BirthDate) < 0)
            {
                yield return new ValidationResult(ErrorStrings.DeathDateEarlierThanBirthDate);
            }

        }

        #region Non-mapped attributes
        [NotMapped]
        public string Name
        {
            get
            {
                return String.Format("{0}, {1}", LastName, FirstName);
            }
        }

        [NotMapped]
        [Required]
        [Display(ResourceType = typeof(DataStrings), Name = "Biography")]
        public string Biography
        {
            get
            {
                var authText = this.AuthorTexts
                    .First(text => Thread.CurrentThread.CurrentUICulture.ToString().Contains(text.LanguageCode) || 
                        text.LanguageCode.Contains(LanguageDefinitions.DefaultLanguage));

                return authText.Biography;
            }
        }

        [NotMapped]
        [Required]
        [Display(ResourceType = typeof(DataStrings), Name = "Curriculum")]
        public string Curriculum
        {
            get
            {
                var authText = this.AuthorTexts
                    .First(text => Thread.CurrentThread.CurrentUICulture.ToString().Contains(text.LanguageCode) || 
                        text.LanguageCode.Contains(LanguageDefinitions.DefaultLanguage));

                return authText.Curriculum;
            }
        }

        [NotMapped]
        [Required]
        [Display(ResourceType = typeof(DataStrings), Name = "Nationality")]
        public string Nationality
        {
            get
            {
                var authText = this.AuthorTexts
                    .First(text => Thread.CurrentThread.CurrentUICulture.ToString().Contains(text.LanguageCode) ||
                        text.LanguageCode.Contains(LanguageDefinitions.DefaultLanguage));

                return authText.Nationality;
            }
        }
        #endregion
    }

    /// <summary>
    /// Text details about this author.
    /// </summary>
    public partial class AuthorText
    {
        /// <summary>
        /// The author which is associated with this detail text.
        /// </summary>
        [Key]
        public int Id { get; set; }
        [Display(ResourceType = typeof(DataStrings), Name = "LanguageCode")]
        public string LanguageCode { get; set; }

        /// <summary>
        /// The nationality of this author. eg. Portuguese.
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(DataStrings), Name = "Nationality")]
        public string Nationality { get; set; }

        /// <summary>
        /// Text containing the biography of this author.
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(DataStrings), Name = "Biography")]
        public string Biography { get; set; }

        /// <summary>
        /// Text containing the curriculum of this author.
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(DataStrings), Name = "Curriculum")]
        public string Curriculum { get; set; }

        [Required]
        public virtual Author Author { get; set; }
        
    }
}
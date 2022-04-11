using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace CoreLibraryProj
{
    public partial class Language
    {
        public Language()
        {
            DocumentFullTexts = new HashSet<DocumentFullText>();
        }

        public int Id { get; set; }
        [Display(Name ="Мова")]
        public string? LanguageName { get; set; }

        public virtual ICollection<DocumentFullText> DocumentFullTexts { get; set; }
    }
}

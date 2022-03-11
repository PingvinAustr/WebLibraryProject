using System;
using System.Collections.Generic;

namespace CoreLibraryProj
{
    public partial class Language
    {
        public Language()
        {
            DocumentFullTexts = new HashSet<DocumentFullText>();
        }

        public int Id { get; set; }
        public string? LanguageName { get; set; }

        public virtual ICollection<DocumentFullText> DocumentFullTexts { get; set; }
    }
}

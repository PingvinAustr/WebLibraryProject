using System;
using System.Collections.Generic;

namespace CoreLibraryProj
{
    public partial class DocumentFullText
    {
        public int Id { get; set; }
        public int? DocumentId { get; set; }
        public int? LanguageId { get; set; }
        public string? FullDocumentText { get; set; }

        public virtual Book? Document { get; set; }
        public virtual Language? Language { get; set; }
    }
}

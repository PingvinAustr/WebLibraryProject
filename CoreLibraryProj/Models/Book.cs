using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace CoreLibraryProj
{
    public partial class Book
    {
        public Book()
        {
            DocumentFullTexts = new HashSet<DocumentFullText>();
        }

        public int Id { get; set; }
        [Display(Name ="Назва")]
        public string? BookName { get; set; }

        [Display(Name = "Короткий опис")]
        public string? BookDescription { get; set; }
        public int? BookRubricId { get; set; }
        public int? BookAuthorId { get; set; }

        [Display(Name = "Автор")]
        public virtual Author? BookAuthor { get; set; }
        [Display(Name = "Рубрика")]
        public virtual Rubric? BookRubric { get; set; }
        public virtual ICollection<DocumentFullText> DocumentFullTexts { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace CoreLibraryProj
{
    public partial class Author
    {
        public Author()
        {
            Books = new HashSet<Book>();
        }

        [Display(Name ="ID автора")]
        public int Id { get; set; }

        [Display(Name ="Ім'я автора")]
        public string? AuthorName { get; set; }
        public DateTime? AuthorBirthDate { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}

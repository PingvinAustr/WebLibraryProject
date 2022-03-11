using System;
using System.Collections.Generic;

namespace CoreLibraryProj
{
    public partial class Author
    {
        public Author()
        {
            Books = new HashSet<Book>();
        }

        public int Id { get; set; }
        public string? AuthorName { get; set; }
        public DateTime? AuthorBirthDate { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}

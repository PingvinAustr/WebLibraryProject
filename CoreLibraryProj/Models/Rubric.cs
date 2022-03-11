using System;
using System.Collections.Generic;

namespace CoreLibraryProj
{
    public partial class Rubric
    {
        public Rubric()
        {
            Books = new HashSet<Book>();
        }

        public int Id { get; set; }
        public string? RubricName { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}

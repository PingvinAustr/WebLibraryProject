using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoreLibraryProj
{
    public partial class Rubric
    {
        public Rubric()
        {
            Books = new HashSet<Book>();
        }

        public int Id { get; set; }
        [Display(Name ="Назва рубрики")]
        public string? RubricName { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}

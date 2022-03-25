using System.Collections.Generic;


namespace CoreLibraryProj.Models
{
    public class IndexViewModel
    {
        public IEnumerable<string> TextItems { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}

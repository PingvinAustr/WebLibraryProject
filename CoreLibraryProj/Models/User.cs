using Microsoft.AspNetCore.Identity;

namespace CoreLibraryProj
{
    public class User : IdentityUser
    {
        public int Year { get; set; }
    }
}

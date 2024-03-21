using Org.BouncyCastle.Asn1.Ocsp;

namespace HelloJob.Entities.Models
{
    public class AppUser: Microsoft.AspNetCore.Identity.IdentityUser
    {
        public bool IsActivate { get; set; } = true;
        public List<Resume> Resumes { get; set; }

        public AppUser()
        {
            Resumes =new List<Resume>();
        }
    }
}

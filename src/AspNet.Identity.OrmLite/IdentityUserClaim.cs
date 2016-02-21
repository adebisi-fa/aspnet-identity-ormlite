
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNet.Identity.OrmLite
{
    public partial class IdentityUserClaim : IdentityUserClaim<string>
    { }

    public partial class IdentityUserClaim<TKey>
    {
        public IdentityUserClaim()
        {
            Id = Guid.NewGuid().ToString();
        }
        [StringLength(56)]
        [Required]
        public string Id { get; set; }

        [StringLength(56)]
        [ForeignKey(typeof(IdentityUser), OnDelete ="CASCADE")]
        [Required]
        public TKey UserId { get; set; }

        public string ClaimValue { get; set; }

        public string ClaimType { get; set; }
    }
}

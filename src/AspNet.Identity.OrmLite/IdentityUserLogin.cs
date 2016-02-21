
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNet.Identity.OrmLite
{
    public partial class IdentityUserLogin:IdentityUserLogin<string> { }

    public partial class IdentityUserLogin<TKey>
    {
        public IdentityUserLogin()
        {
            Id = Guid.NewGuid().ToString();
        }
        [StringLength(56)]
        [Required]
        public string Id { get; set; }

        [StringLength(56)]
        [ForeignKey(typeof(IdentityUser), OnDelete = "CASCADE")]
        [Required]
        public TKey UserId { get; set; }

        [StringLength(128)]
        [Required]
        public string LoginProvider { get; set; }

        [StringLength(128)]
        [Required]
        public string ProviderKey { get; set; }
    }
}

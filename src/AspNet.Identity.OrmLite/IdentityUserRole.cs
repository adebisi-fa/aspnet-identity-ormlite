
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNet.Identity.OrmLite
{
    public partial class IdentityUserRole : IdentityUserRole<string, string> { }

    public partial class IdentityUserRole<TKey, TRoleKey>
    {
        public IdentityUserRole()
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

        [StringLength(56)]
        [ForeignKey(typeof(IdentityRole), OnDelete = "CASCADE")]
        [Required]
        public TRoleKey RoleId { get; set; }
    }
}

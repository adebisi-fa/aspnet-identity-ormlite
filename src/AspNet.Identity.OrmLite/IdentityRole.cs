using Microsoft.AspNet.Identity;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNet.Identity.OrmLite
{
    public class IdentityRole : IdentityRole<string>
    {
        public IdentityRole()
        {
            Id = Guid.NewGuid().ToString();
        }
        public IdentityRole(string name)
            : base(Guid.NewGuid().ToString(), name)
        {
        }
        public IdentityRole(string id, string name)
            : base(id, name)
        {
        }
    }
    public class IdentityRole<TKey> : IRole<TKey>
    {
        /// <summary>
        /// Default constructor for Role 
        /// </summary>
        public IdentityRole()
        {
        }
        /// <summary>
        /// Constructor that takes names as argument 
        /// </summary>
        /// <param name="name"></param>
        public IdentityRole(string name)
            : this()
        {
            Name = name;
        }

        public IdentityRole(TKey id, string name) : this(name)
        {
            Id = id;
        }

        /// <summary>
        /// Role ID
        /// </summary>
        [StringLength(56)]
        public TKey Id { get; set; }

        /// <summary>
        /// Role name
        /// </summary>
        [StringLength(50)]
        [Index(Unique = true)]
        [Required]
        public string Name { get; set; }
    }
}

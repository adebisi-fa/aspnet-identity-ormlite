﻿using Microsoft.AspNet.Identity;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNet.Identity.OrmLite
{
    public class IdentityUser : IdentityUser<string>
    {
        public IdentityUser()
        {
            Id = Guid.NewGuid().ToString();
        }
        public IdentityUser(string name) : base(name)
        {
            Id = Guid.NewGuid().ToString();
        }
        public IdentityUser(string id, string name) : base(id, name)
        {

        }
    }
    public class IdentityUser<TKey> : IUser<TKey>
    {
        /// <summary>
        /// Default constructor 
        /// </summary>
        public IdentityUser()
        {
        }

        /// <summary>
        /// Constructor that takes user name as argument
        /// </summary>
        /// <param name="userName"></param>
        public IdentityUser(string userName)
            : this()
        {
            UserName = userName;
        }

        public IdentityUser(TKey id, string userName) : this(userName)
        {
            Id = id;
        }
        /// <summary>
        /// User ID
        /// </summary>
        [StringLength(56)]
        [Required]
        public TKey Id { get; set; }


        /// <summary>
        /// User's name
        /// </summary>
        [StringLength(50)]
        [Index(Unique =true)]
        [Required]
        public string UserName { get; set; }

        /// <summary>
        ///     Email
        /// </summary>
        [StringLength(100)]
        public virtual string Email { get; set; }

        /// <summary>
        ///     True if the email is confirmed, default is false
        /// </summary>
        public virtual bool EmailConfirmed { get; set; }

        /// <summary>
        ///     The salted/hashed form of the user password
        /// </summary>
        [StringLength(100)]
        public virtual string PasswordHash { get; set; }

        /// <summary>
        ///     A random value that should change whenever a users credentials have changed (password changed, login removed)
        /// </summary>
        [StringLength(100)]
        public virtual string SecurityStamp { get; set; }

        /// <summary>
        ///     PhoneNumber for the user
        /// </summary>
        [StringLength(40)]
        public virtual string PhoneNumber { get; set; }

        /// <summary>
        ///     True if the phone number is confirmed, default is false
        /// </summary>
        public virtual bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        ///     Is two factor enabled for the user
        /// </summary>
        public virtual bool TwoFactorEnabled { get; set; }

        /// <summary>
        ///     DateTime in UTC when lockout ends, any time in the past is considered not locked out.
        /// </summary>
        public virtual DateTime? LockoutEndDateUtc { get; set; }

        /// <summary>
        ///     Is lockout enabled for this user
        /// </summary>
        public virtual bool LockoutEnabled { get; set; }

        /// <summary>
        ///     Used to record failures for the purposes of lockout
        /// </summary>
        public virtual int AccessFailedCount { get; set; }

        /// <summary>
        ///     Used to record the date this entry was created.
        /// </summary>
        public DateTime DateCreated { get; set; }


        /// <summary>
        ///     Used to record the date this entry was last updated.
        /// </summary>
        public DateTime DateLastUpdated { get; set; }
    }
}

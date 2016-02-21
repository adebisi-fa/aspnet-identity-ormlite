using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Common;
using System.Collections;
using System.Security.Claims;
using ServiceStack.OrmLite;

namespace AspNet.Identity.OrmLite
{
    public class UserStore : IUserLoginStore<IdentityUser>,
        IUserClaimStore<IdentityUser>,
        IUserRoleStore<IdentityUser>,
        IUserPasswordStore<IdentityUser>,
        IUserSecurityStampStore<IdentityUser>,
        IQueryableUserStore<IdentityUser>,
        IUserEmailStore<IdentityUser>,
        IUserPhoneNumberStore<IdentityUser>,
        IUserTwoFactorStore<IdentityUser, string>,
        IUserLockoutStore<IdentityUser, string>,
        IUserStore<IdentityUser>
    {

        OrmLiteIdentityDbContext _context;

        public UserStore(OrmLiteIdentityDbContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            _context = context;
        }

        public virtual Task AddLoginAsync(IdentityUser user, UserLoginInfo login)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (login == null)
            {
                throw new ArgumentNullException("login");
            }
            var userLogin = new IdentityUserLogin<string>()
            {
                UserId = user.Id,
                ProviderKey = login.ProviderKey,
                LoginProvider = login.LoginProvider
            };
            using (var db = _context.OpenDbConnection())
                db.Insert(userLogin);

            return Task.FromResult(0);
        }

        public virtual Task<IdentityUser> FindAsync(UserLoginInfo login)
        {
            using (var db = _context.OpenDbConnection())
            {
                var userLogin = db.FirstOrDefault<IdentityUserLogin>(i => i.LoginProvider == login.LoginProvider && i.ProviderKey == login.ProviderKey);
                if (userLogin == null)
                    return null;

                return Task.FromResult(db.FirstOrDefault<IdentityUser>(u => u.Id == userLogin.UserId));
            }
        }

        public virtual Task<IList<UserLoginInfo>> GetLoginsAsync(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            using (var db = _context.OpenDbConnection())
            {
                var logins = db
                    .Where<IdentityUserLogin>(l => l.UserId == user.Id)
                    .ToArray()
                    .Select(l => new UserLoginInfo(l.LoginProvider, l.ProviderKey));

                IList<UserLoginInfo> result = logins.ToList();
                return Task.FromResult(result);
            }
        }

        public virtual Task RemoveLoginAsync(IdentityUser user, UserLoginInfo login)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (login == null)
            {
                throw new ArgumentNullException("login");
            }

            using (var db = _context.OpenDbConnection())
            {
                db.Delete<IdentityUserLogin>(l =>
                    l.UserId == user.Id &&
                    l.ProviderKey == login.ProviderKey &&
                    l.LoginProvider == login.LoginProvider
                );
                return Task.FromResult(0);
            }
        }

        public virtual Task CreateAsync(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            using (var db = _context.OpenDbConnection())
            {
                user.DateCreated = DateTime.UtcNow;
                db.Insert(user);
                return Task.FromResult(0);
            }
        }

        public virtual Task DeleteAsync(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            using (var db = _context.OpenDbConnection())
            {
                db.Delete<IdentityUser>(u => u.Id == user.Id);
                return Task.FromResult(0);
            }
        }

        public virtual Task<IdentityUser> FindByIdAsync(string userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException("userId");
            }

            using (var db = _context.OpenDbConnection())
                return Task.FromResult(db.FirstOrDefault<IdentityUser>(u => u.Id == userId));
        }

        public virtual Task<IdentityUser> FindByNameAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException("userName");
            }
            using (var db = _context.OpenDbConnection())
                return Task.FromResult(db.FirstOrDefault<IdentityUser>(u => u.UserName == userName));
        }

        public virtual Task UpdateAsync(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return UpdateUser(user);
        }

        private Task UpdateUser(IdentityUser user)
        {
            using (var db = _context.OpenDbConnection())
            {
                user.DateLastUpdated = DateTime.UtcNow;
                db.Update(user);
                return Task.FromResult(0);
            }
        }

        public virtual void Dispose()
        {

        }

        public virtual Task AddClaimAsync(IdentityUser user, System.Security.Claims.Claim claim)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (claim == null)
            {
                throw new ArgumentNullException("claim");
            }

            var userClaim = new IdentityUserClaim()
            {
                UserId = user.Id,
                ClaimType = claim.ValueType,
                ClaimValue = claim.Value
            };

            using (var db = _context.OpenDbConnection())
            {

                db.Insert(userClaim);
                UpdateUser(user);
                return Task.FromResult(0);
            }
        }

        public virtual Task<IList<System.Security.Claims.Claim>> GetClaimsAsync(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            using (var db = _context.OpenDbConnection())
            {
                var claims = db
                    .Where<IdentityUserClaim>(l => l.UserId == user.Id)
                    .ToArray()
                    .Select(c => new Claim(c.ClaimType, c.ClaimValue));

                IList<Claim> result = claims.ToList();
                return Task.FromResult(result);
            }
        }

        public virtual Task RemoveClaimAsync(IdentityUser user, System.Security.Claims.Claim claim)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (claim == null)
            {
                throw new ArgumentNullException("claim");
            }

            using (var db = _context.OpenDbConnection())
            {

                db.Delete<IdentityUserClaim>(c =>
                    c.UserId == user.Id &&
                    c.ClaimValue == claim.Value &&
                    c.ClaimType == claim.Type
                );

                UpdateUser(user);

                return Task.FromResult(0);
            }
        }

        public virtual Task AddToRoleAsync(IdentityUser user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentException("Argument cannot be null or empty: roleName.");
            }

            using (var db = _context.OpenDbConnection())
            {
                var role = db.FirstOrDefault<IdentityRole>(r => r.Name == roleName);
                if (role != null)
                {
                    var newUserRole = new IdentityUserRole
                    {
                        RoleId = role.Id,
                        UserId = user.Id
                    };
                    db.Insert(newUserRole);
                    UpdateUser(user);
                }
                return Task.FromResult(0);
            }
        }

        public virtual Task<IList<string>> GetRolesAsync(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            using (var db = _context.OpenDbConnection())
            {
                var roles = db
                    .Where<IdentityUserRole>(r => r.UserId == user.Id)
                    .Select(r => r.RoleId)
                    .ToArray();

                var roleNames = db
                    .Where<IdentityRole>(r => roles.Contains(r.Id))
                    .Select(r => r.Name)
                    .ToList();

                return Task.FromResult<IList<string>>(roleNames);
            }
        }

        public virtual Task<bool> IsInRoleAsync(IdentityUser user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException("roleName");
            }

            using (var db = _context.OpenDbConnection())
            {
                var role = db.FirstOrDefault<IdentityRole>(r => r.Name == roleName);

                bool isInRole = false;
                if (role != null)
                    isInRole = db.Count<IdentityUserRole>(r => r.RoleId == role.Id && r.UserId == user.Id) > 0;

                return Task.FromResult(isInRole);
            }
        }

        public virtual Task RemoveFromRoleAsync(IdentityUser user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException("roleName");
            }

            using (var db = _context.OpenDbConnection())
            {
                var role = db.FirstOrDefault<IdentityRole>(r => r.Name == roleName);
                if (role == null)
                    return Task.FromResult(0);

                db.Delete<IdentityUserRole>(r => r.UserId == user.Id && r.RoleId == role.Id);
                UpdateUser(user).Wait();
                return Task.FromResult(0);
            }
        }

        public virtual async Task<string> GetPasswordHashAsync(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return await Task.FromResult(user.PasswordHash);
        }

        public virtual async Task<bool> HasPasswordAsync(IdentityUser user)
        {
            var passwordHash = await GetPasswordHashAsync(user);
            return string.IsNullOrEmpty(passwordHash);
        }

        public virtual Task SetPasswordHashAsync(IdentityUser user, string passwordHash)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.PasswordHash = passwordHash;
            return UpdateUser(user);
        }

        public virtual async Task<string> GetSecurityStampAsync(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return await Task.FromResult(user.SecurityStamp);
        }

        public virtual Task SetSecurityStampAsync(IdentityUser user, string stamp)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.SecurityStamp = stamp;
            return UpdateUser(user);
        }

        public virtual IQueryable<IdentityUser> Users
        {
            get
            {
                using (var db = _context.OpenDbConnection())
                    return db.Where<IdentityUser>(u => u.UserName != " ").AsQueryable();
            }
        }

        public virtual Task<IdentityUser> FindByEmailAsync(string email)
        {
            using (var db = _context.OpenDbConnection())
                return Task.FromResult(db.FirstOrDefault<IdentityUser>(u => u.Email == email));
        }

        public virtual async Task<string> GetEmailAsync(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return await Task.FromResult(user.Email);
        }

        public virtual async Task<bool> GetEmailConfirmedAsync(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return await Task.FromResult(user.EmailConfirmed);
        }

        public virtual Task SetEmailAsync(IdentityUser user, string email)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.Email = email;
            return UpdateUser(user);
        }

        public virtual Task SetEmailConfirmedAsync(IdentityUser user, bool confirmed)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.EmailConfirmed = confirmed;
            return UpdateUser(user);
        }

        public virtual async Task<string> GetPhoneNumberAsync(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return await Task.FromResult(user.PhoneNumber);
        }

        public virtual async Task<bool> GetPhoneNumberConfirmedAsync(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return await Task.FromResult(user.PhoneNumberConfirmed);
        }

        public virtual Task SetPhoneNumberAsync(IdentityUser user, string phoneNumber)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.PhoneNumber = phoneNumber;
            return UpdateUser(user);
        }

        public virtual Task SetPhoneNumberConfirmedAsync(IdentityUser user, bool confirmed)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.PhoneNumberConfirmed = confirmed;
            return UpdateUser(user);
        }

        public virtual async Task<bool> GetTwoFactorEnabledAsync(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return await Task.FromResult(user.TwoFactorEnabled);
        }

        public virtual Task SetTwoFactorEnabledAsync(IdentityUser user, bool enabled)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.TwoFactorEnabled = enabled;
            return UpdateUser(user);
        }

        public virtual async Task<int> GetAccessFailedCountAsync(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return await Task.FromResult(user.AccessFailedCount);
        }

        public virtual async Task<bool> GetLockoutEnabledAsync(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return await Task.FromResult(user.LockoutEnabled);
        }

        public virtual async Task<DateTimeOffset> GetLockoutEndDateAsync(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return await Task.FromResult(user.LockoutEndDateUtc.HasValue
                    ? new DateTimeOffset(DateTime.SpecifyKind(user.LockoutEndDateUtc.Value, DateTimeKind.Utc))
                    : new DateTimeOffset());
        }

        public virtual Task<int> IncrementAccessFailedCountAsync(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.AccessFailedCount++;
            UpdateUser(user).Wait();
            return Task.FromResult(user.AccessFailedCount);
        }

        public virtual Task ResetAccessFailedCountAsync(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.AccessFailedCount = 0;
            return UpdateUser(user);
        }

        public virtual Task SetLockoutEnabledAsync(IdentityUser user, bool enabled)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.LockoutEnabled = enabled;
            return UpdateUser(user);
        }

        public virtual Task SetLockoutEndDateAsync(IdentityUser user, DateTimeOffset lockoutEnd)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.LockoutEndDateUtc = lockoutEnd.UtcDateTime;
            return UpdateUser(user);
        }
    }
}

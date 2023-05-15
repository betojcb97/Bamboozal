using Bamboo.Data;
using Bamboo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting.Internal;

namespace Bamboo.Util
{
    public class Util
    {
        public static CustomUser getLoggedUser(IHttpContextAccessor httpContextAccessor, BambooContext db)
        {
            if (httpContextAccessor.HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                string token = httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
                CustomUser dbUser = db.CustomUsers.Where(u => u.token.Equals(token)).FirstOrDefault();
                if (dbUser == null || dbUser.tokenExpirationDate < DateTime.Now || dbUser.tokenExpirationDate == null)
                {
                    return null;
                }
                return dbUser;
            }
            else
            {
                return null;
            }
        }
    }
}

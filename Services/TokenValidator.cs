using Bamboo.Data;
using Bamboo.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

public class TokenValidator
{
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly BambooContext db;

    public TokenValidator(IHttpContextAccessor _httpContextAccessor, BambooContext _db)
    {
        httpContextAccessor = _httpContextAccessor;
        db = _db;
    }

    public bool ValidateToken()
    {
        if (httpContextAccessor.HttpContext.Request.Headers.ContainsKey("Authorization"))
        {
            string token = httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
            CustomUser dbUser = db.CustomUsers.Where(u => u.token.Equals(token)).FirstOrDefault();
            if (dbUser == null || dbUser.tokenExpirationDate < DateTime.Now || dbUser.tokenExpirationDate == null)
            {
                return false;
            }
            return true;
        }
        else
        {
            return false;
        }
    }

}

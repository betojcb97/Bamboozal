using Microsoft.AspNetCore.Authorization;

namespace Bamboo.Authorization
{
    public class ActiveUser : IAuthorizationRequirement
    {
        public ActiveUser(string isActive)
        {
            isActive = isActive;
        }

        public string isActive { get; set; }
    }
}

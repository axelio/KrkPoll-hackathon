using System.Linq;
using Microsoft.AspNetCore.Http;

namespace KrkPoll.Services
{
    public interface IIdentityProvider
    {
        string GetUserIdFromClaims();
    }

    public class IdentityProvider : IIdentityProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public IdentityProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string GetUserIdFromClaims()
        {
            const string userIdClaimKey = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

            var claim = _contextAccessor?.HttpContext?.User.Claims.FirstOrDefault(c => string.Equals(c.Type, userIdClaimKey));
            return claim?.Value;
        }
    }
}

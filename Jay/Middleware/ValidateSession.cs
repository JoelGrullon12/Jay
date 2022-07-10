using Jay.Core.Application.Helpers;
using Jay.Core.Application.Interfaces.Services;
using Jay.Core.Application.ViewModels.User;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Jay.Presentation.WebApp.Middleware
{
    public class ValidateSession
    {
        private readonly IHttpContextAccessor _httpContext;

        public ValidateSession(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public bool HasUser()
        {
            UserViewModel user = _httpContext.HttpContext.Session.Get<UserViewModel>("user");

            if (user == null)
                return false;

            return true;
        }
    }
}

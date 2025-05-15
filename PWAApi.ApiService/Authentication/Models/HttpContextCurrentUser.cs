using System.Security.Claims;

namespace PWAApi.ApiService.Authentication.Models
{
    public interface ICurrentUser
    {
        Guid UserID { get; }
    }

    public class HttpContextCurrentUser : ICurrentUser
    {
        public HttpContextCurrentUser(IHttpContextAccessor http)
        {
            var guid = http.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            UserID = !string.IsNullOrEmpty(guid) ? Guid.Parse(guid) : Guid.Empty;
        }

        public Guid UserID { get; set; }
    }
}

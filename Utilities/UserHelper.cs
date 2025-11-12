using MiniEcom.Models;
using System.Security.Claims;
namespace MiniEcom.Utilities
{
    public static class UserHelper
    {
        public static int GetUserId(ClaimsPrincipal user)
        {
            var value = user.FindFirst("UserId")?.Value;
            return int.TryParse(value, out int id) ? id : -1;
        }
    }

}

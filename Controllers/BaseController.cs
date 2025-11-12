using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MiniEcom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected int? CurrentUserId => GetUserId();

        private int? GetUserId()
        {
            var claimValue = User.FindFirst("UserId")?.Value;
            if (int.TryParse(claimValue, out int id))
                return id;

            return null;
        }
    }
}

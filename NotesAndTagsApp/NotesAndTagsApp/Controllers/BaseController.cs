using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NotesAndTagsApp.Controllers
{
    [Authorize]
    public class BaseController : ControllerBase
    {
    }
}

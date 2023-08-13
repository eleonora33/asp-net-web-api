using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UserAppG5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<string>> Get()
        {
            return StatusCode(StatusCodes.Status200OK, StaticDb.UsersNames);

        }

        [HttpGet("{index}")]
        public ActionResult<string> GetByIndex(int index)
        {
            try
            {
                if(index < 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "The index has negative value!");

                }

                if(index >= StaticDb.UsersNames.Count)
                {
                    return StatusCode(StatusCodes.Status404NotFound, $"There is no resource on index {index}");
                }

                return StatusCode(StatusCodes.Status200OK, StaticDb.UsersNames[index]);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred. Contact the administrator.");
            }     
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Waseet.System.Services.Persistence.Data;
using Waseet.System.Services.Persistence.Errors;

namespace Waseet.System.Services.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController : ControllerBase
    {
        private readonly WaseetContext _context;

        public BuggyController(WaseetContext context)
        {
            _context = context;
        }

        [HttpGet("NotFound")]
        public ActionResult GetNotFoundRequest()
        {
            var product = _context.products.Find(1000);

            if (product == null) return NotFound(new ApiResponse(404));

            return Ok(product);
        }


        [HttpGet("servererror")]
        public ActionResult GetServererror()
        {
            var product = _context.products.Find(1000);

            var production = product.ToString();

            return Ok(production);
        }

        [HttpGet("badrequest")]
        public ActionResult Getbadrequest()
        {
            return BadRequest(new ApiResponse(400));
        }

        [HttpGet("badrequest/{id}")]
        public ActionResult Getbadrequest(int id)
        {
            return Ok();
        }

    }
}

using BOs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace OldCarShowroomNetworkRazorPages.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowroomController : ControllerBase
    {
        private readonly BOs.Models.OldCarShowroomNetworkContext _context;
        public ShowroomController()
        {
            _context = new OldCarShowroomNetworkContext();
        }
        [HttpGet]
        [Route("totalShowrooms")]
        public ActionResult<int> GetTotalShowrooms()
        {
            int totalShowrooms = _context.Showrooms.Count();
            return Ok(totalShowrooms);
        }

    }
}

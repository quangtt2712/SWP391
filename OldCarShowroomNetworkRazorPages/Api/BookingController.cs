using BOs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace OldCarShowroomNetworkRazorPages.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly BOs.Models.OldCarShowroomNetworkContext _context;
        public BookingController()
        {
            _context = new OldCarShowroomNetworkContext();
        }
        [HttpGet]
        [Route("booking")]
        public async Task<ActionResult> CheckBooking(int carId, int isBooked, DateTime dateTime)
        {
            var query = await _context.Bookings.FirstOrDefaultAsync(b => b.DayBooking == dateTime && b.Slot == isBooked && b.Notification.Equals(1) && b.CarId == carId);

            if (query != null)
            {
                return Ok();
            }
            return NotFound();
            
        }

    }
}

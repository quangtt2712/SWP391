using BOs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OldCarShowroomNetworkRazorPages.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly BOs.Models.OldCarShowroomNetworkContext _context;

        public CarController()
        {
            _context = new OldCarShowroomNetworkContext();
        }
        [HttpGet]
        [Route("carName/{carId}")]
        public ActionResult<List<CarName>> listCarNames(int carId) {


            return _context.CarNames.Where(c => c.ManufactoryId == carId).ToList();

        }
       /* public ActionResult<Pet> Create(Pet pet)
        {
            pet.Id = _petsInMemoryStore.Any() ?
                     _petsInMemoryStore.Max(p => p.Id) + 1 : 1;
            _petsInMemoryStore.Add(pet);

            return CreatedAtAction(nameof(GetById), new { id = pet.Id }, pet);
        }*/

    }
}

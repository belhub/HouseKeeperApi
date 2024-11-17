using HouseKeeperApi.Models;
using HouseKeeperApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseKeeperApi.Controllers
{
    //działa
    [Route("api/[controller]")]
    [ApiController] // walidacja
    [Authorize(Roles = "Tenant, Landlord")] //autoryzacja
    public class HouseController : ControllerBase
    {
        //działa wszystko
        private readonly IHouseService _houseService;

        public HouseController(IHouseService houseService)
        {
            _houseService = houseService;
        }

        [HttpGet("byHouseId/{id}")]
        public async Task<ActionResult<HouseDto>> GetHouseById([FromRoute] int id)
        {
            var house = await _houseService.GetHouseById(id);
            return house == null ? NotFound() : Ok(house);
        }

        [HttpGet("byUserId/{userId}")]
        public async Task<ActionResult<List<HouseDto>>> GetHousesByUserId([FromRoute] int userId)
        {
            var houses = await _houseService.GetHousesByUserId(userId); //brakuje houseTenants
            return houses == null ? NotFound() : Ok(houses);
        }

        [HttpPost]
        [Authorize(Roles = "Landlord")]
        public async Task<ActionResult> CreateHouse([FromBody] HouseDto houseDto)
        {
            var houseId = await _houseService.CreateHouse(houseDto);
            return Created($"/api/house/{houseId}", null);
        }

        [HttpPut("{houseId}")]
        [Authorize(Roles = "Landlord")]
        public async Task<ActionResult> UpdateHouse([FromRoute] int houseId, [FromBody] HouseDto houseDto)
        {
            var house = await _houseService.UpdateHouse(houseId, houseDto);
            return house ? Ok() : NotFound();
        }

        [HttpDelete("{houseId}")]
        [Authorize(Roles = "Landlord")]
        public async Task<ActionResult> DeleteHouseById([FromRoute] int houseId)
        {
            var isDeleted = await _houseService.DeleteHouseById(houseId);
            return isDeleted ? NoContent() : NotFound();
        }
    }
}

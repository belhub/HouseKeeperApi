using HouseKeeperApi.Models;
using HouseKeeperApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseKeeperApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Tenat, Landlord")]
    public class EquipmentController : Controller
    {
        private readonly IEqiupmentService _eqiupmentService;

        public EquipmentController(IEqiupmentService eqiupmentService)
        {
            _eqiupmentService = eqiupmentService;
        }

        [HttpGet("byHouseId/{houseId}")]
        public async Task<ActionResult<List<EquipmentDto>>> GetEquipmentsByHouseId([FromRoute] int houseId)
        {
            var equipments = await _eqiupmentService.GetAllEquipmentByHouseId(houseId);
            return equipments == null ? NotFound() : Ok(equipments);
        }

        [HttpGet("byUserId/{userId}")]
        public async Task<ActionResult<List<EquipmentDto>>> GetEquipmentsByUserId([FromRoute] int userId)
        {
            var equipments = await _eqiupmentService.GetAllEquipmentByUserId(userId);
            return equipments == null ? NotFound() : Ok(equipments);
        }

        [HttpPost]
        public async Task<ActionResult> CreateEquipment([FromBody] EquipmentDto equipmentDto)
        {
            var equipmentId = await _eqiupmentService.CreateEquipment(equipmentDto);
            return Created($"Pomyslnie utworzono obiekt o id: {equipmentId}", null);
        }

        [HttpPut("{equipmentId}")]
        public async Task<ActionResult> UpdateEquipment([FromRoute] int equipmentId, [FromBody] EquipmentDto equipmentDto)
        {
            var house = await _eqiupmentService.UpdateEquipment(equipmentId, equipmentDto);
            return house ? Ok() : NotFound();
        }

        [HttpDelete("{equipmentId}")]
        public async Task<ActionResult> DeleteEquipmentById([FromRoute] int equipmentId)
        {
            var isDeleted = await _eqiupmentService.DeleteEquipment(equipmentId);
            return isDeleted ? NoContent() : NotFound();
        }
    }
}

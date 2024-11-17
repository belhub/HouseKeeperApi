using HouseKeeperApi.Models;
using HouseKeeperApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseKeeperApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController] // walidacja
    [Authorize(Roles = "Tenant, Landlord")] //autoryzacja
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet("byHouseId/{houseId}")]
        [Authorize(Roles = "Landlord")]
        public async Task<ActionResult<List<RoomDto>>> GetRoomsByHouseId([FromRoute] int houseId)
        {
            var rooms = await _roomService.GetRoomsByHouseId(houseId);
            return rooms == null ? NotFound() : Ok(rooms);
        }

        [HttpGet("forTenantId/{tenantId}")]
        [Authorize(Roles = "Tenant")]
        public async Task<ActionResult<List<RoomDto>>> GetRoomsForTenantId([FromRoute] int tenantId)
        {
            var rooms = await _roomService.GetRoomsByOwnerId(tenantId);
            return rooms == null ? NotFound() : Ok(rooms);
        }

        [HttpPost]
        [Authorize(Roles = "Landlord")]
        public async Task<ActionResult> CreateRoom([FromBody] RoomDto roomDto)
        {
            var room = await _roomService.CreateRoom(roomDto);

            return Created($"Pomyslnie utworzono pokoj o id: {room}", null);
        }

        [HttpPut("{roomId}")]
        public async Task<ActionResult> UpdateRoom([FromRoute] int roomId, [FromBody] RoomDto roomDto)
        {
            var room = await _roomService.UpdateRoom(roomId, roomDto);
            return room ? Ok() : NotFound();
        }

        [HttpDelete("{roomId}")]
        public async Task<ActionResult> DeleteRoom([FromRoute] int roomId)
        {
            var room = await _roomService.DeleteRoom(roomId);
            return room ? NoContent() : NotFound();
        }

    }
}

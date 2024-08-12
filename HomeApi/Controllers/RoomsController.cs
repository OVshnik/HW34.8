using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HomeApi.Contracts.Models.Rooms;
using HomeApi.Data.Models;
using HomeApi.Data.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace HomeApi.Controllers
{
    /// <summary>
    /// Контроллер комнат
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class RoomsController : ControllerBase
    {
        private IRoomRepository _repository;
        private IMapper _mapper;
        private IDeviceRepository _deviceRepository;

        public RoomsController(IRoomRepository repository, IMapper mapper, IDeviceRepository deviceRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _deviceRepository = deviceRepository;
        }

        //TODO: Задание - добавить метод на получение всех существующих комнат

        /// <summary>
        /// Добавление комнаты
        /// </summary>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Add([FromBody] AddRoomRequest request)
        {
            var existingRoom = await _repository.GetRoomByName(request.Name);
            if (existingRoom == null)
            {
                var newRoom = _mapper.Map<AddRoomRequest, Room>(request);
                await _repository.AddRoom(newRoom);
                return StatusCode(201, $"Комната {request.Name} добавлена!");
            }

            return StatusCode(409, $"Ошибка: Комната {request.Name} уже существует.");
        }
        [HttpDelete]
        [Route ("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var room = await _repository.GetRoomById(id);
            if (room == null)
                return StatusCode(400, $"Ошибка: Комната с идентификатором {id} не существует.");

            var devices = await _deviceRepository.GetDevices();
            if (devices != null)
            {
                foreach (var device in devices)
                {
                    if (device.RoomId == id)
                    {
                        await _deviceRepository.DeleteDevice(device);
                        return StatusCode(200, $"Устройство {device.Name} в комнате {room.Name} удалено");
                    }
                }
                await _repository.DeleteRoom(room);
                return StatusCode(200, $"Комната {room.Name} удалена");
            }
            await _repository.DeleteRoom(room);
            return StatusCode(200, $"Комната {room.Name} удалена");
        }
        [HttpPut]
        [Route("{id}")]
        public async Task <IActionResult> Update([FromRoute] Guid id, [FromBody] Room updRoom)
        {
            var room = await _repository.GetRoomById(id);
            if (room == null)
                return StatusCode(400, $"Ошибка: Комната с идентификатором {id} не существует.");
            room.Name = updRoom.Name;
            room.Name = updRoom.Name;
            room.Area = updRoom.Area;
            room.GasConnected = updRoom.GasConnected;
            room.Voltage = updRoom.Voltage;
            
            await _repository.UpdateRoom(room);
            return StatusCode(200, $"Данные комнаты {room.Name} обновлены");

        }
    }
}
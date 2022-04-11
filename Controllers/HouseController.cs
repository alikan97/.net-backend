using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Server.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Server.Repositories;
using Server.Entities;

namespace Server.Controllers
{
    [Route("api/house")]
    [ApiController]
    public class HouseContoller : ControllerBase
    {
        private readonly HouseRepository _houseService;
        public HouseContoller(HouseRepository service)
        {
            _houseService = service;
        }

        [HttpPost]
        [Route("new")]
        public async Task<ActionResult> CreateHouse([FromBody] createHouseDto house)
        {
            var Occupantsss = new List<string> {};
            foreach(var elem in house.Occupants)
            {
                Occupantsss.Add(elem);
            }

            var newhouse = new House {
                Temperature = house.Temperature,
                Humidity = house.Humidity,
                alarmTriggers = null,
                weatherDescription = null,
                Occupants = Occupantsss,
                Name = house.Name,
                Id = new Guid()
            };

            await _houseService.CreateHouse(newhouse);
            return Ok();
        }
    }
}

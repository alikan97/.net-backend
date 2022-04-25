using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Entities;

namespace Server.Repositories
{
    public interface IHouseRepository
    {
        Task CreateHouse(House newhouse);
        Task updateTemperature(decimal newTemp);
        Task updateHumidity(decimal newHumidity);
        Task createAlarmTrigger(string alarmName);
        Task updateAlarm(Dictionary<string,string> alarmState);
        Task deleteAlarm(string alarmName);
        Task updateWeatherDescription(string newDescription);
        Task addOccupant(string address, string[] occupant);
    }
}
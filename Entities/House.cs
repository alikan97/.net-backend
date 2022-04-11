using System;
using System.Collections.Generic;

namespace Server.Entities
{
    public record House
    {
        // Use Record types for immutable objects
        public string Name {get; set;}
        public decimal Temperature {get; set; }
        public decimal Humidity {get; set; }
        public Dictionary<string,string> alarmTriggers {get; set; }
        public string weatherDescription {get;set;}
        public List<string> Occupants {get; set;}
        public Guid Id{get; init;}
        //Address, finance, ....
    }
}
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace IoTDemo.Models
{
    public class EventLog
    {
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("__createdAt")]
        public DateTime CreatedAt { get; set; }
    }
}

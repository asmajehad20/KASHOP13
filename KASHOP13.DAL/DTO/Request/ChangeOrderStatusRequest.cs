using KASHOP13.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace KASHOP13.DAL.DTO.Request
{
    public class ChangeOrderStatusRequest
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrderStatusEnum Status { get; set; }
    }
}

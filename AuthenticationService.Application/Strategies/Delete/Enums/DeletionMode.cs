using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Strategies.Delete.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DeletionMode
    {
        Hard = 0,
        Soft = 1
    }
}

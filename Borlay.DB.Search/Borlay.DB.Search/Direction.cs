using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Borlay.DB.Search
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Direction
    {
        Asc,
        Desc
    }
}

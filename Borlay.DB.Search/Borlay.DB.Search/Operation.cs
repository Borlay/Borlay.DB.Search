using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Borlay.DB.Search
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Operation
    {
        Equals,
        Greater,
        Less,
        GreaterOrEqual,
        LessOrEqual,
        Contains,
        StartsWith,
        EndsWith
    }
}

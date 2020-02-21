using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Runtime.CompilerServices;

namespace Borlay.DB.Search
{
    public class OrderBy
    {
        public string OrderColumn { get; set; }
        public Direction OrderDirection { get; set; }
    }
}

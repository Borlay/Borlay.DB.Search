using System;

namespace Borlay.DB.Search
{
    public class DataResult<T>
    {
        public T[] Data { get; set; }

        public int Skip { get; set; }

        public int? Take { get; set; }

        public int Total { get; set; }
    }
}

using System.Collections.Generic;

namespace MemoryServer
{
    public class Config
    {
        public List<double> TimeIntervals { get; set; } = new List<double>();
        public string Database { get; set; }
    }
}
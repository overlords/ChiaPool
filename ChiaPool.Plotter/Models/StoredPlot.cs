using System;

namespace ChiaPool.Models
{
    public class StoredPlot
    {
        public long Id { get; set; }
        public string Secret { get; set; }
        public string Path { get; set; }
        public bool Available { get; set; }
        public bool Deleted { get; set; }

        public StoredPlot()
        {
        }
        public StoredPlot(string path)
        {
            Secret = Guid.NewGuid().ToString();
            Path = path;
            Available = true;
            Deleted = false;
        }
    }
}

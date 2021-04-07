namespace ChiaPool.Models
{
    public class PlottingConfiguration
    {
        public int Size { get; set; } = 32;
        public string Path { get; set; }
        public string CachePath { get; set; }
        public int BucketCount { get; set; }
        public int BufferSize { get; set; }

        public PlottingConfiguration()
        {
        }
        public PlottingConfiguration(int size, string path, string cachePath, int bucketCount, int bufferSize)
        {
            Size = size;
            Path = path;
            CachePath = cachePath;
            BucketCount = bucketCount;
            BufferSize = bufferSize;
        }
    }
}

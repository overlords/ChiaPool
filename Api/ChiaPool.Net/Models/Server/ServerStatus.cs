namespace ChiaPool.Models
{
    public sealed class ServerStatus
    {
        public long SyncHeight { get; set; }
        public long MaxSyncHeight { get; set; }

        public ServerStatus(long syncHeight, long maxSyncHeight)
        {
            SyncHeight = syncHeight;
            MaxSyncHeight = maxSyncHeight;
        }
    }
}

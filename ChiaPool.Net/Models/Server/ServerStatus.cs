namespace ChiaPool.Models
{
    public sealed class ServerStatus
    {
        public long SyncHeight { get; init; }
        public long MaxSyncHeight { get; init; }

        public ServerStatus(long syncHeight, long maxSyncHeight)
        {
            SyncHeight = syncHeight;
            MaxSyncHeight = maxSyncHeight;
        }
    }
}

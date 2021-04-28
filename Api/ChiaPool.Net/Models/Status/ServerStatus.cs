namespace ChiaPool.Models
{
    public sealed class ServerStatus : IStatus
    {
        public bool Synced { get; init; }
        public long SyncHeight { get; init; }
        public long MaxSyncHeight { get; init; }


        public ServerStatus()
        {
        }
        public ServerStatus(bool synced, long syncHeight, long maxSyncHeight)
        {
            Synced = synced;
            SyncHeight = syncHeight;
            MaxSyncHeight = maxSyncHeight;
        }
    }
}

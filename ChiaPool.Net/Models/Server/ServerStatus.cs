namespace ChiaPool.Models
{
    public sealed class ServerStatus
    {
        public long SyncHeight { get; init; }

        public ServerStatus(long syncHeight)
        {
            SyncHeight = syncHeight;
        }
    }
}

namespace ChiaPool.Models
{
    public sealed class DisconnectRequest
    {
        public string Reason { get; init; }

        public DisconnectRequest()
        {
        }
        public DisconnectRequest(string reason)
        {
            Reason = reason;
        }
    }
}

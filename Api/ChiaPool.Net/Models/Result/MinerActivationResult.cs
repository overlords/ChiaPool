namespace ChiaPool.Models
{
    public sealed class MinerActivationResult
    {
        public bool Successful { get; init; }
        public long UserId { get; init; }
        public string Reason { get; init; }

        public MinerActivationResult()
        {
        }

        public static MinerActivationResult FromSuccess(long userId)
            => new MinerActivationResult()
            {
                Successful = true,
                UserId = userId,
                Reason = default,
            };
        public static MinerActivationResult FromFailed(string reason)
            => new MinerActivationResult()
            {
                Successful = false,
                UserId = default,
                Reason = reason,
            };
    }
}

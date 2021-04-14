namespace ChiaPool.Models
{
    public sealed class ActivationResult
    {
        public bool Successful { get; init; }
        public long UserId { get; init; }
        public string Reason { get; init; }

        public ActivationResult()
        {
        }

        public static ActivationResult FromSuccess(long userId)
            => new ActivationResult()
            {
                Successful = true,
                UserId = userId,
                Reason = default,
            };
        public static ActivationResult FromFailed(string reason)
            => new ActivationResult()
            {
                Successful = false,
                UserId = default,
                Reason = reason,
            };
    }
}

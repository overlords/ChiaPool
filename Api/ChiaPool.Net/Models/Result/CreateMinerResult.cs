namespace ChiaPool.Models
{
    public sealed class CreateMinerResult
    {

        public string Token { get; init; }
        public MinerInfo Info { get; init; }

        public CreateMinerResult(string token, MinerInfo info)
        {
            Token = token;
            Info = info;
        }
    }
}

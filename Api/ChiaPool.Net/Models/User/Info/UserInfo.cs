namespace ChiaPool.Models
{
    public sealed class UserInfo
    {
        public long Id { get; init; }
        public string Name { get; init; }

        public long PlotMinutes { get; init; }


        public UserInfo()
        {
        }
        public UserInfo(long id, string name, long plotMinutes)
        {
            Id = id;
            Name = name;
            PlotMinutes = plotMinutes;
        }
    }
}

namespace ChiaPool.Models
{
    public interface INode
    {
        public long Id { get; }
        public string Name { get; }

        public long OwnerId { get; }
    }
}

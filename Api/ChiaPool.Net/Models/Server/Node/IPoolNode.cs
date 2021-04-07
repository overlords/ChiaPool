namespace ChiaPool.Models
{
    public interface IPoolNode
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public long OwnerId { get; set; }
    }
}

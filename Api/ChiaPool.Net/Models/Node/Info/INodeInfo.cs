namespace ChiaPool.Models
{
    public interface INodeInfo : INode
    {
        public bool Online { get; }
    }
}

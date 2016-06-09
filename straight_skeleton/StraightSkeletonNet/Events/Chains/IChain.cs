using StraightSkeletonNet.Circular;

namespace StraightSkeletonNet.Events.Chains
{
    internal interface IChain
    {
        Edge PreviousEdge { get; }
        Edge NextEdge { get; }
        Vertex PreviousVertex { get; }
        Vertex NextVertex { get; }
        Vertex CurrentVertex { get; }
        ChainType ChainType { get; }
    }
}
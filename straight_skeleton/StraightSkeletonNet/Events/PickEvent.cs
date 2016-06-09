using StraightSkeletonNet.Events.Chains;
using StraightSkeletonNet.Primitives;

namespace StraightSkeletonNet.Events
{
    internal class PickEvent : SkeletonEvent
    {
        public readonly EdgeChain Chain;

        public override bool IsObsolete { get { return false; } }

        public PickEvent(Vector2d point, double distance, EdgeChain chain) : base(point, distance)
        {
            Chain = chain;
        }
    }
}
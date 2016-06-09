using System.Collections.Generic;
using StraightSkeletonNet.Events.Chains;
using StraightSkeletonNet.Primitives;

namespace StraightSkeletonNet.Events
{
    internal class MultiSplitEvent : SkeletonEvent
    {
        public readonly List<IChain> Chains;

        public override bool IsObsolete { get { return false; } }

        public MultiSplitEvent(Vector2d point, double distance, List<IChain> chains)
            : base(point, distance)
        {
            Chains = chains;
        }
    }
}
using System;
using StraightSkeletonNet.Circular;
using StraightSkeletonNet.Primitives;

namespace StraightSkeletonNet.Events
{
    internal class SplitEvent : SkeletonEvent
    {
        public readonly Edge OppositeEdge;
        public readonly Vertex Parent;

        public SplitEvent(Vector2d point, double distance, Vertex parent, Edge oppositeEdge)
            : base(point, distance)
        {
            Parent = parent;
            OppositeEdge = oppositeEdge;
        }

        public override bool IsObsolete { get { return Parent.IsProcessed; } }


        public override String ToString()
        {
            return "SplitEvent [V=" + V + ", Parent=" + (Parent != null ? Parent.Point.ToString() : "null") +
                   ", Distance=" + Distance + "]";
        }
    }
}
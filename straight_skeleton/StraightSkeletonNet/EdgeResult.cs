using System.Collections.Generic;
using StraightSkeletonNet.Circular;
using StraightSkeletonNet.Primitives;

namespace StraightSkeletonNet
{
    public class EdgeResult
    {
        public readonly Edge Edge;
        public readonly List<Vector2d> Polygon;

        public EdgeResult(Edge edge, List<Vector2d> polygon)
        {
            Edge = edge;
            Polygon = polygon;
        }
    }
}
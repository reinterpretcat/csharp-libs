using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StraightSkeletonNet.Primitives;

namespace StraightSkeletonNet.Tests
{
    internal class SkeletonTestUtil
    {
        public static List<Vector2d> GetFacePoints(Skeleton sk)
        {
            List<Vector2d> ret = new List<Vector2d>();

            foreach (EdgeResult edgeOutput in sk.Edges)
            {
                List<Vector2d> points = edgeOutput.Polygon;
                foreach (Vector2d vector2d in points)
                {
                    if (!ContainsEpsilon(ret, vector2d))
                        ret.Add(vector2d);
                }
            }
            return ret;
        }

        public static void AssertExpectedPoints(List<Vector2d> expectedList, List<Vector2d> givenList)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Vector2d expected in expectedList)
            {
                if (!ContainsEpsilon(givenList, expected))
                    sb.AppendFormat("Can't find expected point ({0}, {1}) in given list\n", expected.X, expected.Y);
            }

            foreach (Vector2d given in givenList)
            {
                if (!ContainsEpsilon(expectedList, given))
                    sb.AppendFormat("Can't find given point ({0}, {1}) in expected list\n", given.X, given.Y);
            }

            if (sb.Length > 0)
                throw new InvalidOperationException(sb.ToString());
        }

        public static bool ContainsEpsilon(List<Vector2d> list, Vector2d p)
        {
            return list.Any(l => EqualEpsilon(l.X, p.X) && EqualEpsilon(l.Y, p.Y));
        }

        public static bool EqualEpsilon(double d1, double d2)
        {
            return Math.Abs(d1 - d2) < 5E-6;
        }
    }
}
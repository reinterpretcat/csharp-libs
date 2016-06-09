using System.Collections.Generic;
using NUnit.Framework;
using StraightSkeletonNet.Primitives;

namespace StraightSkeletonNet.Tests
{
    [TestFixture]
    public class SkeletonLevelEventsTest
    {
        private void AssertPolygonWithEdges(int numOfEdges, Skeleton sk)
        {
            foreach (var edgeOutput in sk.Edges)
            {
                var points = edgeOutput.Polygon;
                if (points.Count == numOfEdges)
                    return;
            }
            Assert.Fail("Expected polygon with number of edges: " + numOfEdges);
        }

        [Test]
        public void Skeleton_multiEdgeEvent()
        {
            var outer = new List<Vector2d>
            {
                new Vector2d(0, 1),
                new Vector2d(-1, 0),
                new Vector2d(0, -1),
                new Vector2d(5, -2),
                new Vector2d(7, 0),
                new Vector2d(5, 2)
            };

            var expected = new List<Vector2d>
            {
                new Vector2d(0.53518, 0),
                new Vector2d(4.39444872, 0)
            };
            expected.AddRange(outer);

            var sk = SkeletonBuilder.Build(outer, null);


            SkeletonTestUtil.AssertExpectedPoints(expected, SkeletonTestUtil.GetFacePoints(sk));
        }

        [Test]
        public void Skeleton_pickEvent()
        {
            var outer = new List<Vector2d>
            {
                new Vector2d(-1, -1),
                new Vector2d(1, -1),
                new Vector2d(1, 1),
                new Vector2d(-1, 1)
            };
            var expected = new List<Vector2d>
            {
                new Vector2d(0, 0)
            };
            expected.AddRange(outer);


            var sk = SkeletonBuilder.Build(outer, null);

            SkeletonTestUtil.AssertExpectedPoints(expected, SkeletonTestUtil.GetFacePoints(sk));
        }

        [Test]
        public void SkeletonTest_cross_T1()
        {
            var outer = new List<Vector2d>
            {
                new Vector2d(-3, -1),
                new Vector2d(3, -1),
                new Vector2d(3, 1),
                new Vector2d(1, 1),
                new Vector2d(1, 3),
                new Vector2d(-1, 3),
                new Vector2d(-1, 1),
                new Vector2d(-3, 1)
            };

            var expected = new List<Vector2d>();

            expected.AddRange(outer);
            expected.Add(new Vector2d(-2, 0));
            expected.Add(new Vector2d(2, 0));
            expected.Add(new Vector2d(0, 0));
            expected.Add(new Vector2d(0, 2));

            var sk = SkeletonBuilder.Build(outer, null);

            SkeletonTestUtil.AssertExpectedPoints(expected, SkeletonTestUtil.GetFacePoints(sk));
        }

        [Test]
        public void SkeletonTest_cross_X1()
        {
            var outer = new List<Vector2d>
            {
                new Vector2d(-3, -1),
                new Vector2d(-1, -1),
                new Vector2d(-1, -3),
                new Vector2d(1, -3),
                new Vector2d(1, -1),
                new Vector2d(3, -1),
                new Vector2d(3, 1),
                new Vector2d(1, 1),
                new Vector2d(1, 3),
                new Vector2d(-1, 3),
                new Vector2d(-1, 1),
                new Vector2d(-3, 1)
            };


            var expected = new List<Vector2d>(outer)
            {
                new Vector2d(0, 0),
                new Vector2d(0, 2),
                new Vector2d(0, -2),
                new Vector2d(2, 0),
                new Vector2d(-2, 0)
            };

            var sk = SkeletonBuilder.Build(outer, null);

            SkeletonTestUtil.AssertExpectedPoints(expected, SkeletonTestUtil.GetFacePoints(sk));
        }

        [Test]
        public void SkeletonTest_double_split()
        {
            var outer = new List<Vector2d>
            {
                new Vector2d(-6, 0),
                new Vector2d(-3, -6),
                new Vector2d(-1, -2),
                new Vector2d(1, -2),
                new Vector2d(3, -6),
                new Vector2d(6, 0)
            };

            var expected = new List<Vector2d>(outer)
            {
                new Vector2d(-3.0000000000000004, -1.854101966249685),
                new Vector2d(-1.6180339887498951, -1.0000000000000002),
                new Vector2d(1.6180339887498951, -1.0000000000000002),
                new Vector2d(3.0000000000000004, -1.854101966249685)
            };

            var sk = SkeletonBuilder.Build(outer, null);

            SkeletonTestUtil.AssertExpectedPoints(expected, SkeletonTestUtil.GetFacePoints(sk));
        }

        [Test]
        public void SkeletonTest_double_split2()
        {
            var outer = new List<Vector2d>
            {
                new Vector2d(-6, 0),
                new Vector2d(-3, -6),
                new Vector2d(-1, -2),
                new Vector2d(0, -3),
                new Vector2d(1, -2),
                new Vector2d(3, -6),
                new Vector2d(6, 0)
            };

            var expected = new List<Vector2d>(outer)
            {
                new Vector2d(-3.0000000000000004, -1.854101966249685),
                new Vector2d(-1.2038204263767998, -0.7440019398522527),
                new Vector2d(-0.0, -1.242640687119285),
                new Vector2d(1.2038204263767998, -0.7440019398522527),
                new Vector2d(3.0000000000000004, -1.854101966249685)
            };

            var sk = SkeletonBuilder.Build(outer, null);

            SkeletonTestUtil.AssertExpectedPoints(expected, SkeletonTestUtil.GetFacePoints(sk));

            AssertPolygonWithEdges(7, sk);
        }

        [Test]
        public void SkeletonTest_multiple()
        {
            var outer = new List<Vector2d> {new Vector2d(0, 0), new Vector2d(5, 0), new Vector2d(5, 5), new Vector2d(0, 5)};

            var h1 = new List<Vector2d> { new Vector2d(1, 1), new Vector2d(2, 1), new Vector2d(2, 2), new Vector2d(1, 2)};

            var h2 = new List<Vector2d> { new Vector2d(3, 3), new Vector2d(4, 3), new Vector2d(4, 4), new Vector2d(3, 4)};

            var h3 = new List<Vector2d> {new Vector2d(1, 3), new Vector2d(2, 3), new Vector2d(2, 4), new Vector2d(1, 4)};

            var h4 = new List<Vector2d> {new Vector2d(3, 1), new Vector2d(4, 1), new Vector2d(4, 2), new Vector2d(3, 2)};

            var expected = new List<Vector2d>(outer)
            {
                new Vector2d(4.5, 2.5),
                new Vector2d(4.5, 0.5),
                new Vector2d(4.5, 4.5),
                new Vector2d(0.5, 4.5),
                new Vector2d(2.5, 4.5),
                new Vector2d(0.5, 0.5),
                new Vector2d(0.5, 2.5),
                new Vector2d(2.5, 0.5),
                new Vector2d(2.5, 2.5),
                new Vector2d(2.0, 2.0),
                new Vector2d(2.0, 1.0),
                new Vector2d(1.0, 1.0),
                new Vector2d(1.0, 2.0),
                new Vector2d(4.0, 4.0),
                new Vector2d(4.0, 3.0),
                new Vector2d(3.0, 3.0),
                new Vector2d(3.0, 4.0),
                new Vector2d(2.0, 4.0),
                new Vector2d(2.0, 3.0),
                new Vector2d(1.0, 3.0),
                new Vector2d(1.0, 4.0),
                new Vector2d(4.0, 2.0),
                new Vector2d(4.0, 1.0),
                new Vector2d(3.0, 1.0),
                new Vector2d(3.0, 2.0)
            };

            var sk = SkeletonBuilder.Build(outer, new List<List<Vector2d>> {h1, h2, h3, h4});

            SkeletonTestUtil.AssertExpectedPoints(expected, SkeletonTestUtil.GetFacePoints(sk));
        }
    }
}
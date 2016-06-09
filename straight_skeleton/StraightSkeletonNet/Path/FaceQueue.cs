using System;
using StraightSkeletonNet.Circular;

namespace StraightSkeletonNet.Path
{
    internal class FaceQueue : PathQueue<FaceNode>
    {
        /// <summary> Edge for given queue. </summary>
        public Edge Edge;

        /// <summary> Flag if queue is closed. After closing can't be modify. </summary>
        public bool Closed { get; private set; }

        /// <summary> Flag if queue is connected to edges. </summary>
        public bool IsUnconnected
        {
            get { return Edge == null; }
        }

        public override void AddPush(PathQueueNode<FaceNode> node, PathQueueNode<FaceNode> newNode)
        {
            if (Closed)
                throw new InvalidOperationException("Can't add node to closed FaceQueue");

            base.AddPush(node, newNode);
        }

        /// <summary> Mark queue as closed. After closing can't be modify. </summary>
        public void Close()
        {
            Closed = true;
        }
    }
}
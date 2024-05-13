using System.Numerics;

namespace Logic;

public class AabbTree
{
    private AabbTreeNode? _root;
    private ReaderWriterLock _lock = new ReaderWriterLock();

    public ReaderWriterLock Lock => _lock;

    public void Clear()
    {
        _root = null;
    }

    public void Insert(AabbBox bounds, int id)
    {
        _root = Insert(bounds, in id, _root);
    }

    private AabbTreeNode Insert(AabbBox bounds, in int id, AabbTreeNode? parent)
    {
        if (parent == null)
        {
            return new AabbTreeNode(bounds, id);
        }

        if (parent.IsLeaf)
        {
            parent.Left = new AabbTreeNode(parent.Bounds, parent.Id);
            parent.Right = new AabbTreeNode(bounds, id);

            parent.Bounds = AabbBox.Combine(parent.Bounds, bounds);
        }
        else
        {
            float leftVolumeIncrease =
                AabbBox.VolumeDifference(parent.Left!.Bounds, AabbBox.Combine(parent.Left.Bounds, bounds));
            float rightVolumeIncrease =
                AabbBox.VolumeDifference(parent.Right!.Bounds, AabbBox.Combine(parent.Right.Bounds, bounds));

            if (leftVolumeIncrease < rightVolumeIncrease)
            {
                parent.Left = Insert(bounds, in id, parent.Left);
            }
            else
            {
                parent.Right = Insert(bounds, in id, parent.Right);
            }

            parent.Bounds = AabbBox.Combine(parent.Left.Bounds, parent.Right.Bounds);
        }

        return parent;
    }

    public LinkedList<int> Query(AabbBox bounds)
    {
        LinkedList<int> result = new LinkedList<int>();
        if (_root == null)
        {
            return result;
        }

        Query(_root, bounds, result);
        return result;
    }

    private void Query(AabbTreeNode node, AabbBox bounds, LinkedList<int> overlaps)
    {
        if (!node.Bounds.Intersects(bounds))
        {
            return;
        }

        if (node.IsLeaf)
        {
            overlaps.AddLast(node.Id);
            return;
        }

        Query(node.Left!, bounds, overlaps);
        Query(node.Right, bounds, overlaps);
    }

    private class AabbTreeNode
    {
        private AabbBox _bounds;
        private AabbTreeNode? _left;
        private AabbTreeNode? _right;
        private readonly int _id = -1;

        public AabbTreeNode(AabbBox bounds)
        {
            _bounds = bounds;
        }

        public AabbTreeNode(AabbBox bounds, int id)
        {
            _bounds = bounds;
            _id = id;
        }

        public AabbTreeNode? Left
        {
            get => _left;
            set => _left = value;
        }

        public AabbTreeNode? Right
        {
            get => _right;
            set => _right = value;
        }

        public bool IsLeaf => _left == null && _right == null;

        public AabbBox Bounds
        {
            get => _bounds;
            set => _bounds = value;
        }

        public int Id
        {
            get => _id;
        }
    }
}

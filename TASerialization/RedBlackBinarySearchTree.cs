namespace TASerialization
{
    public class RedBlackBinarySearchTree<T>
    {
        private class TreeNode
        {
            public int Value;
            public T Data;
            public bool IsRed;
            public TreeNode Left;
            public TreeNode Right;

            public TreeNode(int value, T data, bool isRed)
            {
                Value = value;
                Data = data;
                IsRed = isRed;
            }
        }

        private TreeNode _origin;

        private TreeNode _RotateLeft(TreeNode node)
        {
            TreeNode result = node.Right;
            node.Right = result.Left;
            result.Left = node;
            result.IsRed = node.IsRed;
            node.IsRed = true;
            return result;
        }

        private TreeNode _RotateRight(TreeNode node)
        {
            TreeNode result = node.Left;
            node.Left = result.Right;
            result.Right = node;
            result.IsRed = node.IsRed;
            node.IsRed = true;
            return result;
        }

        private void _FlipColors(TreeNode node)
        {
            node.IsRed = true;
            if (node.Left != null) node.Left.IsRed = false;
            if (node.Right != null) node.Right.IsRed = false;
        }

        private bool _IsRed(TreeNode node)
        {
            return (node == null) || node.IsRed;
        }

        public void Insert(int value, T data)
        {
            _origin = _Insert(_origin, value, data);
            _origin.IsRed = false;
        }

        private TreeNode _Insert(TreeNode node, int value, T data)
        {
            if (node == null) return new TreeNode(value, data, true);
            if (value <= node.Value) node.Left = _Insert(node.Left, value, data);
            else node.Right = _Insert(node.Right, value, data);

            if (_IsRed(node.Right) && !_IsRed(node.Left)) node = _RotateLeft(node);
            if (_IsRed(node.Left) && (node.Left != null && _IsRed(node.Left.Left))) node = _RotateRight(node);
            if (_IsRed(node.Left) && _IsRed(node.Right)) _FlipColors(node);

            return node;
        }

        public T Find(int value)
        {
            TreeNode node = _origin;
            while (node != null)
            {
                int compareResult = value - node.Value;
                if (compareResult == 0) return node.Data;
                else if (compareResult > 0) node = node.Right;
                else node = node.Left;
            }
            return node.Data;
        }
    }
}
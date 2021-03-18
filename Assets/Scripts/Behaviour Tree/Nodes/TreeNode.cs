namespace WhiteWillow
{
    public abstract class TreeNode
    {
        public string Name { get; protected set; }
        public TreeNode Parent { get; protected set; }
        protected BehaviourTree Tree { get; }

        public TreeNode(BehaviourTree tree, string name)
        {
            Name = name;
            Tree = tree;
        }

        public void SetParent(TreeNode node) => Parent = node;
        public abstract Result Tick();
    }
}
namespace WhiteWillow
{
    public abstract class DecoratorNode : TreeNode
    {
        public TreeNode ChildNode { get; protected set; }

        public DecoratorNode(BehaviourTree tree, string name) : base(tree, name) { }
        public DecoratorNode(BehaviourTree tree, string name, TreeNode child) : base(tree, name) => ChildNode = child;

        public void SetChild(TreeNode node) => ChildNode = node;
    }

    public abstract class InvertorNode : DecoratorNode
    {
        public InvertorNode(BehaviourTree tree, string name) : base(tree, name) { }
        public InvertorNode(BehaviourTree tree, string name, TreeNode child) : base(tree, name, child) { }

        public override Result Tick()
        {
            Result childResult = ChildNode.Tick();

            if (childResult == Result.SUCCESS)
                return Result.FAILURE;
            else if (childResult == Result.FAILURE)
                return Result.SUCCESS;

            return childResult;
        }
    }

    public abstract class ForceSuccessNode : DecoratorNode
    {
        public ForceSuccessNode(BehaviourTree tree, string name) : base(tree, name) { }
        public ForceSuccessNode(BehaviourTree tree, string name, TreeNode child) : base(tree, name, child) { }

        public override Result Tick()
        {
            Result childResult = ChildNode.Tick();

            if (childResult == Result.FAILURE)
                return Result.SUCCESS;

            return childResult;
        }
    }

    public abstract class ForceFailureNode : DecoratorNode
    {
        public ForceFailureNode(BehaviourTree tree, string name) : base(tree, name) { }
        public ForceFailureNode(BehaviourTree tree, string name, TreeNode child) : base(tree, name, child) { }

        public override Result Tick()
        {
            Result childResult = ChildNode.Tick();

            if (childResult == Result.SUCCESS)
                return Result.FAILURE;

            return childResult;
        }
    }

    public abstract class RepeatNode : DecoratorNode
    {
        public RepeatNode(BehaviourTree tree, string name) : base(tree, name) { }
        public RepeatNode(BehaviourTree tree, string name, TreeNode child) : base(tree, name, child) { }

        public override Result Tick()
        {
            return Result.FAILURE;
        }
    }

    public abstract class RetryNode : DecoratorNode
    {
        public RetryNode(BehaviourTree tree, string name) : base(tree, name) { }
        public RetryNode(BehaviourTree tree, string name, TreeNode child) : base(tree, name, child) { }

        public override Result Tick()
        {
            return Result.FAILURE;
        }
    }
}
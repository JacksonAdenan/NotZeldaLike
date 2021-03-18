using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WhiteWillow
{
    class CompositeNode : TreeNode
    {
        public TreeNode RunningChild { get; protected set; }
        public List<TreeNode> Children { get; protected set; }

        public CompositeNode(BehaviourTree tree, string name) : base(tree, name) { }
        public CompositeNode(BehaviourTree tree, string name, List<TreeNode> children) : base(tree, name) => Children = children;
        public CompositeNode(BehaviourTree tree, string name, params TreeNode[] children) : base(tree, name)
        {
            Children = new List<TreeNode>();
            Children.AddRange(children);
        }

        public override Result Tick() { return Result.RUNNING; }
    }

    class SelectorNode : CompositeNode
    {
        public SelectorNode(BehaviourTree tree, string name) : base(tree, name) { }
        public SelectorNode(BehaviourTree tree, string name, List<TreeNode> children) : base(tree, name, children) { }
        public SelectorNode(BehaviourTree tree, string name, params TreeNode[] children) : base(tree, name, children) { }

        public override Result Tick()
        {
            foreach (var child in Children)
            {
                Result childResult = child.Tick();
                if (childResult != Result.FAILURE)
                    return childResult;
            }

            return Result.FAILURE;
        }
    }

    class SequenceNode : CompositeNode
    {
        public SequenceNode(BehaviourTree tree, string name) : base(tree, name) { }
        public SequenceNode(BehaviourTree tree, string name, List<TreeNode> children) : base(tree, name, children) { }
        public SequenceNode(BehaviourTree tree, string name, params TreeNode[] children) : base(tree, name, children) { }

        public override Result Tick()
        {
            foreach (var child in Children)
            {
                Result childResult = child.Tick();
                if (childResult != Result.SUCCESS)
                    return childResult;
            }

            return Result.SUCCESS;
        }
    }
}
using UnityEngine;

namespace WhiteWillow
{
    public class BehaviourTree
    {
        public bool Running;
        public TreeNode RootNode { get; private set; }
        public TreeNode RunningNode { get; private set; }

        public BehaviourTree() { }

        public void Tick()
        {
            if (RunningNode != null)
                RunningNode.Tick();
            else
                RootNode?.Tick();
        }

        public void SetRootNode(TreeNode node) => RootNode = node;
        public void SetRunningNode(TreeNode node) => RunningNode = node;
        public void ClearRunningNode() => RunningNode = null;
    }
}
using System;

namespace WhiteWillow
{
    class LeafNode : TreeNode
    {
        private Action m_Action;
        private Func<bool> m_Condition;

        public LeafNode(BehaviourTree tree, string name, Action action, Func<bool> condition) : base(tree, name)
        {
            m_Action = action;
            m_Condition = condition;
        }

        public override Result Tick()
        {
            if (m_Condition())
            {
                m_Action();

                if (m_Condition())
                {
                    Tree.SetRunningNode(this);
                    return Result.RUNNING;
                }
                else
                {
                    Tree.ClearRunningNode();
                    return Result.SUCCESS;
                }
            }

            Tree.ClearRunningNode();
            return Result.FAILURE;
        }
    }
}
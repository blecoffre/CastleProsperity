using System;

namespace CastleProsperity.AI.BehaviourTree
{
    public class ActionNode : Node
    {
        private readonly System.Action _action;
        public ActionNode(System.Action action)
        {
            _action = action;
        }

        public override NodeState Evaluate()
        {
            // Call the action and return success
            _action();
            _state = NodeState.Success;
            return _state;
        }
    }
}
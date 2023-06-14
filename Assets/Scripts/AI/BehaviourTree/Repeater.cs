using System;
using System.Collections.Generic;

namespace CastleProsperity.AI.BehaviourTree
{
    public class Repeater : Node
    {
        private readonly List<Node> _childNodes;

        public Repeater(List<Node> children) : base(children)
        {
            _childNodes = children ?? throw new ArgumentNullException(nameof(children));
        }

        public override NodeState Evaluate()
        {
            while (true)
            {
                // Evaluate the child nodes
                foreach (var child in _childNodes)
                {
                    child.Evaluate();

                    // If a child node fails, break out of the loop and return failure
                    if (child.State == NodeState.Failure)
                    {
                        _state = NodeState.Failure;
                        return State;
                    }

                    // If a child node is still running, return running
                    if (child.State == NodeState.Running)
                    {
                        _state = NodeState.Running;
                        return State;
                    }
                }

                _state = NodeState.Success;
                return State;
            }
        }
    }
}

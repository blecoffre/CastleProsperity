using System.Collections.Generic;
using UnityEngine;

namespace CastleProsperity.AI.BehaviourTree
{
    public class CommandSequence : Node
    {
        private readonly List<IOrder> _commands;

        public CommandSequence(List<IOrder> commands)
        {
            _commands = commands;
        }

        public override NodeState Evaluate()
        {
            foreach (IOrder command in _commands)
            {
                if (!command.ExecuteOrder())
                {
                    _state = NodeState.Failure;
                    return _state;
                }
            }

            _state = NodeState.Success;
            return _state;
        }
    }
}
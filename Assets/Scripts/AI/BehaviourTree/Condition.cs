using CastleProsperity.Building;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace CastleProsperity.AI.BehaviourTree
{
    public class Condition : Node
    {
        private Func<bool> _condition;

        public Condition(Func<bool> condition)
        {
            _condition = condition;

            // Log the expression used in the _condition Func<bool>
            string expression = _condition.Method + " -> " + _condition.Target;
            Debug.Log(expression);
        }

        public override NodeState Evaluate()
        {
            if (_condition())
            {
                _state = NodeState.Success;
            }
            else
            {
                _state = NodeState.Failure;
            }

            return _state;
        }
    }
}

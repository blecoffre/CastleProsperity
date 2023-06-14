using CastleProsperity.Building;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace CastleProsperity.AI.BehaviourTree
{
    class ExecuteOrder : Node
    {
        public ExecuteOrder()
        {
            
        }

        public override NodeState Evaluate()
        {
            return NodeState.Success;
        }
    }
}
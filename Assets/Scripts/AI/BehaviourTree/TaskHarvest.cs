using CastleProsperity.Building;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace CastleProsperity.AI.BehaviourTree
{
    class TaskHarvest : Node
    {
        private Transform _transform;

        public TaskHarvest(Transform transform)
        {
            _transform = transform;
        }

        public override NodeState Evaluate()
        {
            _state = NodeState.Success;
            return _state;
        }
    }
}
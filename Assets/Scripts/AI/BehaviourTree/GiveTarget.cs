using CastleProsperity.Building;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace CastleProsperity.AI.BehaviourTree
{
    class GiveTarget : Node
    {
        private Func<Vector3> _getTargetPosition;

        public GiveTarget(Func<Vector3> getTargetPosition)
        {
            _getTargetPosition = getTargetPosition;
        }

        public override NodeState Evaluate()
        {
            Debug.Log("Give target" + _getTargetPosition());
            Parent.SetData("target", _getTargetPosition());       

            _state = NodeState.Success;
            return _state;
        }
    }
}
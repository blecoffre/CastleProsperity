using CastleProsperity.Building;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace CastleProsperity.AI.BehaviourTree
{
    class Wait : Node
    {
        private float _waitingTime;
        private float _timer;

        public Wait(float waitingTime)
        {
            _waitingTime = waitingTime;
            _timer = 0.0f;
        }

        public override NodeState Evaluate()
        {
            _timer += Time.deltaTime;

            if(_timer < _waitingTime)
            {
                _state = NodeState.Running;
            }
            else
            {
                _timer = 0.0f;
                _state = NodeState.Success;
            }

            return _state;
        }
    }
}
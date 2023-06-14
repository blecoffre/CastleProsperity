using CastleProsperity.Building;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace CastleProsperity.AI.BehaviourTree
{
    class TaskTransportResource : Node
    {
        private Transform _transform;
        private NavMeshAgent _agent;
        private Vector3 _target;

        public TaskTransportResource(Transform transform, NavMeshAgent navMeshAgent)
        {
            _transform = transform;
            _agent = navMeshAgent;
        }

        public override NodeState Evaluate()
        {
            if(!_agent.hasPath)
            {
                _agent.SetDestination(_target);

                _state = NodeState.Running;
                return _state;
            } 

            if(Vector3.Distance(_transform.position, _target) <= 0.01f)
            {
                _state = NodeState.Success;
                return _state;
            }

            _state = NodeState.Running;
            return _state;
        }
    }
}
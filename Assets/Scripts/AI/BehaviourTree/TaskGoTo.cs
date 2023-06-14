using CastleProsperity.Building;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace CastleProsperity.AI.BehaviourTree
{
    class TaskGoTo : Node
    {
        private Transform _transform;
        private NavMeshAgent _agent;

        public TaskGoTo(Transform transform, NavMeshAgent navMeshAgent)
        {
            _transform = transform;
            _agent = navMeshAgent;
        }

        public override NodeState Evaluate()
        {
            Vector3 target = (Vector3)GetData("target");

            if (_agent == null || _transform == null || target == null)
            {
                Debug.LogError("TaskGoTo: Transform, NavMeshAgent, or target not set.");
                _state = NodeState.Failure;
                return _state;
            }

            if(_agent.destination.x != target.x && _agent.destination.z != target.z)
            {
                _agent.SetDestination(target);
            }

            if (_agent.remainingDistance > _agent.stoppingDistance)
            {
                _state = NodeState.Running;
                return _state;
            }

            if (Vector3.Distance(_transform.position, target) <= _agent.stoppingDistance)
            {
                ClearData("target");
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace CastleProsperity.AI.BehaviourTree
{
    class TaskWalk : Node
    {
        private Transform _transform;
        private NavMeshAgent _agent;
        private float _speed;
        private bool _hasTarget = false;

        private List<Vector3> _pathToTarget;

        public TaskWalk(Transform transform, NavMeshAgent agent, float speed)
        {
            _transform = transform;
            _agent = agent;
            _speed = speed;
        }

        public override NodeState Evaluate()
        {
            _state = NodeState.Running;

            if (_pathToTarget == null)
            {
                Vector3 targetWorldPos = (Vector3)GetData("target");

                if (Vector3.Distance(_transform.position, targetWorldPos) < 0.01f)
                {
                    _state = NodeState.Success;
                }
                else
                {
                    _agent.SetDestination(targetWorldPos);
                    _pathToTarget = _agent.path.corners.ToList();

                    if (_pathToTarget == null)
                    {
                        _state = NodeState.Failure;
                    }
                } 
            }
            else if(_pathToTarget.Count > 0)
            {
                Vector3 t = _pathToTarget[0];

                if(Vector3.Distance(_transform.position, t) < 0.01f)
                {
                    _transform.position = t;
                    _pathToTarget.RemoveAt(0);

                    if (_pathToTarget.Count == 0)
                    {
                        _state = NodeState.Success;
                        _pathToTarget = null;
                    }
                }
                else
                {

                }
            }

            return _state;
        }
    }
}

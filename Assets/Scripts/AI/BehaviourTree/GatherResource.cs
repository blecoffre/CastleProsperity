using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CastleProsperity.AI.BehaviourTree
{
    class GatherResource : Node
    {
        private Transform _agentTr;
        private Transform[] _waypoints;

        private int _currentWaypointIndex = 0;

        private float _gatheringDuration = 0.0f;
        private float _gatheringCounter = 0.0f;
        private bool _gathering = false;

        public GatherResource(Transform agentTr, Transform[] waypoints, float gatheringDuration)
        {
            _agentTr = agentTr;
            _waypoints = waypoints;
            _gatheringDuration = gatheringDuration;
        }

        public override NodeState Evaluate()
        {
            return NodeState.Failure;
        //    if (_gathering)
        //    {
        //        _gatheringCounter += Time.deltaTime;

        //        if(_gatheringCounter < _gatheringDuration)
        //        {
        //            return NodeStateEnum.Success;
        //        }

        //        _gathering = false;
        //    }

        //    Transform wp = _waypoints[_currentWaypointIndex];
        //    if(Vector3.Distance(_agentTr.position, wp.position) < 0.01f)
        //    {
        //        _agentTr.position = wp.position;
        //        _gatheringCounter = 0.0f;
        //        _gathering = true;

        //        _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
        //    }
        //    else
        //    {
        //        _agentTr.position = Vector3.MoveTowards(_agentTr.position, wp.position, 10 * Time.deltaTime);
        //        _agentTr.LookAt(wp.position);
        //    }
        }
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CastleProsperity.AI.BehaviourTree
{
    public class Sequence : Node
    {
        private bool _isRandom;
        public Sequence() : base() { _isRandom = false; }
        public Sequence(bool isRandom) : base() { _isRandom = isRandom; }
        public Sequence(List<Node> children, bool isRandom = false) : base(children)
        {
            _isRandom = isRandom;
        }

        public override bool IsFlowNode => true;

        public static List<T> Shuffle<T>(List<T> list)
        {
            System.Random r = new System.Random();
            return list.OrderBy(x => r.Next()).ToList();
        }

        public override NodeState Evaluate()
        {
            bool anyChildIsRunning = false;

            if (_isRandom)
            {
                _children = Shuffle(_children);
            }

            foreach (Node node in _children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.Failure:
                        _state = NodeState.Failure;
                        return _state;
                    case NodeState.Success:
                        continue;
                    case NodeState.Running:
                        _state = NodeState.Running;
                        anyChildIsRunning = true;
                        return _state;
                    default:
                        _state = NodeState.Success;
                        return _state;
                }
            }

            _state = anyChildIsRunning ? NodeState.Running : NodeState.Success;
            return _state;
        }
    }
}
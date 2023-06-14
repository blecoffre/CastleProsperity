using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CastleProsperity.AI.BehaviourTree
{
    public abstract class Tree : MonoBehaviour
    {
        private Node _root = null;

        protected virtual void Start()
        {
            _root = SetupTree();
        }

        private void Update()
        {
            if(_root != null)
            {
                _root.Evaluate();
            }
        }

        protected abstract Node SetupTree();
    }
}

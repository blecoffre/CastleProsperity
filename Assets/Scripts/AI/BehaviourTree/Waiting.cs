using UnityEngine;

namespace CastleProsperity.AI.BehaviourTree
{
    class Waiting : Node
    {
        private string _name;

        public Waiting(string name)
        {
            _name = name;
        }

        public override NodeState Evaluate()
        {
            //Animation stuff ?
            Debug.Log($"{_name} is waiting");
            return NodeState.Success;
        }
    }
}

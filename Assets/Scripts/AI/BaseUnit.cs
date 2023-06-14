using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CastleProsperity.AI.BehaviourTree;
using UnityEngine.AI;
using Cysharp.Threading.Tasks;
using CastleProsperity.Utils;

namespace CastleProsperity.AI.BehaviourTree
{
    public abstract class BaseUnit : Tree, ISelectable, IMovable
    {
        [SerializeField] private float _moveSpeed = 2.0f;

        protected NavMeshAgent _agent = default;
        protected PNJCanvas _canvas = default;

        protected bool _hasOrder = false;
        protected Vector3 _targetPosition = default;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _canvas = GetComponentInChildren<PNJCanvas>();

            _agent.speed = _moveSpeed;
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override Node SetupTree()
        {
            Node root = new Selector(new List<Node>
            {
                new Sequence(new List<Node>
                {
                    new Waiting(gameObject.name)
                })
            });

            return root;
        }

        public virtual void Select()
        {
            _canvas.SelectedFeedbackDisplayState(true);
        }

        public virtual void Deselect()
        {
            _canvas.SelectedFeedbackDisplayState(false);
        }

        public async UniTask Move(Vector3 targetPosition)
        {
            _agent.SetDestination(targetPosition);

            await UniTask.WaitUntil(() => NavMeshUtils.DestinationReached(_agent, transform.position) is true);
        }
    }
}
using CastleProsperity.Building;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CastleProsperity.AI.BehaviourTree
{
    public class HarvestingUnit : BaseUnit, IHarvester
    {
        UnitInventory _inventory = new UnitInventory();

        private bool _isInHarvestingBuilding = false;


        protected override Node SetupTree()
        {

            Node root = new Selector(new List<Node>
            {
                //Execute player orders
                new Sequence(new List<Node>
                {
                    new Condition(() => _hasOrder == true),
                    new ExecuteOrder(), //Move, harvest etc..
                    new ActionNode(() => _hasOrder = false),
                }),


                //Automatic behaviour
                new Sequence(new List<Node>
                {
                    new Condition(() => _isInHarvestingBuilding),

                    new Selector(new List<Node>
                    {
                        //Transport resources to warehouse
                        new Sequence(new List<Node>
                        {
                            new Condition(() => _inventory.GetProgress() == 1),
                            //new TaskTransportResource(),
                            //new AddResourceToInventory(() => _collectedResources = 0)
                        }),

                        // Harvest resources until inventory is full
                        new Sequence(new List<Node>
                        {
                            new Repeater(new List<Node>
                            {
                                new Condition(() => _inventory.GetProgress() < 1),
                                //new TaskHarvest(),
                                //new Wait(),
                                new ActionNode(() => _inventory.AddStack(1)),
                            })
                        }),
                    })
                }),

                new Sequence(new List<Node>
                {
                    new Waiting(gameObject.name)
                })
            });
            return root;
        }

        public override void Select()
        {
            _canvas.SelectedFeedbackDisplayState(true);
        }

        public override void Deselect()
        {
            _canvas.SelectedFeedbackDisplayState(false);
        }

        public async void Harvest(Vector3 target)
        {
            await Move(target);

        }

        public void Harvest(IResourceField resource)
        {
            throw new System.NotImplementedException();
        }
    }
}

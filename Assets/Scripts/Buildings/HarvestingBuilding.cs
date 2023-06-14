using CastleProsperity.AI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CastleProsperity.Building
{
    [ExecuteInEditMode]
    public class HarvestingBuilding : BaseBuilding, IHarvestingBuilding
    {
        [SerializeField] private float _managementScope = 25.0f;
        [SerializeField] private int _maxHarvesterCapacity = 3;
        private List<IResourceField> _resources = new List<IResourceField>();
        private OpenWarehouse _nearestWarehouse = default;

        public List<IHarvester> AssignedHarvester { get; set; }

        private void Awake()
        {
            GetHarvestingResourcesAround();
            GetNearbyWarehouse();
        }

        public override void Start()
        {
            
        }

        public override void Update()
        {

        }

        private void GetHarvestingResourcesAround()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, _managementScope);
            foreach (var hitCollider in hitColliders)
            {
                IResourceField resource;
                hitCollider.TryGetComponent<IResourceField>(out resource);

                if (resource != null)
                {
                    _resources.Add(resource);
                }
            }
        }

        private void GetNearbyWarehouse()
        {
            OpenWarehouse[] houses = FindObjectsOfType<OpenWarehouse>();
            _nearestWarehouse = houses.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).ToArray()[0];
        }

#if UNITY_EDITOR
        protected virtual void OnDrawGizmos()
        {
            UnityEditor.Handles.color = Color.blue;
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, _managementScope);

            foreach (BaseGatherable resource in _resources)
            {
                UnityEditor.Handles.DrawWireDisc(resource.transform.position, Vector3.up, resource.GetComponent<BoxCollider>().size.x / 2);
            }

            UnityEditor.Handles.DrawLine(transform.position, _nearestWarehouse.transform.position);
        }
#endif

        public bool AssignWorkerSuccessfullyOnAvailableSlot(IHarvester unit)
        {
            if(AssignedHarvester.Count < _maxHarvesterCapacity)
            {
                AssignedHarvester.Add(unit);
                return true;
            }

            return false;
        }

        public HarvestOrder GiveHarvestOrder(IHarvester harvester)
        {
            return new HarvestOrder(harvester, _resources.First(x => x.IsAvailable()));
        }
    }
}
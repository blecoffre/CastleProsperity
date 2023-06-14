using CastleProsperity.AI;
using UnityEngine;

public class HarvestOrder : IOrder
{
    private IResourceField _resource;
    private IHarvester _harvestUnit;

    public HarvestOrder(IHarvester unit, IResourceField resource)
    {
        this._harvestUnit = unit;
        this._resource = resource;
    }

    public bool ExecuteOrder()
    {
        _harvestUnit.Harvest(_resource);
        return true;
    }
}


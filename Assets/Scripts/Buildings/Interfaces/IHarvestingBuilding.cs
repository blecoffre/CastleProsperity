using CastleProsperity.AI;
using CastleProsperity.Building;
using System.Collections.Generic;

public interface IHarvestingBuilding
{
    public List<IHarvester> AssignedHarvester { get; set; }
    public bool AssignWorkerSuccessfullyOnAvailableSlot(IHarvester unit);
    public HarvestOrder GiveHarvestOrder(IHarvester harvester);
}
using CastleProsperity.Building;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CastleProsperity.Building.SO
{
    [CreateAssetMenu(fileName = "BuildingData", menuName = "ScriptableObjects/BuildingData", order = 1)]
    public class BuildingDataScriptableObject : ScriptableObject
    {
        [SerializeField] private GhostObject _ghostBuilding;
        [SerializeField] private BaseBuilding _finalBuilding;
        [SerializeField] private BuildingUnlockConditionBase[] _unlockConditions;

        public GhostObject GhostBuilding => _ghostBuilding;
        public BaseBuilding FinalBuilding => _finalBuilding;
        public BuildingUnlockConditionBase[] UnlockConditions => _unlockConditions;
    }
}

using CastleProsperity.Building;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CastleProsperity.Building.SO
{
    [CreateAssetMenu(fileName = "BuildingListScriptableObject", menuName = "ScriptableObjects/BuildingListScriptableObject", order = 1)]
    public class BuildingListScriptableObject : ScriptableObject
    {
        [SerializeField] private BuildingKeyValuePair[] _buildings = default;

        public BuildingKeyValuePair[] Buildings => _buildings;   
    }

    [System.Serializable]
    public class BuildingKeyValuePair
    {
        public BuildingEnum key;
        public BuildingDataScriptableObject value;
    }
}

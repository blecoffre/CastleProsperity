using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CastleProsperity.Building.SO
{
    [CreateAssetMenu(fileName = "BuildingListForUIScriptableObject", menuName = "ScriptableObjects/BuildingListForUIScriptableObject", order = 1)]
    public class BuildingListForUIScriptableObject : ScriptableObject
    {
        [SerializeField] private BuildingTypeIconPair[] _buildings = default;

        public BuildingTypeIconPair[] Buildings => _buildings;
    }

    [System.Serializable]
    public class BuildingTypeIconPair
    {
        public BuildingEnum type;
        public Sprite icon;
    }
}

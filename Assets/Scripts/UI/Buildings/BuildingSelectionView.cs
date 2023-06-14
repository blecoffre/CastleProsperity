using CastleProsperity.Building.SO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BuildingSelectionView : MonoBehaviour
{
    [SerializeField] private Transform _content = default;

    [Inject] private BuildingButton _buttonPrefab = default;
    [Inject] private BuildingListForUIScriptableObject _buildingList = default;
    [Inject] private SignalBus _signalBus = default;

    private void Start()
    {
        CreateBuildingOptions();
    }

    private void CreateBuildingOptions()
    {
        if (_buildingList != null && _buildingList.Buildings != null && _buildingList.Buildings.Length > 0)
        {
            for (int i = 0; i < _buildingList.Buildings.Length; i++)
            {
                BuildingButton newButton = Instantiate(_buttonPrefab);
                newButton.SetIcon(_buildingList.Buildings[i].icon);
                BuildingEnum type = _buildingList.Buildings[i].type;
                newButton.BindToOnClick(() => BuildingSelected(type));

                AddOption(newButton);
            }
        }
        else
        {
            Debug.LogError("Missing building list data");
        }
    }

    private void AddOption(BuildingButton newButton)
    {
        newButton.transform.SetParent(_content);
    }

    private void BuildingSelected(BuildingEnum type)
    {
        _signalBus.Fire(
                new SignalPreviewBuilding() { buildingType = type }
            );
    }
}

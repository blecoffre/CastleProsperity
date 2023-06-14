using CastleProsperity.Building;
using CastleProsperity.Building.Interface;
using CastleProsperity.Building.SO;
using UnityEngine;
using Zenject;

public class BuildingInstaller : MonoInstaller
{
    [SerializeField] private BuildingButton _buildingButtonPrefab = default;
    [SerializeField] private BuildingListForUIScriptableObject _buildingListForUi = default;
    [SerializeField] private BuildingListScriptableObject _buildingList = default;

    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        
        Container.BindFactory<Object, BaseBuilding, BaseBuilding.Factory>().FromFactory<PrefabFactory<BaseBuilding>>();


        //Prefab bindings
        Container.Bind<BuildingButton>().FromInstance(_buildingButtonPrefab).AsSingle();
        Container.Bind<BuildingListForUIScriptableObject>().FromInstance(_buildingListForUi).AsSingle();
        Container.Bind<BuildingListScriptableObject>().FromInstance(_buildingList).AsSingle();

        //Components Bindings
        Container.Bind<BuildingSelectionView>().FromComponentInHierarchy().AsSingle();
        Container.Bind<BuildingPlacementController>().FromComponentInHierarchy().AsSingle();

        BindSignals();
    }

    private void BindSignals()
    {
        Container.DeclareSignalWithInterfaces<SignalPreviewBuilding>();
    }
}

public struct SignalPreviewBuilding : ISignalBuildingPreviewer
{
    public BuildingEnum buildingType;
}
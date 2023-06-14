using SLE.Systems.Selection;
using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<SelectionHandler>().FromComponentInHierarchy().AsSingle();
        Container.Bind<PNJSelectionController>().FromComponentInHierarchy().AsSingle();

    }
}
using CastleProsperity.Building.Interface;
using CastleProsperity.Building.SO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.Linq;
using UnityEngine.InputSystem;
using System;
using CastleProsperity.Building;

public class BuildingPlacementController : MonoBehaviour
{
    [SerializeField] private float _ghostRotationSpeed = 50.0f;

    [Inject] private SignalBus _signalBus = default;
    [Inject] private BuildingListScriptableObject _buildings = default;

    private PlayertControlActions _playerActions;
    private BuildingEnum _buildingType = default;
    private GhostObject _currentGhostBuilding = default;
    private RaycastHit _hit;

    private int _terrainLayerMask = 1 << 8;

    private InputAction _rotationDirection = default;

    private void Awake()
    {
        _playerActions = new PlayertControlActions();
    }

    private void OnEnable()
    {
        _signalBus.Subscribe<SignalPreviewBuilding>(x => InstantiateBuildingPreview(x.buildingType));
        _playerActions.Player.MouseClick.performed += PlaceBuilding;
        _playerActions.Player.Escape.performed += AbortBuilding;
        _rotationDirection = _playerActions.Player.Rotation;
        _playerActions.Enable();
    }

    private void OnDisable()
    {
        _signalBus.TryUnsubscribe<SignalPreviewBuilding>(x => InstantiateBuildingPreview(x.buildingType));
        _playerActions.Player.MouseClick.performed -= PlaceBuilding;
        _playerActions.Player.Escape.performed -= AbortBuilding;

        _playerActions.Disable();
    }

    private void InstantiateBuildingPreview(BuildingEnum buildingType)
    {
        DestroyGhostBuilding();

        _buildingType = buildingType;
        InstantiateBuildingPreviewObject(_buildings.Buildings.FirstOrDefault(x => x.key == buildingType).value.GhostBuilding);
    }

    private void InstantiateBuildingPreviewObject(GhostObject ghost)
    {
        _currentGhostBuilding = Instantiate(ghost, Vector3.zero, Quaternion.identity);
    }

    private void Update()
    {
        if (_currentGhostBuilding != null)
        {
            MovePhantomBuilding();
            CheckValidPlacement();
            RotateBuilding();
        }
    }

    private void MovePhantomBuilding()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out _hit, 1000f, _terrainLayerMask))
        {
            _currentGhostBuilding.transform.position = _hit.point;
        }
    }

    private void CheckValidPlacement()
    {
        if (_currentGhostBuilding.nCollisions == 0 && _currentGhostBuilding.CheckFourCornerDistanceWithTerrain())
        {
            _currentGhostBuilding.IsInValidPlacement();
        }
        else
        {
            _currentGhostBuilding.IsInInvalidPlacement();
        }
    }

    private void PlaceBuilding(InputAction.CallbackContext obj)
    {
        if (_currentGhostBuilding != null)
        {
            BaseBuilding building = Instantiate(_buildings.Buildings.FirstOrDefault(x => x.key == _buildingType).value.FinalBuilding);
            building.transform.position = _currentGhostBuilding.transform.position;
            building.transform.rotation = _currentGhostBuilding.transform.rotation;

            DestroyGhostBuilding();
            _currentGhostBuilding = null;
        }
    }

    private void AbortBuilding(InputAction.CallbackContext obj)
    {
        DestroyGhostBuilding();
    }

    private void DestroyGhostBuilding()
    {
        if (_currentGhostBuilding != null)
        {
            Destroy(_currentGhostBuilding.gameObject);
        }
    }

    private void RotateBuilding()
    {
        float direction = _rotationDirection.ReadValue<float>();
        Debug.Log(direction);
        _currentGhostBuilding.transform.Rotate(0, direction * _ghostRotationSpeed * Time.deltaTime, 0);
    }
}

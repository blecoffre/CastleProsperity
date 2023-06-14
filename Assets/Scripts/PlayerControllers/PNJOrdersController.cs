using SLE.Systems.Selection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using System.Linq;
using CastleProsperity.AI;

public class PNJOrdersController : MonoBehaviour
{
    private SelectionHandler _selectionHandler = default;
    private Camera _mainCamera = default;
    private OrderQueue _orderQueue = new OrderQueue();

    [Inject]
    private void Init(SelectionHandler selectionHandler)
    {
        _selectionHandler = selectionHandler;
    }

    private void Start()
    {
        _mainCamera = FindObjectOfType<CameraController>().Camera;
    }

    private void Update()
    {
        RightClickTarget();
    }

    private void RightClickTarget()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            RaycastHit hit;
            if (Physics.Raycast(_mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()), out hit))
            {
                _orderQueue.ClearQueue();

                //Check if right click target is a havresting building
                IHarvestingBuilding harvestBuilding = hit.collider.GetComponent<IHarvestingBuilding>();
                if (harvestBuilding != null)
                {
                    AssignToHarvestingBuilding(harvestBuilding);
                }

                MoveTo(hit.point);
            }
        }

        ExecuteOrderQueue();
    }

    private void AssignToHarvestingBuilding(IHarvestingBuilding building)
    {
        bool _isFull = false;
        foreach (ISelectable unit in _selectionHandler.currentSelection)
        {
            IHarvester harvester = unit as IHarvester;
            if (harvester != null)
            {
                _isFull = building.AssignWorkerSuccessfullyOnAvailableSlot(harvester);
                if (_isFull)
                {
                    break;
                }

                _orderQueue.Enqueue(building.GiveHarvestOrder(harvester));
            }
        }
    }

    private void MoveTo(Vector3 position)
    {
        foreach (ISelectable unit in _selectionHandler.currentSelection)
        {
            if (unit is IMovable)
            {
                MoveOrder moveOrder = new MoveOrder((unit as IMovable), position);
                _orderQueue.Enqueue(moveOrder);
            }
        }
    }

    private void ExecuteOrderQueue()
    {
        _orderQueue.ExecuteNextOrder();
    }
}

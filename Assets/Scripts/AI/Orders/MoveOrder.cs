using UnityEngine;

public class MoveOrder : IOrder
{
    private Vector3 _targetPosition;
    private IMovable _movableUnit;

    public MoveOrder(IMovable unit, Vector3 targetPosition)
    {
        this._movableUnit = unit;
        this._targetPosition = targetPosition;
    }

    public bool ExecuteOrder()
    {
        _movableUnit.Move(_targetPosition);
        return true;
    }
}
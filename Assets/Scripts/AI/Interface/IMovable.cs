using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IMovable
{
    public UniTask Move(Vector3 destination);
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGatherable : MonoBehaviour, IGatherable
{
    public abstract IResourceField Gather();

    public abstract bool IsAvailable();
}
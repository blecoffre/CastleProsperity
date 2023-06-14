using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGatherable
{
    public IResourceField Gather();

    public bool IsAvailable();
}

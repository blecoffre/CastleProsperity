using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheatField : Field
{
    public override IResourceField Gather()
    {
        Debug.Log("Gathering Wheat on field");
        throw new System.NotImplementedException();
    }

    public override bool IsAvailable()
    {
        return true;
    }
}

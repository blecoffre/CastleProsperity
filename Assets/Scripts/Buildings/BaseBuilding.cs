using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CastleProsperity.Building
{
    public abstract class BaseBuilding : MonoBehaviour, IBuilding, ISelectable
    {


        public abstract void Start();

        public abstract void Update();

        public class Factory : PlaceholderFactory<Object, BaseBuilding>
        {

        }

        public void Deselect()
        {

        }

        public void Select()
        {

        }
    }
}

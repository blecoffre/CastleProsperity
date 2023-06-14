using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CastleProsperity.AI
{
    class UnitInventory
    {
        private int _maxStack;
        private int _currentStackCount;

        private float _progress;
        public UnitInventory()
        {

        }

        public UnitInventory(int maxStack)
        {
            _maxStack = maxStack;
        }

        public void AddStack(int count)
        {
            _currentStackCount += count;

            if(_currentStackCount > _maxStack)
            {
                _currentStackCount = _maxStack;
            }
        }

        private void ComputeProgress()
        {
            _progress = _currentStackCount / _maxStack * 100;

        }

        public float GetProgress()
        {
            ComputeProgress();
            return _progress;
        }
    }
}

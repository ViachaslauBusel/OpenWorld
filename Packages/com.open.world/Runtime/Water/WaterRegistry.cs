using System.Collections.Generic;
using UnityEngine;

namespace OpenWorld.Water.Water
{
    public class WaterRegistry
    {
        private List<WaterTag> _waters = new List<WaterTag>();

        internal void Register(WaterTag water)
        {
            _waters.Add(water);
        }

        internal void Unregister(WaterTag water)
        {
            _waters.Remove(water);
        }

        public WaterTag GetWaterInPoint(Vector3 point)
        {
            foreach (var water in _waters)
            {
                if (water.IsPointInWater(point))
                {
                    return water;
                }
            }
            return null;
        }
    }
}

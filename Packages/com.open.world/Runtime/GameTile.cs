using System.Collections.Generic;
using UnityEngine;

namespace OpenWorld
{
    public class GameTile : BaseTile
    {
        //private List<GameObject> _objects = new();
        internal override void Destroy()
        {
            Destroy(gameObject);
        }
    }
}

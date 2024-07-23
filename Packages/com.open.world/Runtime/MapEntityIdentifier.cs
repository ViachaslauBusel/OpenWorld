using UnityEngine;

namespace OpenWorld
{
    public class MapEntityIdentifier : MonoBehaviour
    {
        private int _id;

        public int ID => _id;

        public void Initialize(int id)
        {
            if(_id != 0) Debug.LogWarning("ID already set");

            _id = id;
        }
    }
}

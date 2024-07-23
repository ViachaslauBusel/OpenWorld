using System;
using UnityEngine;

namespace OpenWorld
{
    public class MapEntityIdentifier : MonoBehaviour
    {
        private int _id;

        public int ID => _id;

        public void Initialize(int id, bool notify = true)
        {
            _id = id;

            if(notify) OnIdentifierInitialize();
        }

        public void NotifyOnDestroyIdentifier()
        {
            if (_id == 0) Debug.LogWarning("ID not set");

            OnIdentifierDestroy();
        }

        /// <summary>
        /// Notification of identifier initialization
        /// </summary>
        protected virtual void OnIdentifierInitialize()
        {
        }

        /// <summary>
        /// Notification of identifier destruction
        /// </summary>
        protected virtual void OnIdentifierDestroy()
        {
        }
    }
}

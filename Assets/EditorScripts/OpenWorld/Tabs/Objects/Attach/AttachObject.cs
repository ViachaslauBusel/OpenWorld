#if UNITY_EDITOR
using UnityEngine;

namespace OpenWorld.Tools.Objects
{
    public struct AttachObject
    {
        /// <summary>
        /// Обьект на сцене
        /// </summary>
        public GameObject Object { get; }
        /// <summary>
        /// Префаб
        /// </summary>
        public GameObject Prefab { get; }

        public AttachObject(GameObject sceneObj, GameObject prefab)
        {
            this.Object = sceneObj;
            this.Prefab = prefab;
        }
    }
}
#endif
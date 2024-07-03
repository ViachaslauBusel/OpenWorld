using UnityEngine;

namespace OpenWorldEditor.MapObjectTab.Attach
{
    public struct AttachObject
    {
        /// <summary>
        /// The object in the scene.
        /// </summary>
        public GameObject SceneObject { get; }
        /// <summary>
        /// The prefab associated with the scene object.
        /// </summary>
        public GameObject Prefab { get; }

        public AttachObject(GameObject sceneObject, GameObject prefab)
        {
            this.SceneObject = sceneObject;
            this.Prefab = prefab;
        }
    }
}

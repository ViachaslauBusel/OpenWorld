using DATA;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OpenWorld.DATA
{
    /// <summary>
    /// Данные для сохранение одного элемента тайла
    /// </summary>
    [System.Serializable]
    public class MapObject
    {
        public Vector3 Position => m_position;
        public Quaternion Rotation => m_rotation;
        public Vector3 Scale => m_scale;
        public Prefab<GameObject> Prefab => m_prefab;

        [SerializeField] Vector3 m_position;
        [SerializeField] Quaternion m_rotation;
        [SerializeField] Vector3 m_scale;
        [SerializeField] Prefab<GameObject> m_prefab;

#if UNITY_EDITOR
        public MapObject(GameObject prefab, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            m_prefab = new Prefab<GameObject>(prefab);
            m_position = position;
            m_rotation = rotation;
            m_scale = scale;
        }
#endif
    }
}
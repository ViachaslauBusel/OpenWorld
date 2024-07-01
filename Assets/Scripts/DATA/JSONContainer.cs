using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace DATA
{
    /// <summary>
    /// Контейнер позволяющий сериализовать/десериализовать произвольные классы
    [System.Serializable]
    public class JSONContainer : ISerializationCallbackReceiver
    {
        [SerializeField] string m_serserializedAddition;
        [SerializeField] string m_serserializedType;

        /// <summary>
        /// Объект который нужно сериализовать/десериализовать
        /// </summary>
        [NonSerialized] 
        private System.Object m_obj;
        public System.Object Object { get => m_obj; set { m_obj = value; } }

        public T GetSerializedOBJ<T>() where T: class
        {
         
            return Object as T;
        }
        //------------------------------------------------------------------

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (string.IsNullOrEmpty(m_serserializedType) || string.IsNullOrEmpty(m_serserializedAddition)) return;
            try
            {
                m_obj = (System.Object)JsonUtility.FromJson(m_serserializedAddition, Type.GetType(m_serserializedType));
            }
            catch (Exception)
            {
                Debug.LogWarning($"Failed deserialize class type:{m_serserializedType} data:{m_serserializedAddition}");
            }
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            if (Object == null)
            {
                m_serserializedAddition = "";
                m_serserializedType = "";
                return;
            }

            m_serserializedAddition = JsonUtility.ToJson(Object);
            m_serserializedType = Object.GetType().ToString();
           // Debug.Log($"typeof:{ Object.GetType().GUID}"); 
        }
        //------------------------------------------------------------------
    }
}

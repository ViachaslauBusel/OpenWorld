using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DATA
{
    [System.Serializable]
    public class JSONList
    {
        
        [SerializeField] List<JSONContainer> m_list = new List<JSONContainer>();
        public void Add(System.Object obj)
        {
            JSONContainer container = new JSONContainer();
            container.Object = obj;
            m_list.Add(container);
        }

        public void RemoveAt(int index)
        {
            m_list.RemoveAt(index);
        }
        public int Count => m_list.Count;
        public T GetOBJ<T>(int index) => (T)m_list[index].Object;

        public System.Object this[int index] => m_list[index].Object;

        internal T Find<T>(Predicate<T> predicate) where T : class
        {
          return (T)m_list.Find((container) => container.Object is T && predicate.Invoke(container.GetSerializedOBJ<T>()))?.Object;
        }
         
        internal void RemoveAll<T>(Predicate<T> predicate) where T : class 
        {
            m_list.RemoveAll((container) => container.Object is T && predicate.Invoke(container.GetSerializedOBJ<T>()));
        }
    } 
}

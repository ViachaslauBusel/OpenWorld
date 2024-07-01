using Bundles;
using DATA;
using OpenWorld.Loader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace OpenWorld.Loader
{
    class TaskPrefabLoader<T> : ITask, IWorkTask, ICoroutineTask where T: UnityEngine.Object
    {
        public bool Completed { get; private set; } = false;
        private IEnumerator m_enumerator;
        private T m_prefab;
        private Action<T> m_action;
        public TaskPrefabLoader(Prefab<T> prefab, Action<T> action) 
        {
            m_enumerator = PrefabLoader(prefab);
            m_action = action;
        }

      

        public void Cancel()
        {
            m_enumerator = null;
            m_action = null;
            Completed = true;
        }

        public void Invoke()
        {
            m_action?.Invoke(m_prefab);
            Completed = true;
        }

        public bool MoveNext()
        {
            if (m_enumerator == null) return false;
            return m_enumerator.MoveNext();
        }

        private IEnumerator PrefabLoader(Prefab<T> prefab)
        {
            if (prefab == null) yield break;
#if UNITY_EDITOR
            m_prefab = prefab.Object;
            yield return null;
#else
            var request = BundlesManager.LoadAssetAsync(prefab);
            if (request == null)
            {
                Debug.LogError("Не удалось найти Bundle");
                yield break;
            }
        yield return request;
        m_prefab = request.asset as T;
        if(m_prefab == null) Debug.Log($"Error in wait or cast:{request.asset == null} : {request.asset.GetType()}");
#endif
        }
    }
}

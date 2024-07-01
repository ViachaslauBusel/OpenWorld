using DATA;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace OpenWorld.Loader
{
    /// <summary>
    /// Конвейер задач, контролирует время выполнения, разбивая задачи по кадрам
    /// </summary>
    public class TaskManager: MonoBehaviour
    {
        private const int TIME = 5;
        private static TaskManager m_instance { get; set; }
        private static Queue<IWorkTask> m_tasks = new Queue<IWorkTask>();
        private static LinkedList<ICoroutineTask> m_coroutines = new LinkedList<ICoroutineTask>();

        private void Awake()
        {
            if(m_instance != null) { Debug.LogWarning("An instance of the \"ObjectLoader\" script has already been attached to the GameObject");  Destroy(gameObject); return; }
            m_instance = this;
            enabled = false;
        }
        /// <summary>
        /// Добавить задачу в очередь на выполнения
        /// </summary>
        public static ITask Execute(Action action)
        {
            if(m_instance == null)
            {
                Debug.LogError("The \"ObjectLoader\" script instance is not attached to the GameObject");
                return null;
            }

            Task task = new Task(action);
            m_tasks.Enqueue(task);
          
            m_instance.enabled = true;
            return task;
        }
        /// <summary>
        /// Загрузить ассет из бандла и добавить задачу в очередь на выполнения
        /// </summary>
        public static ITask Execute<T>(Prefab<T> prefab, Action<T> action) where T:UnityEngine.Object
        {
            if (m_instance == null)
            {
                Debug.LogError("The \"ObjectLoader\" script instance is not attached to the GameObject");
                return null;
            }

            TaskPrefabLoader<T> task = new TaskPrefabLoader<T>(prefab, action);
            m_coroutines.AddLast(task);

            m_instance.enabled = true;
            return task;
        }
        /// <summary>
        /// Добавить задачу в очередь на выполнения
        /// </summary>
        public static ITask Execute(IEnumerator enumerator)
        {
            if (m_instance == null)
            {
                Debug.LogError("The \"ObjectLoader\" script instance is not attached to the GameObject");
                return null;
            }
            TaskCoroutine task = new TaskCoroutine(enumerator);
            m_coroutines.AddLast(task);
            m_instance.enabled = true;
            return task;
        }
        private void Update()
        {
            if (m_tasks.Count > 0 || m_coroutines.Count > 0)
            {
                long timeStart = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                var node = m_coroutines.First;
                while(node != null)
                {
                    var next = node.Next;
                    if (!node.Value.MoveNext())
                    {
                        m_tasks.Enqueue((IWorkTask)node.Value);
                        m_coroutines.Remove(node);
                    }
                    node = next;
                }

                while (m_tasks.Count > 0 && (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - timeStart) < TIME) 
                {
                    try
                    {
                        m_tasks.Dequeue().Invoke();
                    }
                    catch (Exception e) { Debug.LogError($"Error in Task: {e}"); }
                }

            }
            else enabled = false;
        }

        internal static void InstantiateImmediately()
        {
           while(m_tasks.Count > 0)
                 m_tasks.Dequeue()?.Invoke();
        }
    }
}

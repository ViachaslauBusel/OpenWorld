using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DATA
{
    /// <summary>
    /// База для создаваемых данных
    /// </summary>
    public abstract class EditableObject
    {
        [SerializeField] int m_id = 0;
        [SerializeField] protected string m_name;


       
        public int ID => m_id;
        public string Name => m_name;

        public void Initialize(int id)
        {
            if(this.m_id != 0)
            {
                Debug.LogError("error initializing the redactor object");
                return;
            }
            this.m_id = id;
        }
#if UNITY_EDITOR
        /// <summary>
        /// Доступно только в редакторе
        /// </summary>
        public abstract Texture2D Preview { get; }
        public abstract void Draw();
#endif
    }
}
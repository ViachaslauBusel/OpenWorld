using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DATA
{
    /// <summary>
    /// Контейнер для храненния данных
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class EditableObjectList<T> : ScriptableObject where T: EditableObject, new()
    {
        [SerializeField]
        protected List<T> container = new List<T>();
        [SerializeField] int generatorID = 0;

        public List<T> Container => container;



        public T Add()
        {
           int ID = ++generatorID;

            while (ConstainsKey(ID))ID++;
            T t = new T();
            t.Initialize(ID);
            container.Add(t);
            return t;
        }
        public int Count => container.Count;


        public T this[int index] => container[index];
 

        public void Remove(T t)
        {
            if (t == null) return;
            container.Remove(t);
        }

        public bool ConstainsKey(int id) => container.Any((i) => i.ID == id);

        /// <summary>
        /// Найти обьект по ИД
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T FindByID(int id) => container.Find((i) => i.ID == id);


    }
}
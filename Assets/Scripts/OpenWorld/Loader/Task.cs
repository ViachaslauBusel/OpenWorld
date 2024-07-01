using DATA;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenWorld.Loader
{
    public class Task : ITask, IWorkTask
    {
        private Action m_action;
        public bool Completed { get; private set; } = false;

        public Task(Action action)
        {
            this.m_action = action;
        }

        public void Invoke()
        {
            m_action?.Invoke();
            Completed = true;
        }

        public void Cancel()
        {
            m_action = null;
            Completed = true;
        }
    }
}
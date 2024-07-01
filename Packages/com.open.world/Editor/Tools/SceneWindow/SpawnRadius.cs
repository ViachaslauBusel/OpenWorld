#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace OpenWorldEditor.Tools.SceneWindow
{
    public class SpawnRadius: MonoBehaviour
    {
        public float Radius { get; set; }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawCube(transform.position, new Vector3(Radius, 1.6f, Radius));
        }
    }
}
#endif
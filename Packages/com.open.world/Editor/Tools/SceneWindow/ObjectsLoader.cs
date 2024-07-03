#if UNITY_EDITOR
using OpenWorldEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace OpenWorld.SceneWindow
{
    public abstract class ObjectsLoader
    {
        private Vector4 border;
        protected float _areaVisible = 300.0f;
        private float blockSize = 50.0f;
        private Vector3 trackingPosition = Vector3.zero;


        public void Initial()
        {
            blockSize = TabSetting.Map.TileSize;
            _areaVisible = TabSetting.MapSettings.AreaVisible * blockSize + (blockSize / 2.0f);
            CalculateBorder();
            CalculeteVisibleNPCs();
        }

        private void CalculateBorder()
        {
            border = new Vector4();
            CalculateBorderX();
            CalculateBorderY();

        }
        private void CalculateBorderX()
        {
            border.w = (trackingPosition.x - (trackingPosition.x % blockSize)) - blockSize * 0.1f;//Left
            border.y = (trackingPosition.x - (trackingPosition.x % blockSize)) + blockSize * 1.1f;//Right
        }
        private void CalculateBorderY()
        {
            border.x = (trackingPosition.z - (trackingPosition.z % blockSize)) - blockSize * 0.1f;//Down
            border.z = (trackingPosition.z - (trackingPosition.z % blockSize)) + blockSize * 1.1f;//Up
        }

        public void OnSceneGUI(UnityEditor.SceneView sceneView)
        {
            trackingPosition = sceneView.camera.transform.position;
            ChangeBlock();
        }

        private void ChangeBlock()
        {

            if (trackingPosition.x < border.w)//left
            {
                border.w -= blockSize;
                border.y -= blockSize;
                CalculeteVisibleNPCs();
            }
            else if (trackingPosition.x > border.y)//right
            {
                border.w += blockSize;
                border.y += blockSize;
                CalculeteVisibleNPCs();
            }
            else if (trackingPosition.z < border.x)//up
            {
                border.x -= blockSize;
                border.z -= blockSize;
                CalculeteVisibleNPCs();

            }
            else if (trackingPosition.z > border.z)//down
            {
                border.x += blockSize;
                border.z += blockSize;
                CalculeteVisibleNPCs();
            }

        }

        /// <summary>
        /// Если точка находится в пределах загруженной карты, возвращает true
        /// </summary>
        public bool IsMapArea(Vector3 position)
        {
            Vector2 center = Vector2.zero;
            center.x = border.w + blockSize / 2.0f;
            center.y = border.x + blockSize / 2.0f;
            Vector2 distance = Vector2.zero;
            distance.x = Mathf.Abs(center.x - position.x);
            distance.y = Mathf.Abs(center.y - position.z);
            return distance.x < _areaVisible && distance.y < _areaVisible;
        }
        public abstract void Dispose();

        public abstract void CalculeteVisibleNPCs();
    }
}
#endif
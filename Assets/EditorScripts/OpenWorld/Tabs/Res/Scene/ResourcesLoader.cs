#if UNITY_EDITOR
using OpenWorld.SceneWindow;
using System.Collections.Generic;
using UnityEngine;

namespace OpenWorldEditor.Tabs.Res
{
    public class ResourcesLoader: ObjectsLoader
    {
        public List<ResourceDrawGizmos> Resources { get; private set; } = new List<ResourceDrawGizmos>();

        public override void Dispose()
        {
            foreach (ResourceDrawGizmos obj in Resources) GameObject.DestroyImmediate(obj.gameObject);
            Resources.Clear();
        }

        public override void CalculeteVisibleNPCs()
        {
            //Dispose();
            //foreach (WorldResource worldResource in TabSetting.WorldResources.container)
            //{
            //    if (IsMapArea(worldResource.point))
            //    {
            //        GameObject prefabResource = TabSetting.Resources.FindByID(worldResource.id).GetStage(0)?.Prefab.Object;
            //        if (prefabResource != null)
            //        {
            //            GameObject obj = GameObject.Instantiate(prefabResource);
            //            obj.transform.position = worldResource.point;
            //            ResourceDrawGizmos resourceDraw = obj.AddComponent<ResourceDrawGizmos>();
            //            resourceDraw.Radius = worldResource.radius;
            //            resourceDraw.worldResource = worldResource;
            //            Resources.Add(resourceDraw);
            //        }
            //    }
            //}
        }
    }
}
#endif
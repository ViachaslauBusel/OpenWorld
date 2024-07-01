using DATA;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Bundles
{
    public static class BundlesManager 
    {

        public static bool IsReady { get; private set; } = false;
        private static List<AssetBundle> m_bundles = new List<AssetBundle>();
        private static Dictionary<int, AssetBundle> m_bundlesStorage = new Dictionary<int, AssetBundle>();



        public static void Add(AssetBundle bundle)
        {
            m_bundlesStorage.Add(bundle.name.GetHashCode(), bundle);
            m_bundles.Add(bundle);
        }
        public static void LoadingComplete() => IsReady = true;

        public static BundlesMap GetBundlesMap(string mapName)
        {
            AssetBundle map = m_bundles.Find(b => b.name.Equals(mapName.ToLower() + "/map"));
          //  AssetBundle light = bundles.Find(b => b.name.Equals(mapName.ToLower() + "/map(light)"));
            if (map == null) { Debug.LogError("Не удалось загрузить карту"); return null; }
         //   if(light == null) { Debug.LogError("Не удалось загрузить свет"); }

            return new BundlesMap(map);
        }

        public static AssetBundleRequest LoadAssetAsync(IPrefab prefab) 
        {
            if (!m_bundlesStorage.TryGetValue(prefab.BundleKEY, out AssetBundle bundle)) return null;
             return bundle.LoadAssetAsync(prefab.Path);
        }


        public static void Clear()
        {
            foreach(AssetBundle bundle in m_bundles) { bundle.Unload(true); }
            m_bundles.Clear();
            m_bundlesStorage.Clear();
            IsReady = false;
        }
    }
}
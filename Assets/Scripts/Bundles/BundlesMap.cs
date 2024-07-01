using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Bundles
{
    /// <summary>
    /// Бандлы с данными карты
    /// </summary>
    public class BundlesMap
    {
        private AssetBundle m_map;


        public BundlesMap(AssetBundle map)
        {
            this.m_map = map;

        }


        internal AssetBundleRequest LoadTileAsync<T>(string path)
        {
            return m_map.LoadAssetAsync<T>(path);
        }

    }
}

using Bundles;
using OpenWorld.DATA;
using OpenWorld.Loader;
using System.Collections;
using UnityEngine;


namespace OpenWorld
{
    public class MapLoader : MonoBehaviour
    {
        public static MapLoader Instance { get; private set; }
        private BundlesMap bundlesMap;
        public Transform trackingObj;
        public Map map;
        public Material waterMaterial;
        public Material terrainMaterial;


        public ITile[,] Tiles;
        private TileLocation[,] tilesLocations;
        private Vector4 border = new Vector4();
        public bool Ready { get; private set; } = false;

        private void Awake()
        {
            Instance = this;
            Application.backgroundLoadingPriority = ThreadPriority.Low;
            //QualitySettings.masterTextureLimit
           
            enabled = false;
        }

     
        public bool isDone {
            get {
                if (Tiles == null) return false;
                int tileCount = Tiles.GetLength(0) * Tiles.GetLength(1);//Всего тайлов
                int tileDone = GetTileDone();//Уже загруженные тайлы
                return tileCount == tileDone;
            } }
        public float progress {  get {
                if (Tiles == null) return 0.0f;
                int tileCount = Tiles.GetLength(0) * Tiles.GetLength(1);//Всего тайлов
                int tileDone = GetTileDone();//Уже загруженные тайлы
                return tileDone / (float)tileCount;
            } }

        private int GetTileDone()
        {
            int tileDone = 0;//Уже загруженные тайлы
            for (int x = 0; x < Tiles.GetLength(0); x++)
            {
                for (int y = 0; y < Tiles.GetLength(1); y++)
                {
                    if (Tiles[x, y] != null && !Tiles[x, y].name.Contains("Load"))
                    {
                        tileDone++;
                    }
                }
            } 
            return tileDone;
        }

        public void DestroyMap()
        {
            if (!Ready) return;
            LightmapSettings.lightmaps = null;
            enabled = false;
            Ready = false;
           
            for (int x = 0; x < Tiles.GetLength(0); x++)
            {
                for (int y = 0; y < Tiles.GetLength(1); y++)
                {
                       Tiles[x, y]?.Dispose();
                }
            }
            Tiles = null;
            tilesLocations = null;
        }


        public void LoadMap()
        {
            if (Ready) { Debug.LogError("the map is already loaded"); return; }
#if UNITY_EDITOR
          Initial();
         //  StartCoroutine(ELoadMap());
#else
            StartCoroutine(ELoadMap());
#endif
        }
        private IEnumerator ELoadMap()
        {

            Time.timeScale = 0.0f;

            yield return new WaitWhile(() => !BundlesManager.IsReady);
            bundlesMap = BundlesManager.GetBundlesMap(map.Name);


            Initial();
        }

        private void Initial()
        {
            LightmapSettings.lightmaps = null;
            Tiles = new ITile[GraphicsQualitySettings.AreaVisible * 2 + 1, GraphicsQualitySettings.AreaVisible * 2 + 1];
            tilesLocations = new TileLocation[GraphicsQualitySettings.AreaVisible * 2 + 1, GraphicsQualitySettings.AreaVisible * 2 + 1];

            CalculateBorder();

            Vector3 startPosition = trackingObj.position;

            if (startPosition.x < 0.0f) startPosition.x = 0.0f;
            if (startPosition.x > map.Size * 1000.0f) startPosition.x = map.Size * 1000.0f;
            if (startPosition.z < 0.0f) startPosition.z = 0.0f;
            if (startPosition.z > map.Size * 1000.0f) startPosition.z = map.Size * 1000.0f;

            TileLocation startLocation = new TileLocation(map);
            startLocation.Xkm = (int)(startPosition.x / 1000.0f);
            startLocation.Ykm = (int)(startPosition.z / 1000.0f);
            startLocation.Xtr = (int)((startPosition.x % 1000.0f) / map.TileSize);
            startLocation.Ytr = (int)((startPosition.z % 1000.0f) / map.TileSize);


            for (int x = 0; x < Tiles.GetLength(0); x++)
            {
                for (int y = 0; y < Tiles.GetLength(1); y++)
                {
                    tilesLocations[x, y] = startLocation;
                    tilesLocations[x, y].Xtr -= GraphicsQualitySettings.AreaVisible - x;
                    tilesLocations[x, y].Ytr -= GraphicsQualitySettings.AreaVisible - y;
                    CreateTerrain(x, y, tilesLocations[x, y]);
                }
            }
            Ready = true;
            enabled = true;
            Time.timeScale = 1.0f;
        }

        private void CalculateBorder()
        {
            CalculateBorderX();
            CalculateBorderY();
            
        }
        private void CalculateBorderX()
        {
        //    Debug.Log("Border X");
            border.w = (trackingObj.position.x - (trackingObj.position.x % map.TileSize)) - map.TileSize * 0.1f;//Left
            border.y = (trackingObj.position.x - (trackingObj.position.x % map.TileSize)) + map.TileSize * 1.1f;//Right
        }
        private void CalculateBorderY()
        {
         //   Debug.Log("Border Y");
            border.x = (trackingObj.position.z - (trackingObj.position.z % map.TileSize)) - map.TileSize * 0.1f;//Down
            border.z = (trackingObj.position.z - (trackingObj.position.z % map.TileSize)) + map.TileSize * 1.1f;//Up
        }

        private void Update()
        {
            ChangeBlock();
        }

        public void ChangeBlock()
        {
            Vector3 position = trackingObj.position;
#if UNITY_EDITOR
            position.x = Mathf.Clamp(position.x, 0.0f, map.Size * 1000.0f);
            position.z = Mathf.Clamp(position.z, 0.0f, map.Size * 1000.0f);
#endif

            if (trackingObj.position.x < border.w)//left
            {
                border.w -= map.TileSize;
                border.y -= map.TileSize;



                for (int x= Tiles.GetLength(0)-1; x>=0; x--)
                {
                    for(int y=0; y<Tiles.GetLength(1); y++)
                    {
                        tilesLocations[x, y].Xtr--;
                        if (x == Tiles.GetLength(0) - 1) Tiles[x, y]?.Dispose();
                        if (x != 0) Tiles[x, y] = Tiles[x - 1, y];
                        else CreateTerrain(x, y, tilesLocations[x,y]);
                    }
                }
              //  TaskPipeline.Execute(() => AssetBundle.UnloadAllAssetBundles(false));
            }
            else if(trackingObj.position.x > border.y)//right
            {
                border.w += map.TileSize;
                border.y += map.TileSize;


                for (int x = 0; x < Tiles.GetLength(0); x++)
                { 
                    for (int y = 0; y < Tiles.GetLength(1); y++)
                    {
                        tilesLocations[x, y].Xtr++;
                        if (x == 0) Tiles[x, y]?.Dispose();
                        if (x < Tiles.GetLength(0) - 1) Tiles[x, y] = Tiles[x + 1, y];
                        else CreateTerrain(x, y, tilesLocations[x, y]);
                    }
                }
              //  TaskPipeline.Execute(() => AssetBundle.UnloadAllAssetBundles(false));
            }
            else if(trackingObj.position.z < border.x)//up
            {
                border.x -= map.TileSize;
                border.z -= map.TileSize;


                for (int y = Tiles.GetLength(1) - 1; y >= 0; y--)
                {
                    for (int x = 0; x < Tiles.GetLength(0); x++)
                    {
                        tilesLocations[x, y].Ytr--;
                        if (y == Tiles.GetLength(1) - 1) Tiles[x, y]?.Dispose();
                        if (y != 0) Tiles[x, y] = Tiles[x, y-1];
                        else CreateTerrain(x, y, tilesLocations[x, y]);
                    }
                }
               // TaskPipeline.Execute(() => AssetBundle.UnloadAllAssetBundles(false));
            }
            else if(trackingObj.position.z > border.z)//down
            {
                border.x += map.TileSize;
                border.z += map.TileSize;

                for (int y = 0; y < Tiles.GetLength(1); y++)
                {
                    for (int x = 0; x < Tiles.GetLength(0); x++)
                    {
                        tilesLocations[x, y].Ytr++;
                        if (y == 0) Tiles[x, y]?.Dispose();
                        if (y < Tiles.GetLength(1) - 1) Tiles[x, y] = Tiles[x , y+1];
                        else CreateTerrain(x, y, tilesLocations[x, y]);
                    }
                }
              //  TaskPipeline.Execute(() => AssetBundle.UnloadAllAssetBundles(false));
            }

        }



        private void CreateTerrain(int x, int y, TileLocation location)
        {
           
            GameObject obj = new GameObject("LoadingTile");
#if UNITY_EDITOR
           Tiles[x, y] = obj.AddComponent<EditorTile>();
#else
            Tiles[x, y] = obj.AddComponent<GameTile>();
#endif
            obj.transform.SetParent(transform);

            if (location.Xkm < 0 || location.Xkm >= map.Size
             || location.Ykm < 0 || location.Ykm >= map.Size) { obj.name = "OutsideTile"; return; }
            

            Tiles[x, y].Load(bundlesMap, location, this);
           
        }

      
    }
}
#if UNITY_EDITOR
using OpenWorld;
using OpenWorld.DATA;
using OpenWorld.Utilities;
using System.IO;
using UnityEditor;

namespace OpenWorldEditor
{

    public class TerrainExport : IMapUtilityForMapTile
    {
        private BinaryWriter _stream_out;
        private float _terrainHeight;
        private GameMap _map;

        public string Name => "Export terrain.dat";

        public void BeginExecution(GameMap map)
        {
            _stream_out = new BinaryWriter(File.Open(@"Export/terrain.dat", FileMode.Create));
            _stream_out.Write(map.MapSizeKilometers);
            _stream_out.Write(map.TilesPerKilometer);//Тайлов на километр
            _map = map;
        }

        public bool Execute(MapTile tile, TileLocation location)
        {
            int x = location.Xkm * _map.TilesPerKilometer + location.Xtr;
            int y = location.Ykm * _map.TilesPerKilometer + location.Ytr;

            _stream_out.Write(x);
            _stream_out.Write(y);
            _stream_out.Write(tile.TerrainData.heightmapResolution);

            int size = (tile.TerrainData.heightmapResolution * tile.TerrainData.heightmapResolution) * sizeof(float);

            _stream_out.Write(size);//Размер массива высот

            float[,] heights = tile.TerrainData.GetHeights(0, 0, tile.TerrainData.heightmapResolution, tile.TerrainData.heightmapResolution);
            for (int i = 0; i < tile.TerrainData.heightmapResolution; i++)
            {
                for (int j = 0; j < tile.TerrainData.heightmapResolution; j++)
                {
                    _stream_out.Write(heights[i, j] * _terrainHeight);
                }
            }
            return false;
        }

        public void EndExecution(bool success)
        {
            _stream_out?.Close();
            string message = success ? "Экспорт выполнен" : "Ошибка экспорта";
            EditorUtility.DisplayDialog("Export terrain.dat", message, "ok");
        }
    }
}
#endif
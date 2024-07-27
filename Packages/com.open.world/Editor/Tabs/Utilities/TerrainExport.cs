#if UNITY_EDITOR
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

        public string Name => "Export terrain.dat";

        public void BeginExecution(GameMap map)
        {
            _stream_out = new BinaryWriter(File.Open(@"Export/terrain.dat", FileMode.Create));
            _stream_out.Write(map.MapSizeKilometers);
            _stream_out.Write(map.TilesPerKilometer);//Тайлов на километр
            _terrainHeight = map.MaximumWorldHeight;
            //_stream_out.Write(map.HeightmapResolution);
        }

        public bool Execute(MapTile mapElement)
        {
            _stream_out.Write(mapElement.TerrainData.heightmapResolution);
            int size = (mapElement.TerrainData.heightmapResolution * mapElement.TerrainData.heightmapResolution) * sizeof(float);
            _stream_out.Write(size);//Размер массива высот
            float[,] heights = mapElement.TerrainData.GetHeights(0, 0, mapElement.TerrainData.heightmapResolution, mapElement.TerrainData.heightmapResolution);
            for (int i = 0; i < mapElement.TerrainData.heightmapResolution; i++)
            {
                for (int j = 0; j < mapElement.TerrainData.heightmapResolution; j++)
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
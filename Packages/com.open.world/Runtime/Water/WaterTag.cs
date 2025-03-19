using UnityEngine;

namespace OpenWorld.Water
{
    public class WaterTag : MonoBehaviour
    {
        private MapLoader _map;
        private MeshRenderer _renderer;
        private MeshFilter _filter;

        public MeshRenderer Renderer => _renderer;
        public MeshFilter Filter => _filter;

        private void Awake()
        {
            Initialize(GetComponentInParent<MapLoader>());
        }

        private void Initialize(MapLoader map)
        {
            if (map == null)
            {
                Debug.LogError("WaterTag must be a child of MapLoader");
                return;
            }
            _map = map;
            _map.Water.Register(this);
            _renderer = GetComponent<MeshRenderer>();
            _filter = GetComponent<MeshFilter>();
        }

        public bool IsPointInWater(Vector3 point)
        {
            //Check by x and z axis
            if (point.x < _renderer.bounds.min.x || point.x > _renderer.bounds.max.x) return false;
            if (point.z < _renderer.bounds.min.z || point.z > _renderer.bounds.max.z) return false;
            return true;
        }

        private void OnDestroy()
        {
            _map?.Water.Unregister(this);
        }
    }
}

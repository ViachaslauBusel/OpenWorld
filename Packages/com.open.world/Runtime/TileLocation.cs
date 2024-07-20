using OpenWorld.DATA;
using UnityEngine;

namespace OpenWorld
{
    public struct TileLocation
    {
        private GameMap _map;
        // Map location in kilometers on the X-axis
        private int _xKilometers;
        // Map location in kilometers on the Y-axis
        private int _yKilometers;
        // Location within the block on the X-axis
        private int _xTileReference;
        // Location within the block on the Y-axis
        private int _yTileReference;


        public TileLocation(GameMap map)
        {
            _map = map;
            _xKilometers = 0;
            _yKilometers = 0;
            _xTileReference = 0;
            _yTileReference = 0;
        }

        public GameMap Map => _map;

        public int Xtr
        {
            get => _xTileReference;
            set => _xTileReference = AdjustTileReference(value, ref _xKilometers);
        }

        public int Ytr
        {
            get => _yTileReference;
            set => _yTileReference = AdjustTileReference(value, ref _yKilometers);
        }

        public int Xkm
        {
            get => _xKilometers;
            set => _xKilometers = Mathf.Clamp(value, 0, _map.MapSizeKilometers - 1);
        }

        public int Ykm
        {
            get => _yKilometers;
            set => _yKilometers = Mathf.Clamp(value, 0, _map.MapSizeKilometers - 1);
        }

        private int AdjustTileReference(int tileReference, ref int kmReference)
        {
            while (tileReference >= _map.TilesPerKilometer)
            {
                kmReference++;
                tileReference -= _map.TilesPerKilometer;
            }
            while (tileReference < 0)
            {
                kmReference--;
                tileReference += _map.TilesPerKilometer;
            }
            return tileReference;
        }

        public Vector3 Position => new Vector3((_xKilometers * 1000.0f) + (_xTileReference * _map.TileSize), 0.0f, (_yKilometers * 1000.0f) + (_yTileReference * _map.TileSize));
        public string Path => _map.GetPath(_xKilometers, _yKilometers, _xTileReference, _yTileReference);
        public string PathToLight => _map.GetPathToLight(_xKilometers, _yKilometers, _xTileReference, _yTileReference);

        public override bool Equals(object obj)
        {
            if (obj is TileLocation other)
            {
                return _xKilometers == other._xKilometers && _yKilometers == other._yKilometers && _xTileReference == other._xTileReference && _yTileReference == other._yTileReference;
            }
            return false;
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * 23 + _xKilometers.GetHashCode();
                hash = hash * 23 + _yKilometers.GetHashCode();
                hash = hash * 23 + _xTileReference.GetHashCode();
                hash = hash * 23 + _yTileReference.GetHashCode();
                return hash;
            }
        }

        public static bool operator ==(TileLocation left, TileLocation right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TileLocation left, TileLocation right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return $"Xkm: {Xkm}, Ykm: {Ykm}, Xtr: {Xtr}, Ytr: {Ytr}";
        }

    }
}
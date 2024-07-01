using OpenWorld.DATA;
using UnityEngine;

namespace OpenWorld
{
    public struct TileLocation
    {
        private Map map;
        private int _xKM;
        private int _yKM;
        private int _xTR;
        private int _yTR;

        public TileLocation(Map map)
        {
            this.map = map;
            _xKM = 0;
            _yKM = 0;
            _xTR = 0;
            _yTR = 0;
        }
        public int Xtr
        {
            get => _xTR;
            set
            {
                _xTR = value;
                while (_xTR >= map.TilesPerBlock)
                {
                    Xkm++;
                    _xTR -= map.TilesPerBlock;
                }
                while (_xTR < 0)
                {
                    Xkm--;
                    _xTR += map.TilesPerBlock;
                }
            }
        }
        public int Ytr
        {
            get => _yTR;
            set
            {
                _yTR = value;
                while (_yTR >= map.TilesPerBlock)
                {
                    Ykm++;
                    _yTR -= map.TilesPerBlock;
                }
                while (_yTR < 0)
                {
                    Ykm--;
                    _yTR += map.TilesPerBlock;
                }
            }
        }
        public int Xkm
        {
            get => _xKM;
            set => _xKM = value;
            /*  set
              {
                  _xKM = Mathf.Clamp(value, 0, map.mapSize - 1);
              }*/
        }
        public int Ykm
        {
            get => _yKM;
            set => _yKM = value;
        /*    set
            {
                _yKM = Mathf.Clamp(value, 0, map.mapSize - 1);
            }*/
        }

        public Vector3 Position => new Vector3((_xKM * 1000.0f) + (_xTR * map.TileSize), 0.0f, (_yKM * 1000.0f) + (_yTR * map.TileSize));
        public string Path => map.GetPath(_xKM, _yKM, _xTR, _yTR);
        public string PathToLight => map.GetPathToLight(_xKM, _yKM, _xTR, _yTR);

        public static bool operator ==(TileLocation left, TileLocation right)
        {
            return left.Xkm == right.Xkm && left.Ykm == right.Ykm && left.Xtr == right.Xtr && left.Ytr == right.Ytr;
        }
        public static bool operator !=(TileLocation left, TileLocation right)
        {
            return !(left == right);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override string ToString()
        {
            return $"Xkm:{Xkm} Ykm:{Ykm} Xtr:{Xtr} Ytr:{Ytr}";
        }
    }
}
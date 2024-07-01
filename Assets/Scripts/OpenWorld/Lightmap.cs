using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace OpenWorld
{
    public class Lightmap: ScriptableObject
    {
        public Texture2D lightmapColor;
        public Texture2D lightmapDir;
        public Texture2D shadowMask;
        public LightProbes lightProbes;
    }
}

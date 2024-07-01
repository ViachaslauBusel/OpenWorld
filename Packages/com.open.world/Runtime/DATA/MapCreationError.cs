using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWorld.DATA
{
    public enum MapCreationError
    {
        None = 0,
        InvalidName = 1,
        InvalidSize = 2,
        NameAlreadyExists = 3,
        MapCreationFailed = 4,
        UndefinedHeight = 5,
        IncorrectSpecifiedHeight = 6,
        AssetBundleSettingFailed = 7,
        UnknownError = 8
    }
}
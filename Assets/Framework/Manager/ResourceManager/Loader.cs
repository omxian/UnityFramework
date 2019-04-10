using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

interface Loader
{
    T LoadAsset<T>(string resPath) where T : UnityEngine.Object;
    void UnLoadAsset(string resPath);
}

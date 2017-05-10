using UnityEngine;
using System.Collections;
//SceneManager 的命名和系统的命名冲突需要修改
namespace Framework.Owen
{
    public class SceneManager : MonoSingleton<SceneManager>
    {
        private SceneManager()
        {
        }    
    }
}
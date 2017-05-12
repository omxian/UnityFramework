using UnityEngine;

namespace Unity.Framework
{
    public static class UITools
    {
        public static void DestoryAllChildren(Transform transform)
        {
            foreach (Transform child in transform)
            {
                Object.Destroy(child.gameObject);
            }
        }
    }
}

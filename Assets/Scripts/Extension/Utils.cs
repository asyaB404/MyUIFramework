using UnityEngine;
using Object = UnityEngine.Object;


public static class Utils
{
    /// <summary>
    ///     销毁一个transform下的所有子物体
    /// </summary>
    /// <param name="transform"></param>
    public static void DestroyAllChildren(this Transform transform)
    {
        for (var i = transform.childCount - 1; i >= 0; i--) Object.DestroyImmediate(transform.GetChild(i).gameObject);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityObject = UnityEngine.Object;

/// <summary>
/// 리소스 로드를 래핑
/// 나중에 어셋번들로 변경됨.
/// </summary>

public class ResourceManager 
{
    public static UnityObject Load(string path)
    {
        // 지금은 리스소 로드지만 추후에 어셋로드로 변경됨.
        return Resources.Load(path);
    }

    public static GameObject LoadAndInstantiate(string path)
    {
        UnityObject source = Load(path);
        if(source == null)
        {
            return null;
        }
        return GameObject.Instantiate(source) as GameObject;
    }
}

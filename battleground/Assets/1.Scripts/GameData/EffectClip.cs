using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 이펙트 프리팹과 경로와 타입등의 속성 데이터를 가지고 있게되며
/// 프리팹의 프리로딩(이펙트 풀) - 풀링을 위한 기능
/// 이펙트 인스턴스 기능도 가지고 있다 - 풀링과 연계해서 사용
/// </summary>

public class EffectClip : MonoBehaviour
{
    // 추후 속성은 같지만 다른 이펙트 클립이 있을수 있어서 분별용
    public int EffectID = 0;

    public int realID = 0;

    public EffectType effectType = EffectType.NORMAL;
    public GameObject effectPrefab = null;
    public string effectName = "";
    public string effectPath = "";
    public string effectFullPath = "";

    public EffectClip() {}

    public void PreLoad()
    {
        effectFullPath = effectPath + effectName;
        if (this.effectFullPath != string.Empty && effectPrefab == null)
        {
            this.effectPrefab = ResourceManager.Load(effectFullPath) as GameObject;
        }
    }

    public void ReleaseEffect()
    {
        if(this.effectPrefab != null)
        {
            this.effectPrefab = null;
        }
    }

    /// <summary>
    ///  원하는 위치에 인스턴스함.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public GameObject Instantiate(Vector3 pos)
    {
        if(this.effectPrefab == null)
        {
            PreLoad();
        }
        if (this.effectPrefab != null)
        {
            GameObject effect = GameObject.Instantiate(effectPrefab, pos, Quaternion.identity);
            return effect;
        }
        return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����Ʈ �����հ� ��ο� Ÿ�Ե��� �Ӽ� �����͸� ������ �ְԵǸ�
/// �������� �����ε�(����Ʈ Ǯ) - Ǯ���� ���� ���
/// ����Ʈ �ν��Ͻ� ��ɵ� ������ �ִ� - Ǯ���� �����ؼ� ���
/// </summary>

public class EffectClip : MonoBehaviour
{
    // ���� �Ӽ��� ������ �ٸ� ����Ʈ Ŭ���� ������ �־ �к���
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
    ///  ���ϴ� ��ġ�� �ν��Ͻ���.
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

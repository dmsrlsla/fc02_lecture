using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml;
using System.IO;

/// <summary>
/// ����Ʈ Ŭ�� ����Ʈ�� ����Ʈ �����̸� ��θ� ������ ������ �������а� ���� ����� ������ �ִ�.
/// </summary>

public class EffectData : BaseData
{
    public EffectClip[] effectClips = new EffectClip[0];

    public string clipPath = "Effects/";
    private string XmlFilePath = "";
    private string XmlFilePName = "effectData.xml";
    private string dataPath = "Data/effectData";
    // XML ������
    private const string EFFECT = "effect";
    private const string CLIP = "clip";

    private EffectData() { }
    // �о����, �����ϰ�, �����͸� �����ϰ�, ������, �����ϱ�
    public void LoadData()
    {
        XmlFilePath = Application.dataPath + dataDirectory;
        TextAsset asset = (TextAsset)Resources.Load(dataPath);
        if(asset == null || asset.text == null)
        {
            AddData("New Effect");
            return;
        }

        using (XmlTextReader reader = new XmlTextReader(new StringReader(asset.text)))
        {
            int currentID = 0;
            while(reader.Read())
            {
                if(reader.IsStartElement())
                {
                    switch(reader.Name) // XML�� �׸� ���� ���� ������
                    {
                        case "length":
                            // ���̸�ŭ ����Ʈ �ε�
                            int length = int.Parse(reader.ReadString());
                            names = new string[length];
                            effectClips = new EffectClip[length];
                            break;
                        case "id":
                            currentID = int.Parse(reader.ReadString());
                            effectClips[currentID] = new EffectClip();
                            effectClips[currentID].realID = currentID;
                            break;
                        case "name":
                            names[currentID] = reader.ReadString();
                            break;
                        case "effectType":
                            effectClips[currentID].effectType = (EffectType)Enum.Parse(typeof(EffectType), reader.ReadString());
                            break;
                        case "effectName":
                            effectClips[currentID].effectName = reader.ReadString();
                            break;
                        case "effectPath":
                            effectClips[currentID].effectPath = reader.ReadString();
                            break;
                    }
                }
            }
        }
    }

    public void SaveData()
    {
        using (XmlTextWriter xml = new XmlTextWriter(XmlFilePath + XmlFilePName, System.Text.Encoding.Unicode))
        {
            xml.WriteStartDocument();
            xml.WriteStartElement(EFFECT);
            xml.WriteElementString("length", GetDataCount().ToString());
            for(int i = 0; i<this.names.Length; i++)
            {
                EffectClip clip = effectClips[i];
                xml.WriteStartElement(CLIP);
                xml.WriteElementString("id", i.ToString());
                xml.WriteElementString("name", this.names[i]);
                xml.WriteElementString("effectType", clip.effectType.ToString());
                xml.WriteElementString("effectPath", clip.effectPath);
                xml.WriteElementString("effectName", clip.effectName);
                xml.WriteEndElement();
            }
            xml.WriteEndElement();
            xml.WriteEndDocument();
        }
    }

    public override int AddData(string newName)
    {
        if (this.names == null)
        {
            this.names = new string[] { name };
            this.effectClips = new EffectClip[] { new EffectClip() };
        }
        else
        {
            // �迭�� ũ�⸦ �ٲپ� �߰��ϴ� �Լ�.(Ŀ���� �Լ�, �� ����)
            this.names = ArrayHelper.Add(name, this.names);
            this.effectClips = ArrayHelper.Add(new EffectClip(), this.effectClips);
        }

        return GetDataCount();
    }

    public override void RemoveData(int index)
    {
        this.names = ArrayHelper.Remove(index, this.names);
        if (this.names.Length == 0)
        {
            this.names = null;
        }
        this.effectClips = ArrayHelper.Remove(index, this.effectClips);
    }

    public void ClearData()
    {
        foreach(EffectClip clip in this.effectClips)
        {
            clip.ReleaseEffect();
        }
        this.effectClips = null;
        this.names = null;
    }

    public EffectClip GetCopy(int index)
    {
        if(index<0 || index>= this.effectClips.Length)
        {
            return null;
        }
        EffectClip original = new EffectClip();
        EffectClip clip = new EffectClip();
        clip.effectFullPath = original.effectFullPath;
        clip.effectName = original.effectName;
        clip.effectType = original.effectType;
        clip.effectPath = original.effectPath;
        clip.realID = effectClips.Length;
        return clip;
    }

    /// <summary>
    /// ���ϴ� �ε����� ���� �ε��ؼ� ã���ش�.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public EffectClip GetClip(int index)
    {
        if (index < 0 || index >= this.effectClips.Length)
        {
            return null;
        }
        effectClips[index].PreLoad();
        return effectClips[index];
    }

    public override void Copy(int index)
    {
        this.names = ArrayHelper.Add(this.names[index], this.names);
        this.effectClips = ArrayHelper.Add(GetCopy(index), this.effectClips);
    }
}

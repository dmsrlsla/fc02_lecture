using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// data의 기본 클래스임
/// 공통적인 데이터를 가지고 있게 되는데, 이름으로 가지고 있다.
/// 데이터 개수와 이름의 목록 리스트를 얻을수 있다.
/// </summary>
public class BaseData : ScriptableObject
{
    public const string dataDirectory = "/9.ResourcesData/Resources";
    public string[] names = null; // 배열로만 가지고 있어도 됨

    public BaseData() // XML 기준
    {
    
    }

    public int GetDataCount()
    {
        int retValue = 0;
        if (this.names != null)
        {
            retValue = this.names.Length;
        }
        return retValue;
    }

    /// 툴에 출력하기위한 이름목록을 만들어주는 함수.
    ///
    public string[] GetNameList(bool showID, string filterWord ="")
    {
        string[] retList = new string[0];
        if (this.names == null)
        {
            return retList;
        }

        retList = new string[this.name.Length];

        for(int i = 0; i< this.name.Length; i++)
        {
            if(filterWord != "")
            {
                if(names[i].ToLower().Contains(filterWord.ToLower()) == false)
                {
                    continue;
                }
            }

            if(showID == false)
            {
                retList[i] = i.ToString() + " : " + this.name[i];
            }
            else
            {
                retList[i] = this.names[i];
            }
        }

        return retList;
    }

    public virtual int AddData(string newName)
    {
        return GetDataCount();
    }

    public virtual void RemoveData(int index)
    {
        
    }

    public virtual void Copy(int index)
    {

    }
}

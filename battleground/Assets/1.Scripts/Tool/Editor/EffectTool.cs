using UnityEngine;
using UnityEditor;
using System.Text;
using UnityObject = UnityEngine.Object;
/// <summary>
/// 이펙트 데이터를 가지고 있는 툴
/// </summary>
public class EffectTool : MonoBehaviour
{
    //UI 그리는데 필요한 변수들
    public int uiWidthLarge = 300;
    public int uiWidthMiddle = 200;
    int selection = 0;
    Vector2 SP1 = Vector2.zero;
    Vector2 SP2 = Vector2.zero;
}

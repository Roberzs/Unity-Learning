/****************************************************
	文件：ClickCubeOther.cs
	作者：Zhangying
	邮箱：zhy18125@gmail.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.EventSystems;

public class ClickCubeOther : MonoBehaviour, IPointerClickHandler
{
    /// <summary>
    /// 建议3D物体点击事件使用这种方式 相机需要挂载Physical Raycast 组件
    /// </summary>

    private int _index;

    private void Start()
    {
        _index = 0;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        ChangeColor();
    }

    public void ChangeColor()
    {
        if (_index == 0)
        {
            GetComponent<MeshRenderer>().material.SetColor("_Color", Color.black);
        }
        else
        {
            GetComponent<MeshRenderer>().material.SetColor("_Color", Color.white);
        }
        _index = _index == 0 ? 1 : 0;
    }
    
}

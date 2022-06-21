/****************************************************
	文件：ClickMouseCube.cs
	作者：Zhangying
	邮箱：zhy18125@gmail.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickMouseCube : MonoBehaviour
{
    private int _index;
    private GraphicRaycaster _graphicRaycaster;


    private void Start()
    {
        _index = 0;
        _graphicRaycaster = FindObjectOfType<GraphicRaycaster>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && !IsClickUI(Input.mousePosition))
        {
            ChangeColor();
        }
    }

    public void ChangeColor()
    {
        if (_index == 0)
        {
            GetComponent<MeshRenderer>().material.SetColor("_Color", Color.cyan);
        }
        else
        {
            GetComponent<MeshRenderer>().material.SetColor("_Color", Color.white);
        }
        _index = _index == 0 ? 1 : 0;
    }
    
    public bool IsClickUI(Vector3 inputPos)
    {
        // 当相机挂载PhysicsRaycaster 点击到3D物体也会返回True
        //return EventSystem.current.IsPointerOverGameObject();

        PointerEventData data = new PointerEventData(EventSystem.current);
        data.pressPosition = inputPos;
        data.position = inputPos;

        List<RaycastResult> results = new List<RaycastResult>();
        _graphicRaycaster.Raycast(data, results);
        return results.Count != 0;
    }
}

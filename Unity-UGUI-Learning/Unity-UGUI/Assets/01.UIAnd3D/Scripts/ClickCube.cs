/****************************************************
	文件：ClickCube.cs
	作者：Zhangying
	邮箱：zhy18125@gmail.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;

public class ClickCube : MonoBehaviour
{
	private int _index;

    private void Start()
    {
		_index = 0;
    }

    private void OnMouseDown()
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

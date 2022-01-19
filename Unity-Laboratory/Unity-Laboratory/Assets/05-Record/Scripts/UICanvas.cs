using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvas : MonoBehaviour
{
    public GameObject BottomRightPinObj;

    void Update()
    {
        BottomRightPinObj.SetActive(Input.GetKey(KeyCode.R));
    }
}

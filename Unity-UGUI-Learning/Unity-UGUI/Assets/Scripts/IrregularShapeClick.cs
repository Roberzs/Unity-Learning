using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IrregularShapeClick : MonoBehaviour
{
    void Start()
    {
        // ͼƬ�����λ��͸����С��0.1fʱ�����ε���¼�
        // ���Ƽ�ʹ�� ʹ��ʱ�����ͼƬRead/Writeѡ����������������Լ�ͼƬ�����ܴ����ͼ��
        GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
    }
}

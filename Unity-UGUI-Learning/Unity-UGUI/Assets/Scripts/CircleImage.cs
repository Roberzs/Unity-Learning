using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

public class CircleImage : Image
{
    private int segement;

    /// <summary>
    /// �ػ�Mesh
    /// </summary>
    /// <param name="toFill"></param>
    protected override void OnPopulateMesh(VertexHelper toFill)
    {
        toFill.Clear();

        // ��ȡImage�Ŀ����
        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;
        // ��ȡ����UV��Ϣ
        Vector4 uv = overrideSprite != null ? DataUtility.GetOuterUV(overrideSprite) : Vector4.zero;
        // ����Uv�����
        float uvWidth = uv.z - uv.x;
        float uvHeight = uv.w - uv.y;
        // ��ȡ��ͼ(UV)�����ĵ�
        Vector2 uvCenter = new Vector2(uvWidth / 2, uvHeight / 2);
        Vector2 converRatio = new Vector2(uvWidth / width, uvHeight / height);

        // ������뾶
        float radian = 2 * Mathf.PI / segement;
        float radius = width / 2;
    }
}

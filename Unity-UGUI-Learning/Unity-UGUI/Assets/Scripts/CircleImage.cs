using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

public class CircleImage : Image
{
    private int segement;

    /// <summary>
    /// 重绘Mesh
    /// </summary>
    /// <param name="toFill"></param>
    protected override void OnPopulateMesh(VertexHelper toFill)
    {
        toFill.Clear();

        // 获取Image的宽与高
        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;
        // 获取外层的UV信息
        Vector4 uv = overrideSprite != null ? DataUtility.GetOuterUV(overrideSprite) : Vector4.zero;
        // 计算Uv宽与高
        float uvWidth = uv.z - uv.x;
        float uvHeight = uv.w - uv.y;
        // 获取贴图(UV)的中心点
        Vector2 uvCenter = new Vector2(uvWidth / 2, uvHeight / 2);
        Vector2 converRatio = new Vector2(uvWidth / width, uvHeight / height);

        // 弧度与半径
        float radian = 2 * Mathf.PI / segement;
        float radius = width / 2;
    }
}

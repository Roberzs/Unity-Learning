using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class CircleImage : Image
{
    /// <summary>
    /// Ҫ���ֳɶ��ٸ���Ƭ
    /// </summary>
    [SerializeField]
    private int segements = 100;

    /// <summary>
    /// ��ʾռ��
    /// </summary>
    [SerializeField] [Range(0, 1)]
    private float fillPercent = 1;

    /// <summary>
    /// ��Բ����������ж���
    /// </summary>
    private List<Vector3> _vertexLst;

    private static Color GRAY_COLOR = Color.gray;

    /// <summary>
    /// �ػ�Mesh
    /// </summary>
    /// <param name="toFill"></param>
    protected override void OnPopulateMesh(VertexHelper toFill)
    {
        toFill.Clear();

        _vertexLst = new List<Vector3>();

        // ����Ҫ��ʾ����Ƭ����
        var realSegements = (int)(segements * fillPercent);

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
        float radian = 2 * Mathf.PI / segements;
        float radius = width / 2;

        // д��ԭ�㶥��
        UIVertex origin = new UIVertex();
        // ����ԭ����ɫ
        byte t = (byte)(255 * fillPercent);
        Color32 tempColor = (Color.white - GRAY_COLOR) * fillPercent;
        origin.color = new Color32((byte)(GRAY_COLOR.r + tempColor.r) , (byte)(GRAY_COLOR.g + tempColor.g), (byte)(GRAY_COLOR.b + tempColor.b), 255);

        Vector2 originPos = new Vector2((0.5f - rectTransform.pivot.x) * width, (0.5f - rectTransform.pivot.y) * height);
        Vector2 vertPos = Vector2.zero;
        origin.position = originPos;
        origin.uv0 = new Vector2(vertPos.x * converRatio.x + uvCenter.x, vertPos.y * converRatio.y + uvCenter.y);
        toFill.AddVert(origin);

        // д����������
        int vertexCnt = realSegements + 1;
        float curRadian = 0;
        for (int i = 0; i < segements + 1; i++)
        {
            float x = Mathf.Cos(curRadian) * radius;
            float y = Mathf.Sin(curRadian) * radius;
            curRadian += radian;

            UIVertex tempVertex = new UIVertex();

            if (fillPercent > 0 && i < vertexCnt)
            {
                tempVertex.color = color;
            }
            else
            {
                tempVertex.color = GRAY_COLOR;
            }

            
            tempVertex.position = new Vector2(x, y) + originPos;

            _vertexLst.Add(tempVertex.position);

            tempVertex.uv0 = new Vector2(x * converRatio.x + uvCenter.x, y * converRatio.y + uvCenter.y);
            toFill.AddVert(tempVertex);
        }
        // д��������
        var id = 1;
        for (int i = 0; i < segements; i++)
        {
            toFill.AddTriangle(id, 0, id + 1);
            id++;
        }

    }

    public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, eventCamera, out localPoint);


        return IsValidPoint(localPoint);
    }

    private bool IsValidPoint(Vector2 localPoint)
    {
        return GetCrossPointCnt(localPoint, _vertexLst) % 2 == 1;
    }

    private int GetCrossPointCnt(Vector2 localPoint, List<Vector3> vertexLst)
    {
        var vert1 = Vector3.zero;
        var vect2 = Vector3.zero;
        var count = vertexLst.Count;
        var ans = 0;

        for (int i = 0; i < count; i++)
        {
            vert1 = vertexLst[i];
            vect2 = vertexLst[(i + 1) % count];
            if (IsYInRange(localPoint, vert1, vect2))
            {
                //if (localPoint.x < vert1.x && localPoint.x < vect2.x)
                //{
                //    ans++;
                //}
                if (GetIntersectX(vert1, vect2, localPoint.y) > localPoint.x)
                {
                    ans++;
                }
            }
        }
        return ans;
    }

    private bool IsYInRange(Vector2 localPoint,Vector3 vert1, Vector3 vert2)
    {
        if (vert1.y > vert2.y)
        {
            return localPoint.y > vert2.y && localPoint.y < vert1.y;
        }
        else
        {
            return localPoint.y < vert2.y && localPoint.y > vert1.y;
        }
    }

    private float GetIntersectX(Vector3 vert1, Vector3 vert2, float y)
    {
        float k = (vert1.y - vert2.y) / (vert1.x - vert2.x);
        return vert1.x + (y - vert1.y) / k;
    }
}

#if UNITY_EDITOR
[CanEditMultipleObjects]
[CustomEditor(typeof(CircleImage))]
public class CircleImageEditor : UnityEditor.UI.ImageEditor
{

    private SerializedProperty _segements;
    private SerializedProperty _fillPercent;

    protected override void OnEnable()
    {
        base.OnEnable();
        _segements = serializedObject.FindProperty("segements");
        _fillPercent = serializedObject.FindProperty("fillPercent");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();


        EditorGUILayout.PropertyField(_segements);
        EditorGUILayout.Slider(_fillPercent, 0, 1, new GUIContent("Fill Percent"));

        serializedObject.ApplyModifiedProperties();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
#endif
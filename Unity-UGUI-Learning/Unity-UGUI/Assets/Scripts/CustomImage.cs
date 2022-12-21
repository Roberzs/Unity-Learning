using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// 不规则多边形点击策略

[RequireComponent(typeof(PolygonCollider2D))]
public class CustomImage : Image
{
    private PolygonCollider2D _polygon;
    public PolygonCollider2D Polygon
    {
        get
        {
            if (_polygon == null)
            {
                _polygon = GetComponent<PolygonCollider2D>();
            }
            return _polygon;
        }
    }

    public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        Vector3 point;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, screenPoint, eventCamera, out point);

        return Polygon.OverlapPoint(point);
    }
}

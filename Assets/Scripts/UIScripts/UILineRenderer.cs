using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILineRenderer : Graphic
{
    [SerializeField] Vector2Int gridSize;
    [SerializeField] List<Vector2> points;

    float height;
    float width;
    float unitHeight;
    float unitWidth;

    public float thickness = 10f;

    public void SetPoints(Vector2 point)
    {
        points.Add(point);
        SetVerticesDirty();
    }

    public void SetGridSize(Vector2Int size)
    {
        gridSize = size;
        SetVerticesDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        width = rectTransform.rect.width;
        height = rectTransform.rect.height;

        unitHeight = height / (float)gridSize.y;
        unitWidth = width / (float)gridSize.x;

        if (points.Count < 2)
        {
            return;
        }

        for (int i = 0; i < points.Count - 1; i++)
        {
            Vector2 start = points[i];
            Vector2 end = points[i + 1];

            DrawLineSegment(start, end, vh);
        }
    }

    public void DrawLineSegment(Vector2 start, Vector2 end, VertexHelper vh)
    {
        Vector2 dir = (end - start).normalized;
        Vector2 normal = new Vector2(-dir.y, dir.x);

        Vector2 offset = normal * thickness * 0.5f;

        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;

        vertex.position = new Vector3(start.x * unitWidth + offset.x, start.y * unitHeight + offset.y);
        vh.AddVert(vertex);

        vertex.position = new Vector3(start.x * unitWidth - offset.x, start.y * unitHeight - offset.y);
        vh.AddVert(vertex);

        vertex.position = new Vector3(end.x * unitWidth + offset.x, end.y * unitHeight + offset.y);
        vh.AddVert(vertex);

        vertex.position = new Vector3(end.x * unitWidth - offset.x, end.y * unitHeight - offset.y);
        vh.AddVert(vertex);

        int index = vh.currentVertCount - 4;
        vh.AddTriangle(index + 0, index + 1, index + 2);
        vh.AddTriangle(index + 2, index + 1, index + 3);
    }
}

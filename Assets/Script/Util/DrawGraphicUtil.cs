using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGraphicUtil
{
    private const float DURATION = 0.5f;

    private static List<Vector3> _posList = new List<Vector3>();

    public static void DrawRect(Vector3 centerPoint, Vector3 dir, Vector3 right, float width, float height, Color color)
    {
        _posList.Clear();
        dir.Normalize();
        right.Normalize();

        var point1 = centerPoint - right * width * 05f;
        var point2 = point1 + dir * height;
        var point3 = point2 + right * width;
        var point4 = point3 - dir * height;

        _posList.Add(point1);
        _posList.Add(point2);
        _posList.Add(point3);
        _posList.Add(point4);

        DrawPoints(_posList, color);
    }

    public static void DrawRect(Vector3 centerPoint, float width, float height, Color color)
    {
        _posList.Clear();

        var y = centerPoint.y;

        var halfW = width * 0.5f;
        var halfH = height * 0.5f;

        var point1 = centerPoint - new Vector3(halfW, y, halfH);
        var point2 = centerPoint - new Vector3(halfW, y, -halfH);
        var point3 = centerPoint + new Vector3(halfW, y, halfH);
        var point4 = centerPoint + new Vector3(halfW, y, -halfH);

        _posList.Add(point1);
        _posList.Add(point2);
        _posList.Add(point3);
        _posList.Add(point4);

        DrawPoints(_posList, color);
    }

    public static void DrawCircle(Vector3 point, float r, Color color)
    {
        _posList.Clear();
        Quaternion rotation = new Quaternion();
        Vector3 up = new Vector3(0, 0, r);

        for (int f = 0; f <= 360; f += 10)
        {
            rotation.eulerAngles = new Vector3(0, f, 0);
            _posList.Add(rotation * up + point);
        }

        DrawPoints(_posList, color);
    }

    public static void DrawSector(Vector3 centerPoint, Vector3 farPoint, float angle, float r, Color color)
    {
        _posList.Clear();

        _posList.Add(centerPoint);

        var dir = new Vector3(farPoint.x - centerPoint.x, 0, farPoint.z - centerPoint.z);
        dir = Quaternion.Euler(new Vector3(0, (-angle / 2), 0)) * dir;

        for (float f = 0; ; f += 10)
        {
            if (f > angle)
                f = angle;

            var dirOffset = Quaternion.Euler(new Vector3(0, f, 0)) * dir;
            _posList.Add(centerPoint + dirOffset);

            if (f == angle)
                break;
        }

        DrawPoints(_posList, color);
    }

    public static void DrawPoints(List<Vector3> posList, Color color, float duration = 10)
    {
        for (int i = 0; i < posList.Count; i++)
        {
            var next = i + 1;
            if (next == posList.Count) next = 0;

            Debug.DrawLine(posList[i], posList[next], color, duration);
        }

        _posList.Clear();
    }
}
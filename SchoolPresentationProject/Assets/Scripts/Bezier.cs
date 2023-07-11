using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezier : MonoBehaviour
{
    public static Vector3 GetBezier(float f, params Vector3[] points)
    {
        if(points.Length == 1) return points[0];

        List<Vector3> p = new();
        for (int i = 0; i < points.Length - 1; i++) p.Add(Vector3.Lerp(points[i], points[i + 1], f));

        while(p.Count != 1)
        {
            List<Vector3> temp = new();
            for (int i = 0; i < p.Count - 1; i++) temp.Add(Vector3.Lerp(p[i], p[i + 1], f));
            p = temp;
        }

        return p[0];
    }
}

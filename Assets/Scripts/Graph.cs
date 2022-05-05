using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FunctionLibrary;

//[ExecuteInEditMode]
public class Graph : MonoBehaviour
{
    [SerializeField, Range(10, 1000)]
    int resolution = 10;

    [SerializeField]
    FunctionName function;

    [SerializeField]
    Transform grid;

    [SerializeField]
    Transform pointPrefab;

    Transform[] points;

    private void Awake()
    {
        // Create points
        points = new Transform[resolution * resolution];
        var step = 2f / resolution;
        var scale = Vector3.one * step;
        for (int i = 0; i < points.Length; i++)
        {
            var p = points[i] = Instantiate(pointPrefab);
            p.localScale = scale;
            p.SetParent(transform);
        }
    }

    private void Update()
    {
        var t = Time.time;
        var step = 2f / resolution;
        Function f = GetFunction(function);
        var v = 0.5f * step - 1f;
        for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++)
        {
            if (x == resolution)
            {
                x = 0;
                z++;
                v = (z + 0.5f) * step - 1f;
            }
            var u = (x + 0.5f) * step - 1f;
            points[i].localPosition = f(u, v, t);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FunctionLibrary;

//[ExecuteInEditMode]
public class Graph : MonoBehaviour
{
    [SerializeField, Range(10, 100)]
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
        var pointPos = new Vector3(1, 1);
        for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++)
        {
            if (x == resolution)
            {
                x = 0;
                z++;
            }
            var p = points[i] = Instantiate(pointPrefab);
            p.localScale = scale;
            pointPos.x = (x + 0.5f) * step - 1f;
            pointPos.z = (z + 0.5f) * step - 1f;
            p.localPosition = pointPos;
            p.SetParent(transform);
        }
    }

    private void Update()
    {
        var t = Time.time;
        Function f = GetFunction(function);
        for (int i = 0; i < points.Length; i++)
        {
            var p = points[i];
            var pos = p.localPosition;
            pos.y = f(pos.x, pos.z, t);
            p.localPosition = pos;
        }
    }
}

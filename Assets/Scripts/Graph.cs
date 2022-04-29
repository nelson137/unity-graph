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

    [SerializeField]
    Transform lineHorizontalPrefab, lineVerticalPrefab;

    Transform[] points;

    private void Awake()
    {
        // Create grid
        Vector3 pos;
        for (int i = -10; i <= 10; i++)
        {
            // Create horizontal line with y=i
            var h = Instantiate(lineHorizontalPrefab);
            pos = h.localPosition;
            pos.y = i;
            h.localPosition = pos;
            h.SetParent(grid);
            // Create vertical line with x=i
            var v = Instantiate(lineVerticalPrefab);
            pos = v.localPosition;
            pos.x = i;
            v.localPosition = pos;
            v.SetParent(grid);
        }

        // Create points
        points = new Transform[resolution];
        var step = 2f / resolution;
        var scale = Vector3.one * step;
        var pointPos = new Vector3(1, 1);
        for (int i = 0; i < resolution; i++)
        {
            var p = points[i] = Instantiate(pointPrefab);
            p.localScale = scale;
            pointPos.x = (i + 0.5f) * step - 1f;
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
            pos.y = f(pos.x, t);
            p.localPosition = pos;
        }
    }
}

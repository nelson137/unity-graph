using UnityEngine;
using static FunctionLibrary;

public class GpuGraph : MonoBehaviour
{
    static readonly int resolutionId = Shader.PropertyToID("_Resolution");
    static readonly int positionsId = Shader.PropertyToID("_Positions");
    static readonly int stepId = Shader.PropertyToID("_Step");
    static readonly int timeId = Shader.PropertyToID("_Time");

    [SerializeField]
    ComputeShader computeShader;

    [SerializeField]
    Material material;

    [SerializeField]
    Mesh mesh;

    //const int maxResolution = 1000;

    [SerializeField, Range(10, 1000)]
    int resolution = 10;

    [SerializeField]
    FunctionName function;

    //[SerializeField, Min(0f)]
    //float smoothTransitionDuration = 1f;

    ComputeBuffer positionsBuffer;

    //bool isTransitioning;
    //float duration;
    //FunctionName fromFunc;

    public FunctionName GetCurrentFunctionName()
    {
        return function;
    }

    void OnEnable()
    {
        positionsBuffer = new ComputeBuffer(resolution * resolution, 3 * sizeof(float));
    }

    void OnDisable()
    {
        positionsBuffer.Release();
        positionsBuffer = null;
    }

    void Update()
    {
        float step = 2f / resolution;
        var kernelIndex = (int)function;
        // Update compute shader values
        computeShader.SetInt(resolutionId, resolution);
        computeShader.SetFloat(stepId, step);
        computeShader.SetFloat(timeId, Time.time);
        computeShader.SetBuffer(kernelIndex, positionsId, positionsBuffer);
        // Run kernel
        int groups = Mathf.CeilToInt(resolution / 8f);
        computeShader.Dispatch(kernelIndex, groups, groups, 1);
        // Update material values
        material.SetBuffer(positionsId, positionsBuffer);
        material.SetFloat(stepId, step);
        // Draw mesh
        var bounds = new Bounds(Vector3.zero, Vector3.one * (2f + 2f / resolution));
        Graphics.DrawMeshInstancedProcedural(mesh, 0, material, bounds, positionsBuffer.count);
    }

    public void SmoothTransitionTo(FunctionName toFunc)
    {
        //isTransitioning = true;
        //duration = 0f;
        //fromFunc = function;
        function = toFunc;
    }
}

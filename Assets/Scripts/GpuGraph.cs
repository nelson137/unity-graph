using UnityEngine;
using static FunctionLibrary;

public class GpuGraph : MonoBehaviour
{
    static readonly int resolutionId = Shader.PropertyToID("_Resolution");
    static readonly int positionsId = Shader.PropertyToID("_Positions");
    static readonly int stepId = Shader.PropertyToID("_Step");
    static readonly int timeId = Shader.PropertyToID("_Time");
    static readonly int transitionProgressId = Shader.PropertyToID("_TransitionProgress");

    [SerializeField]
    ComputeShader computeShader;

    [SerializeField]
    Material material;

    [SerializeField]
    Mesh mesh;

    public const int minResolution = 10;
    public const int maxResolution = 1000;

    [SerializeField, Range(10, maxResolution)]
    int resolution = 10;

    public int Resolution
    {
        get => resolution;
        set => resolution = value;
    }

    [SerializeField]
    FunctionName function;

    public FunctionName CurrentFunctionName
    {
        get => function;
    }

    [SerializeField, Min(0f)]
    float smoothTransitionDuration = 1f;

    ComputeBuffer positionsBuffer;

    bool isTransitioning;
    float duration;
    FunctionName fromFunc;

    void OnEnable()
    {
        positionsBuffer = new ComputeBuffer(maxResolution * maxResolution, 3 * sizeof(float));
        InitFunctionProperties();
    }

    void OnDisable()
    {
        positionsBuffer.Release();
        positionsBuffer = null;
    }

    void Update()
    {
        float step = 2f / resolution;
        if (isTransitioning)
        {
            duration += Time.deltaTime;
            float progress = Mathf.SmoothStep(0f, 1f, duration / smoothTransitionDuration);
            computeShader.SetFloat(transitionProgressId, progress);
            if (progress >= 1.0f)
            {
                isTransitioning = false;
            }
        }
        var kernelIndex = 7 * (int)(isTransitioning ? fromFunc : function) + (int)function;

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
        Graphics.DrawMeshInstancedProcedural(mesh, 0, material, bounds, resolution * resolution);
    }

    public void SmoothTransitionTo(FunctionName toFunc)
    {
        isTransitioning = true;
        duration = 0f;
        fromFunc = function;
        function = toFunc;
    }

    /*************************************************
     * Function Properties
     ************************************************/

    #region Function Properties

    void InitFunctionProperties()
    {
        InitProperties_SineWave();
        InitProperties_MultiSineWave();
        InitProperties_SimplexNoise();
        InitProperties_Ripple();
        InitProperties_Sphere();
        InitProperties_BandedSphere();
        InitProperties_BandedStarTorus();
    }

    /**
     * Sine Wave
     */

    void InitProperties_SineWave()
    {
        SineWave_Amplitude = 1f;
        SineWave_PeriodFactor = 1f;
    }

    static readonly int _SineWave_Amplitude_Id = Shader.PropertyToID("_SineWave_Amplitude");
    public float SineWave_Amplitude
    {
        set => computeShader.SetFloat(_SineWave_Amplitude_Id, value);
    }

    static readonly int _SineWave_PeriodFactor_Id = Shader.PropertyToID("_SineWave_PeriodFactor");
    public float SineWave_PeriodFactor
    {
        set => computeShader.SetFloat(_SineWave_PeriodFactor_Id, value);
    }

    /**
     * Multi Sine Wave
     */

    void InitProperties_MultiSineWave()
    {
        MultiSineWave_Wave1_Amplitude = 1f;
        MultiSineWave_Wave1_PeriodFactor = 1f;
        MultiSineWave_Wave2_Amplitude = 1f;
        MultiSineWave_Wave2_PeriodFactor = 1f;
        MultiSineWave_Wave3_Amplitude = 1f;
        MultiSineWave_Wave3_PeriodFactor = 1f;
    }

    // Wave 1

    static readonly int _MultiSineWave_Wave1_Amplitude_Id = Shader.PropertyToID(
        "_MultiSineWave_Wave1_Amplitude"
    );
    public float MultiSineWave_Wave1_Amplitude
    {
        set => computeShader.SetFloat(_MultiSineWave_Wave1_Amplitude_Id, value);
    }

    static readonly int _MultiSineWave_Wave1_PeriodFactor_Id = Shader.PropertyToID(
        "_MultiSineWave_Wave1_PeriodFactor"
    );
    public float MultiSineWave_Wave1_PeriodFactor
    {
        set => computeShader.SetFloat(_MultiSineWave_Wave1_PeriodFactor_Id, value);
    }

    // Wave 2

    static readonly int _MultiSineWave_Wave2_Amplitude_Id = Shader.PropertyToID(
        "_MultiSineWave_Wave2_Amplitude"
    );
    public float MultiSineWave_Wave2_Amplitude
    {
        set => computeShader.SetFloat(_MultiSineWave_Wave2_Amplitude_Id, value);
    }

    static readonly int _MultiSineWave_Wave2_PeriodFactor_Id = Shader.PropertyToID(
        "_MultiSineWave_Wave2_PeriodFactor"
    );
    public float MultiSineWave_Wave2_PeriodFactor
    {
        set => computeShader.SetFloat(_MultiSineWave_Wave2_PeriodFactor_Id, value);
    }

    // Wave 3

    static readonly int _MultiSineWave_Wave3_Amplitude_Id = Shader.PropertyToID(
        "_MultiSineWave_Wave3_Amplitude"
    );
    public float MultiSineWave_Wave3_Amplitude
    {
        set => computeShader.SetFloat(_MultiSineWave_Wave3_Amplitude_Id, value);
    }

    static readonly int _MultiSineWave_Wave3_PeriodFactor_Id = Shader.PropertyToID(
        "_MultiSineWave_Wave3_PeriodFactor"
    );
    public float MultiSineWave_Wave3_PeriodFactor
    {
        set => computeShader.SetFloat(_MultiSineWave_Wave3_PeriodFactor_Id, value);
    }

    /**
     * Simplex Noise
     */

    void InitProperties_SimplexNoise()
    {
        SimplexNoise_Speed = 0.75f;
    }

    static readonly int _SimplexNoise_Speed_Id = Shader.PropertyToID("_SimplexNoise_Speed");
    public float SimplexNoise_Speed
    {
        set => computeShader.SetFloat(_SimplexNoise_Speed_Id, value);
    }

    /**
     * Ripple
     */

    void InitProperties_Ripple()
    {
        Ripple_Amplitude = 1f;
    }

    static readonly int _Ripple_Amplitude_Id = Shader.PropertyToID("_Ripple_Amplitude");
    public float Ripple_Amplitude
    {
        set => computeShader.SetFloat(_Ripple_Amplitude_Id, value);
    }

    /**
     * Sphere
     */

    void InitProperties_Sphere()
    {
        Sphere_Speed = 1f;
        Sphere_MinRadius = 0f;
        Sphere_MaxRadius = 1f;
    }

    static readonly int _Sphere_Speed_Id = Shader.PropertyToID("_Sphere_Speed");
    public float Sphere_Speed
    {
        set => computeShader.SetFloat(_Sphere_Speed_Id, value);
    }

    static readonly int _Sphere_MinRadius_Id = Shader.PropertyToID("_Sphere_MinRadius");
    public float Sphere_MinRadius
    {
        set => computeShader.SetFloat(_Sphere_MinRadius_Id, value);
    }

    static readonly int _Sphere_MaxRadius_Id = Shader.PropertyToID("_Sphere_MaxRadius");
    public float Sphere_MaxRadius
    {
        set => computeShader.SetFloat(_Sphere_MaxRadius_Id, value);
    }

    /**
     * Banded Sphere
     */

    void InitProperties_BandedSphere()
    {
        BandedSphere_Vertical = 6f;
        BandedSphere_Horizontal = 4f;
    }

    static readonly int _BandedSphere_Vertical_Id = Shader.PropertyToID("_BandedSphere_Vertical");
    public float BandedSphere_Vertical
    {
        set => computeShader.SetFloat(_BandedSphere_Vertical_Id, value);
    }

    static readonly int _BandedSphere_Horizontal_Id = Shader.PropertyToID(
        "_BandedSphere_Horizontal"
    );
    public float BandedSphere_Horizontal
    {
        set => computeShader.SetFloat(_BandedSphere_Horizontal_Id, value);
    }

    /**
     * Banded Star Torus
     */

    void InitProperties_BandedStarTorus() { }

    #endregion

    /*************************************************
     * End Function Properties
     ************************************************/
}

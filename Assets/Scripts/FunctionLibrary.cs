using UnityEngine;
using static UnityEngine.Mathf;

public static class FunctionLibrary
{
    public delegate float Function(float x, float z, float t);

    [SerializeField]
    public enum FunctionName { SineWave, MultiSineWave, Ripple };
    static Function[] functions = { SineWave, MultiSineWave, Ripple };

    public static Function GetFunction(FunctionName name)
    {
        return functions[(int) name];
    }

    public static float SineWave(float x, float z, float t)
    {
        return Sin(PI * (x + z + t));
    }

    public static float MultiSineWave(float x, float z, float t)
    {
        float y = Sin(PI * (x + 0.5f * t));
        y += 0.5f * Sin(2 * PI * (z + t));
        y += Sin(PI * (x + z + 0.25f * t));
        return y * (1f / 2f);
    }

    public static float Ripple(float x, float z, float t)
    {
        float d = Sqrt(x * x + z * z);
        return Sin(PI * (4f * d - t)) / (2f + 10 * d);
    }
}

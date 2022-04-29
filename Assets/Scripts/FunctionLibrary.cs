using UnityEngine;
using static UnityEngine.Mathf;

public static class FunctionLibrary
{
    public delegate float Function(float x, float t);

    [SerializeField]
    public enum FunctionName { SineWave, MultiSineWave };
    static Function[] functions = { SineWave, MultiSineWave };

    public static Function GetFunction(FunctionName name)
    {
        return functions[(int) name];
    }

    public static float SineWave(float x, float t)
    {
        return Sin(PI * (x + t));
    }

    public static float MultiSineWave(float x, float t)
    {
        float y = Sin(PI * (x + 0.5f * t));
        y += 0.5f * Sin(2 * PI * (x + t));
        return y * (2f / 3f);
    }
}

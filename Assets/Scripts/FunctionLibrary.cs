using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Mathf;

public static class FunctionLibrary
{
    public delegate Vector3 Function(float u, float v, float t);

    [SerializeField]
    public enum FunctionName
    {
        SineWave,
        MultiSineWave,
        SimplexNoise,
        Ripple,
        Sphere,
        BandedSphere,
        BandedStarTorus
    };

    static Function[] functions =
    {
        SineWave,
        MultiSineWave,
        SimplexNoise,
        Ripple,
        Sphere,
        BandedSphere,
        BandedStarTorus
    };

    static FunctionLibrary()
    {
        _NoisePerm = new int[512];
        for (int i = 0; i < _NoisePerm.Length; i++)
        {
            _NoisePerm[i] = _NoiseP[i & 255];
        }

        _NoiseGrad3 = new Vector3[]
        {
            new Vector3(1, 1, 0),
            new Vector3(-1, 1, 0),
            new Vector3(1, -1, 0),
            new Vector3(-1, -1, 0),
            new Vector3(1, 0, 1),
            new Vector3(-1, 0, 1),
            new Vector3(1, 0, -1),
            new Vector3(-1, 0, -1),
            new Vector3(0, 1, 1),
            new Vector3(0, -1, 1),
            new Vector3(0, 1, -1),
            new Vector3(0, -1, -1),
        };
    }

    public static Function GetFunction(FunctionName name)
    {
        return functions[(int)name];
    }

    public static Vector3 SmoothTransition(
        float u,
        float v,
        float t,
        Function from,
        Function to,
        float progress
    )
    {
        return Vector3.LerpUnclamped(from(u, v, t), to(u, v, t), SmoothStep(0f, 1f, progress));
    }

    public static Vector3 SineWave(float u, float v, float t)
    {
        Vector3 p;
        p.x = u;
        p.y = Sin(PI * (u + v + t));
        p.z = v;
        return p;
    }

    public static Vector3 MultiSineWave(float u, float v, float t)
    {
        Vector3 p;
        p.x = u;
        p.y = Sin(PI * (u + 0.5f * t));
        p.y += 0.5f * Sin(2 * PI * (v + t));
        p.y += Sin(PI * (u + v + 0.25f * t));
        p.y *= (1f / 2.5f);
        p.z = v;
        return p;
    }

    // csharpier-ignore
    static readonly int[] _NoiseP =
    {
        151, 160, 137, 91, 90, 15, 131, 13, 201, 95, 96, 53, 194, 233, 7, 225, 140, 36, 103, 30, 69, 142, 8, 99, 37, 240, 21, 10, 23, 190, 6, 148, 247, 120, 234, 75, 0, 26, 197, 62, 94, 252, 219, 203, 117, 35, 11, 32, 57, 177, 33, 88, 237, 149, 56, 87, 174, 20, 125, 136, 171, 168, 68, 175, 74, 165, 71, 134, 139, 48, 27, 166, 77, 146, 158, 231, 83, 111, 229, 122, 60, 211, 133, 230, 220, 105, 92, 41, 55, 46, 245, 40, 244, 102, 143, 54, 65, 25, 63, 161, 1, 216, 80, 73, 209, 76, 132, 187, 208, 89, 18, 169, 200, 196, 135, 130, 116, 188, 159, 86, 164, 100, 109, 198, 173, 186, 3, 64, 52, 217, 226, 250, 124, 123, 5, 202, 38, 147, 118, 126, 255, 82, 85, 212, 207, 206, 59, 227, 47, 16, 58, 17, 182, 189, 28, 42, 223, 183, 170, 213, 119, 248, 152, 2, 44, 154, 163, 70, 221, 153, 101, 155, 167, 43, 172, 9, 129, 22, 39, 253, 19, 98, 108, 110, 79, 113, 224, 232, 178, 185, 112, 104, 218, 246, 97, 228, 251, 34, 242, 193, 238, 210, 144, 12, 191, 179, 162, 241, 81, 51, 145, 235, 249, 14, 239, 107, 49, 192, 214, 31, 181, 199, 106, 157, 184, 84, 204, 176, 115, 121, 50, 45, 127, 4, 150, 254, 138, 236, 205, 93, 222, 114, 67, 29, 24, 72, 243, 141, 128, 195, 78, 66, 215, 61, 156, 180
    };

    static int[] _NoisePerm;

    static Vector3[] _NoiseGrad3;

    /// <summary>
    /// Port of <c>class ClassicNoise</c> from Stefan Gustavson's Java implementation
    /// (<a href="https://weber.itn.liu.se/~stegu/simplexnoise/simplexnoise.pdf">link</a>, p. 8).
    /// <br />
    /// TODO: Investigate <a href="https://www.scratchapixel.com/lessons/procedural-generation-virtual-worlds/perlin-noise-part-2/perlin-noise-terrain-mesh">this c++ implementation</a>
    /// of Perlin Noise.
    /// </summary>
    public static Vector3 SimplexNoise(float u, float v, float t)
    {
        float scaledX = 1.7f * u;
        float scaledY = 1.7f * v;
        float scaledZ = 0.75f * t;

        // Find unit grid cell containing point
        static int fastFloor(float x) => x > 0f ? (int)x : (int)(x - 1f);
        int X = fastFloor(scaledX);
        int Y = fastFloor(scaledY);
        int Z = fastFloor(scaledZ);

        // Get relative coords of point within cell
        float x = scaledX - X;
        float y = scaledY - Y;
        float z = scaledZ - Z;

        // Wrap unit grid cell at 255
        X &= 255;
        Y &= 255;
        Z &= 255;

        // Calculate 8 hashed gradient indices
        int gi000 = _NoisePerm[X + _NoisePerm[Y + _NoisePerm[Z]]] % 12;
        int gi001 = _NoisePerm[X + _NoisePerm[Y + _NoisePerm[Z + 1]]] % 12;
        int gi010 = _NoisePerm[X + _NoisePerm[Y + 1 + _NoisePerm[Z]]] % 12;
        int gi011 = _NoisePerm[X + _NoisePerm[Y + 1 + _NoisePerm[Z + 1]]] % 12;
        int gi100 = _NoisePerm[X + 1 + _NoisePerm[Y + _NoisePerm[Z]]] % 12;
        int gi101 = _NoisePerm[X + 1 + _NoisePerm[Y + _NoisePerm[Z + 1]]] % 12;
        int gi110 = _NoisePerm[X + 1 + _NoisePerm[Y + 1 + _NoisePerm[Z]]] % 12;
        int gi111 = _NoisePerm[X + 1 + _NoisePerm[Y + 1 + _NoisePerm[Z + 1]]] % 12;

        // Calculate noise contributions from each of the 8 corners
        float n000 = Vector3.Dot(_NoiseGrad3[gi000], new Vector3(x, y, z));
        float n100 = Vector3.Dot(_NoiseGrad3[gi100], new Vector3(x - 1, y, z));
        float n010 = Vector3.Dot(_NoiseGrad3[gi010], new Vector3(x, y - 1, z));
        float n110 = Vector3.Dot(_NoiseGrad3[gi110], new Vector3(x - 1, y - 1, z));
        float n001 = Vector3.Dot(_NoiseGrad3[gi001], new Vector3(x, y, z - 1));
        float n101 = Vector3.Dot(_NoiseGrad3[gi101], new Vector3(x - 1, y, z - 1));
        float n011 = Vector3.Dot(_NoiseGrad3[gi011], new Vector3(x, y - 1, z - 1));
        float n111 = Vector3.Dot(_NoiseGrad3[gi111], new Vector3(x - 1, y - 1, z - 1));

        // Compute the fade curve value for x, y, and z
        static float fade(float x) => x * x * x * (x * (6f * x - 15f) + 10f);
        float fadeX = fade(x);
        float fadeY = fade(y);
        float fadeZ = fade(z);

        // Interpolate along x the contributions from each of the corners
        static float mix(float a, float b, float t) => (1f - t) * a + t * b; // TODO: replace with Lerp
        float nx00 = mix(n000, n100, fadeX);
        float nx01 = mix(n001, n101, fadeX);
        float nx10 = mix(n010, n110, fadeX);
        float nx11 = mix(n011, n111, fadeX);
        // Interpolate the results along y
        float nxy0 = mix(nx00, nx10, fadeY);
        float nxy1 = mix(nx01, nx11, fadeY);
        // Interpolate the results along z
        float nxyz = mix(nxy0, nxy1, fadeZ);

        return new Vector3(u, 0.75f * nxyz, v);
    }

    public static Vector3 Ripple(float u, float v, float t)
    {
        float d = Sqrt(u * u + v * v);
        Vector3 p;
        p.x = u;
        p.y = Sin(PI * (4f * d - t)) / (2f + 10 * d);
        p.z = v;
        return p;
    }

    public static Vector3 Sphere(float u, float v, float t)
    {
        Vector3 p;
        var r = 0.5f + 0.5f * Sin(PI * t);
        var s = r * Cos(0.5f * PI * v);
        p.x = s * Sin(PI * u);
        p.y = r * Sin(0.5f * PI * v);
        p.z = s * Cos(PI * u);
        return p;
    }

    public static Vector3 BandedSphere(float u, float v, float t)
    {
        Vector3 p;
        var r = 0.9f + 0.1f * Sin(PI * (6f * u + 4f * v + t));
        var s = r * Cos(0.5f * PI * v);
        p.x = s * Sin(PI * u);
        p.y = r * Sin(0.5f * PI * v);
        p.z = s * Cos(PI * u);
        return p;
    }

    public static Vector3 BandedStarTorus(float u, float v, float t)
    {
        Vector3 p;
        var r1 = 0.7f + 0.1f * Sin(PI * (6f * u + 0.5f * t));
        var r2 = 0.15f + 0.05f * Sin(PI * (8f * u + 4f * v + 2f * t));
        var s = r1 + r2 * Cos(PI * v);
        p.x = s * Sin(PI * u);
        p.y = r2 * Sin(PI * v);
        p.z = s * Cos(PI * u);
        return p;
    }
}

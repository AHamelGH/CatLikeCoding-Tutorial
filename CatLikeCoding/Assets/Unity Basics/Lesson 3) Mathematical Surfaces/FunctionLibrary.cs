using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Mathf;

public static class FunctionLibrary
{
    public delegate Vector3 Function(float u, float v, float t);

    public enum FunctionName { Wave, MultiWave, Ripple, Sphere, Torus }

    private static Function[] functions = { Wave, MultiWave, Ripple, Sphere, Torus };

    public static int FunctionCount => functions.Length;

    public static Function GetFunction(FunctionName name) => functions[(int)name];

    public static Vector3 Wave(float u, float v, float t)
    {
        Vector3 p;
        p.x = u;
        p.y = Sin(PI * (u + v + t));
        p.z = v;

        return p;
    }

    public static Vector3 MultiWave(float u, float v, float t)
    {
        Vector3 p;
        p.x = u;
        p.y = Sin(PI * (u + 0.5f * t));
        p.y += 0.5f * Sin(PI * 2f * (v + t));
        p.y += Sin(PI * (u + v + 0.25f * t));
        p.y *= (1f / 2.5f);
        p.z = v;

        return p;
    }

    public static Vector3 Ripple(float u, float v, float t)
    {
        float d = Sqrt(u * u + v * v);
        Vector3 p;
        p.x = u;
        p.y = Sin(PI * (4f * d - t));
        p.y /= 1f + 10f * d;
        p.z = v;

        return p;
    }

    public static Vector3 Sphere(float u, float v, float t)
    {
        //float r = 0.5f + 0.5f * Sin(PI * t);
        //float r = 0.9f + 0.1f * Sin(8f * PI * u);     //Vertical bands
        //float r = 0.9f + 0.1f * Sin(8f * PI * v);     //Horizontal bands
        float r = 0.9f + 0.1f * Sin(PI * (12f * u + 8f * v + t));    //Rotating bands
        float s = r * Cos(PI * 0.5f * v);
        Vector3 p;
        p.x = s * Sin(PI * u);
        p.y = r * Sin(PI * 0.5f * v);
        p.z = s * Cos(PI * u);

        return p;
    }

    public static Vector3 Torus(float u, float v, float t)
    {
        //float r1 = 0.75f;     //Normal ring torus
        //float r2 = 0.25f;

        float r1 = 0.7f + 0.1f * Sin(PI * (8f * u + 0.5f * t));
        float r2 = 0.15f + 0.05f * Sin(PI * (16f * u + 8f * v + 3f * t));
        float s = r1 + r2 * Cos(PI * v);
        Vector3 p;
        p.x = s * Sin(PI * u);
        p.y = r2 * Sin(PI * v);
        p.z = s * Cos(PI * u);

        return p;
    }

    public static FunctionName GetNextFunctionName(FunctionName name) =>
        (int)name < functions.Length - 1 ? name + 1 : 0;

    public static FunctionName GetRandomFunctionNameOtherThan(FunctionName name)
    {
        var choice = (FunctionName)Random.Range(1, functions.Length);
        return choice == name ? 0 : choice;
    }

    public static Vector3 Morph(float u, float v, float t, Function from, Function to, float progress)
    {
        return Vector3.LerpUnclamped(from(u, v, t), to(u, v, t), SmoothStep(0f, 1f, progress));
    }
}

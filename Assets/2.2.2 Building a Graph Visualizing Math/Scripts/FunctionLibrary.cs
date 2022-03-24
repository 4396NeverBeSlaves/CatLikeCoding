using UnityEngine;
using static UnityEngine.Mathf;

public static class FunctionLibrary
{
    public static float alpha;
    public static float beta;
    public static float gamma;
    public static Vector3 Wave(float u, float v, float t)
    {
        Vector3 vec = Vector3.zero;

        vec.x = u;
        vec.y = Sin(PI * (u + v + t));
        vec.z = v;

        return vec;
    }

    public static Vector3 MultiWave(float u, float v, float t)
    {
        Vector3 vec = Vector3.zero;

        float result = Sin(PI * (u + 0.5f * t));
        result += 0.5f * Sin(2 * PI * (v + t));
        result += 0.25f * Sin(PI * (u + v + 2 * t));

        vec.x = u;
        vec.y = result;
        vec.z = v;
        return vec;
    }

    public static Vector3 Ripple(float u, float v, float t)
    {
        Vector3 vec = Vector3.zero;

        float d = Sqrt(u * u + v * v);
        float y = Sin(PI * (4 * d + t)) / (1 + 10 * d);

        vec.x = u;
        vec.y = y;
        vec.z = v;

        return vec;
    }

    public static Vector3 Sphere(float u, float v, float t)
    {
        Vector3 vec = Vector3.zero;
        float theta = (u + 1) * 2 * PI;
        float phi = (v + 1) * PI;
        //float r = (Time.time * 10) %20 *0.1f -1f;
        float r = (1f - alpha)  + alpha * Sin(PI * (beta * u + gamma * v + t));


        vec.x = r * Sin(phi) * Cos(theta);
        vec.y = Cos(phi);
        vec.z = r * Sin(phi) * Sin(theta);

        return vec;
    }

    public static Vector3 Torus(float u, float v, float t)
    {
        Vector3 vec;
        float r1 = alpha * 0.875f + alpha * 0.125f * Sin(PI * (beta* 2f * u + gamma*v + 0.5f * t));
        float r2 = (1f - alpha) * 0.75f + (1f - alpha) * 0.25f * (Cos(PI * (beta*2f * u + gamma * v + 2f * t)));
        float s = r1 + r2 * Cos(PI * v);

        vec.x = s * Sin(PI * u);
        vec.y = r2 * Sin(PI * v);
        vec.z = s * Cos(PI * u);

        return vec;
    }

    public delegate Vector3 Function(float u, float v, float t);
    public enum FunctionName { Wave, MultiWave, Ripple, Sphere, Torus, End}

    static Function[] functions = { Wave, MultiWave, Ripple, Sphere, Torus };

    public static Function GetFunction(int idx)
    {
        return functions[idx];
    }
    public static Function GetFunction(FunctionName name)
    {
        return functions[(int)name];
    }
    public static FunctionName GetNextFunctionName(FunctionName name)
    {
        return name < FunctionName.End - 1 ? name + 1 : FunctionName.Wave;
    }

    public static FunctionName GetRandomFunctionName()
    {
        return (FunctionName)Random.Range(0, (int)FunctionName.End);
    }

    public static FunctionName GetDifferentRandomFunctionName(FunctionName name)
    {
        var randName = (FunctionName)Random.Range(1, (int)FunctionName.End);
        return randName == name ? FunctionName.Wave : randName;
    }

    public static Vector3 Morph(float u, float v, float t, Function from, Function to, float factor)
    {
        return Vector3.LerpUnclamped(from(u, v, t), to(u, v, t), factor);
    }
}
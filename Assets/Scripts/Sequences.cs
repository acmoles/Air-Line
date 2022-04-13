using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Sequences
{
    public static IEnumerator DoSequence(Turtle turtle, string commandString)
    {
        Type thisType = typeof(Sequences);
        MethodInfo methodInfo = thisType.GetMethod(commandString);
        //TODO check methodInfo is a valid method
        ParameterInfo[] parameters = methodInfo.GetParameters();
        object[] parametersArray = new object[] { turtle };
        yield return methodInfo.Invoke(null, parametersArray);
    }

    public static IEnumerator DoArc(Turtle turtle)
    {
        yield return new WaitForSeconds(1);
        Vector3 controlPoint = new Vector3(1.5f, 1.75f, 1f);
        Vector3 endPoint = new Vector3(0.5f, 2f, 0f);
        yield return turtle.QuadraticArc(controlPoint, endPoint);
    }


    public static IEnumerator DoSphere(Turtle turtle)
    {
        const int iterations = 32;
        yield return new WaitForSeconds(1);
        for (int i = 0; i < iterations; i++)
        {
            Vector3 target = UnityEngine.Random.onUnitSphere;
            yield return turtle.PointAt(target);
            yield return turtle.MoveToTarget(target);
            yield return turtle.SetCustomColor(NextColorStep(ref i, iterations, Color.cyan, Color.magenta));
        }
    }

    public static IEnumerator DoBraid(Turtle turtle)
    {
        Color[] colors = new Color[4];
        colors[0] = Color.cyan;
        colors[1] = Color.green;
        colors[2] = Color.blue;
        colors[3] = Color.magenta;
        int c = 0;

        for (int i = 0; i < 3; i++)
        {

            yield return turtle.Ld(.1f);
            yield return turtle.SetCustomColor(NextColor(colors, ref c));
            yield return turtle.Rd(.5f);
            yield return turtle.SetCustomColor(NextColor(colors, ref c));

            for (int j = 0; j < 2; j++)
            {
                yield return turtle.Rd(.6f);
                yield return turtle.SetCustomColor(NextColor(colors, ref c));
            }

            yield return turtle.Ld(.3f);
            yield return turtle.SetCustomColor(NextColor(colors, ref c));
            yield return turtle.Rd(.1f);
            yield return turtle.SetCustomColor(NextColor(colors, ref c));
            yield return turtle.Ld(.5f);
            yield return turtle.SetCustomColor(NextColor(colors, ref c));

            for (int j = 0; j < 2; j++)
            {
                yield return turtle.Ld(.6f);
                yield return turtle.SetCustomColor(NextColor(colors, ref c));
            }

            yield return turtle.Rd(.3f);
            yield return turtle.SetCustomColor(NextColor(colors, ref c));

        }
    }

    public static Color NextColor(Color[] colors, ref int index)
    {
        index++;
        return index >= colors.Length ? colors[index = 0] : colors[index];
    }

    public static Color NextColorStep(ref int index, int totalSteps, Color start, Color end)
    {
        index++;
        return Color.Lerp(start, end, (float)index / (float)totalSteps);
    }

    public static IEnumerator DoShape(Turtle turtle)
    {
        int c = 0;
        for (int i = 0; i < 3; i++)
        {

            for (int j = 0; j < 2; j++)
            {
                yield return turtle.SetCustomColor(NextColorStep(ref c, 36, Color.cyan, Color.magenta));
                yield return turtle.Td(.4f);
            }

            for (int j = 0; j < 2; j++)
            {
                yield return turtle.SetCustomColor(NextColorStep(ref c, 36, Color.cyan, Color.magenta));
                yield return turtle.Ld(.9f);
            }

            for (int j = 0; j < 2; j++)
            {
                yield return turtle.SetCustomColor(NextColorStep(ref c, 36, Color.cyan, Color.magenta));
                yield return turtle.Td(.4f);
            }

            for (int j = 0; j < 6; j++)
            {
                yield return turtle.SetCustomColor(NextColorStep(ref c, 36, Color.cyan, Color.magenta));
                yield return turtle.Rd(.3f);
            }

        }
    }

    public static IEnumerator DoTriangle(Turtle turtle)
    {
        yield return turtle.Segment(1f, 0f, 120f);
        yield return turtle.Segment(1f, 0f, 120f);
        yield return turtle.Segment(1f, 0f, 120f);
    }

    public static IEnumerator DoTest(Turtle turtle)
    {
        yield return new WaitForSeconds(1);
        yield return turtle.Move(0.5f);
        yield return turtle.Turn(80f);
        yield return turtle.SetColor(BrushColor.Blue);
        yield return turtle.SetSize(BrushSize.Large);
        yield return turtle.Move(0.6f);
        yield return turtle.Turn(-120f);
        yield return turtle.SetSize(BrushSize.Small);
        yield return turtle.Move(0.4f);
        yield return turtle.Turn(80f);
        yield return turtle.Dive(-40f);
        yield return turtle.SetColor(BrushColor.Green);
        yield return turtle.SetSize(BrushSize.Medium);
        yield return turtle.Move(0.4f);
        yield return new WaitForSeconds(1);
        // turtle.moveSpeed = 2;
        // turtle.rotateSpeed = 360;
        // yield return DoSpiral(turtle);
    }

    public static IEnumerator DoSpiral(Turtle turtle)
    {
        Debug.Log("Spiral Started!");
        yield return null;
        yield return new WaitForSeconds(4);
        for (int i = 0; i < 120; i++)
        {
            Color lerpedColor = Color.Lerp(Color.blue, Color.green, Mathf.PingPong(Time.time, 1));
            yield return turtle.SetCustomColor(lerpedColor);
            yield return turtle.Move(0.1f);
            yield return turtle.TurnInstant(5f);
            yield return turtle.DiveInstant(5f);
        }
        Debug.Log("Spiral Done!");
    }
}
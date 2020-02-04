using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using UnityEngine.Jobs;
using Unity.Collections;

public struct ColorJob : IJobParallelFor
{
    public NativeArray<Color> colors;

    public void Execute(int index)
    {
        colors[index] = Random.ColorHSV();
    }
}

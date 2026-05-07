using System;
using UnityEngine;

public static class PaddleEvents
{
    public static event Action<Paddle, int> OnCenterHit;

    public static void InvokeCenterHit(Paddle paddle, int count)
    {
        OnCenterHit?.Invoke(paddle, count);
    }
}
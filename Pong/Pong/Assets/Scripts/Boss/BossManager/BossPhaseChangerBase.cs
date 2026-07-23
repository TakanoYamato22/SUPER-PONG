using System;
using UnityEngine;

public abstract class BossPhaseChangerBase :
    MonoBehaviour
{
    public abstract bool PhaseChanged
    {
        get;
    }

    public abstract bool PhaseChanging
    {
        get;
    }

    public abstract void StartPhaseChange(
        Action onFinished = null
    );
}
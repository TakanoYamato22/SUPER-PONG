
using UnityEngine;

public class CreatureCaptureState : MonoBehaviour
{
    public bool IsCaptured { get; private set; }

    public void SetCaptured(bool captured)
    {
        IsCaptured = captured;
    }
}

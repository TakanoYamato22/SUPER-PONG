using UnityEngine;

public class RotateEffect : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0, 0, 3000 * Time.deltaTime);
    }
}
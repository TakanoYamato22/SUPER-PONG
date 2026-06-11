using UnityEngine;

public abstract class Paddle : MonoBehaviour
{
    public float speed = 5f;

    public void ResetPosition()
    {
        transform.position = new Vector2(transform.position.x, 0f);
    }

    public void Shrink(float duration)
    {
        var size = GetComponent<PaddleSizeController>();
        if (size != null)
            size.Shrink(duration);
    }
    [Header("Smash")]
    public float smashPower = 2f;
}

using UnityEngine;

public class SmashZone : MonoBehaviour
{
    public bool CanSmash { get; private set; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
        {
            CanSmash = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
        {
            CanSmash = false;
        }
    }
}
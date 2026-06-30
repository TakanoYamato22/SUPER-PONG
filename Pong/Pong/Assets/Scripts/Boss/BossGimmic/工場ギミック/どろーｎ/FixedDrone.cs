using UnityEngine;


public class FixedDrone : MonoBehaviour
{

    [SerializeField] private AudioClip itemSound;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            AudioSource.PlayClipAtPoint(itemSound, transform.position);
            Destroy(gameObject, 0.05f);
        }
    }
}